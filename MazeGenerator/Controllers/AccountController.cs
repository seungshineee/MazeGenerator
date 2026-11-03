using MazeGenerator.Models;
using MazeGenerator.Services;
using MazeGenerator.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace MazeGenerator.Controllers
{
    public class AccountController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IPasswordHasher<User> _hasher;
        public AccountController(AppDbContext context, IPasswordHasher<User> hasher)
        {
            _context = context;
            _hasher = hasher;
        }

        [HttpGet]
        public IActionResult Register() => View();

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var existing = _context.Users.FirstOrDefault(u =>
                u.Username.ToLower() == model.Username.ToLower());
            if (existing != null)
            {
                ModelState.AddModelError("", "Такой пользователь уже есть");
                return View(model);
            }

            var user = new User { Username = model.Username };
            user.Password = _hasher.HashPassword(user, model.Password);

            _context.Users.Add(user);
            _context.SaveChanges();

            await SignIn(user);
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult Login() => View();

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = _context.Users.FirstOrDefault(u =>
                u.Username.ToLower() == model.Username.ToLower());

            if (user == null ||
                _hasher.VerifyHashedPassword(user, user.Password, model.Password)
                    == PasswordVerificationResult.Failed)
            {
                ModelState.AddModelError("", "Неверный логин или пароль");
                return View(model);
            }

            await SignIn(user);
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }

        private async Task SignIn(User user)
        {
            var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id),
            new(ClaimTypes.Name, user.Username)
        };
            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            await HttpContext.SignInAsync(new ClaimsPrincipal(identity));
        }

        [Authorize]
        [HttpGet]
        public IActionResult ChangeCharacter()
        {
            var user = GetCurrentUser();
            if (user == null) return RedirectToAction("Login");

            var model = new SelectCharacterViewModel
            {
                Characters = _context.Character.ToList(),
                SelectId = user.CharacterId
            };
            return View(model);
        }

        [Authorize]
        [HttpPost]
        public IActionResult ChangeCharacter(int characterId)
        {
            var user = GetCurrentUser();
            if (user == null) return RedirectToAction("Login");

            if (_context.Character.Any(c => c.Id == characterId))
            {
                user.CharacterId = characterId;
                _context.SaveChanges();
            }
            return RedirectToAction("ChangeCharacter");
        }

        private User? GetCurrentUser()
        {
            var user = User.Identity?.Name;
            return _context.Users.Include(u => u.Results).FirstOrDefault(u => u.Username == user);
        }

        [Authorize]
        [HttpGet]
        public IActionResult Profile()
        {
            var user = GetCurrentUser();
            if (user == null) return RedirectToAction("Login");

            var best = user.Results
            .GroupBy(r => new { r.MazeType, r.Difficulty })
            .Select(g => g.OrderBy(r => r.TimeSeconds).First())
            .ToList();

            var model = new ProfileViewModel
            {
                Username = user.Username,
                Character = _context.Character.FirstOrDefault(x => x.Id == user.CharacterId),
                BestResults = best
            };
            return View(model);
        }
    }
}
