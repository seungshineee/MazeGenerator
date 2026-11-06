using MazeGenerator.Models;
using MazeGenerator.Models.Enums;
using MazeGenerator.Services;
using MazeGenerator.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace MazeGenerator.Controllers
{
    public class MazeController : Controller
    {
        public readonly AppDbContext _context;
        public readonly IMazeGenerator _generator;

        private const int PacmanSize = 14;
        private const int PacmanItems = 5;
        private const double OpenChance = 0.3;

        private static readonly string[] EnemyImages =
        {
            "/images/enemy1.png",
            "/images/enemy2.png",
            "/images/enemy3.png"
        };

        public MazeController(IMazeGenerator generator, AppDbContext context)
        {
            _context = context;
            _generator = generator;
        }

        [HttpGet]
        public IActionResult Play(DifficultType difficulty = DifficultType.Easy)
        {
            var user = GetCurrentUser();
            var (width, height) = GetSize(difficulty);

            var characterId = user?.CharacterId ?? 1;
            var character = _context.Character.FirstOrDefault(c => c.Id == characterId)
                            ?? _context.Character.First();

            var model = new MazeViewModel
            {
                Difficulty = difficulty,
                Width = width,
                Height = height,
                CharacterImage = character.ImagePath,
                BestTimeSeconds = GetBestTime(user, MazeType.Standart, difficulty)
            };

            return View(model);
        }

        [HttpGet]
        public IActionResult Generate(DifficultType difficulty = DifficultType.Easy)
        {
            var (width, height) = GetSize(difficulty);
            var maze = new Maze { Width = width, Height = height };
            var grid = _generator.Generate(maze);

            var cells = new List<object>();
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    var c = grid[x, y];
                    cells.Add(new { top = c.Top, right = c.Right, bottom = c.Bottom, left = c.Left });
                }
            }

            return Json(new { width, height, cells });
        }

        [HttpGet]
        public IActionResult Pacman()
        {
            var user = GetCurrentUser();

            var model = new PacmanViewModel
            {
                CharacterImage = GetCharacterImage(user),
                BestTime = GetBestTime(user, MazeType.Pacman, DifficultType.Easy)
            };

            return View(model);
        }

        [HttpGet]
        public IActionResult GeneratePacman()
        {
            var maze = new Maze { Width = PacmanSize, Height = PacmanSize };
            var grid = _generator.Generate(maze);
            MazeOpener.OpenWalls(grid, PacmanSize, PacmanSize, OpenChance);

            var random = new Random();
            var taken = new HashSet<(int, int)> { (0, 0) };

            var items = new List<object>();
            while (items.Count < PacmanItems)
            {
                int x = random.Next(PacmanSize), y = random.Next(PacmanSize);
                if (x + y < 5) continue;
                if (!taken.Add((x, y))) continue;
                items.Add(new { x, y });
            }

            var enemies = new List<object>();
            while (enemies.Count < EnemyImages.Length)
            {
                int x = random.Next(PacmanSize), y = random.Next(PacmanSize);
                if (x + y < 12) continue;
                if (!taken.Add((x, y))) continue;
                enemies.Add(new { x, y, image = EnemyImages[enemies.Count] });
            }

            return Json(new
            {
                width = PacmanSize,
                height = PacmanSize,
                cells = ToCells(grid, PacmanSize, PacmanSize),
                items,
                enemies
            });
        }

        [HttpPost]
        public IActionResult SaveResult([FromBody] SaveResultRequest request)
        {
            var user = GetCurrentUser();
            if (user == null)
                return Unauthorized();

            if (!Enum.TryParse<DifficultType>(request.Difficulty, out var difficulty)
                || request.TimeSeconds <= 0)
                return BadRequest();

            if (!Enum.TryParse<MazeType>(request.MazeType, out var mazeType))
                mazeType = MazeType.Standart;

            user.Results.Add(new LevelResults
            {
                MazeType = mazeType,
                Difficulty = difficulty,
                TimeSeconds = request.TimeSeconds,
                CompletedAt = DateTime.UtcNow
            });
            _context.SaveChanges();

            return Json(new { bestTime = GetBestTime(user, mazeType, difficulty) });
        }

        private static (int Width, int Height) GetSize(DifficultType difficulty) => difficulty switch
        {
            DifficultType.Easy => (10, 10),
            DifficultType.Medium => (20, 20),
            DifficultType.Hard => (35, 35),
            _ => (10, 10)
        };

        private static List<object> ToCells(Cell[,] grid, int width, int height)
        {
            var cells = new List<object>();
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    var c = grid[x, y];
                    cells.Add(new { top = c.Top, right = c.Right, bottom = c.Bottom, left = c.Left });
                }
            }
            return cells;
        }

        private User? GetCurrentUser()
        {
            var user = User.Identity.Name;
            return _context.Users.Include(u => u.Results).FirstOrDefault(u => u.Username == user);
        }

        private string GetCharacterImage(User? user)
        {
            var characterId = user?.CharacterId ?? 1;
            var character = _context.Character.FirstOrDefault(c => c.Id == characterId)
                            ?? _context.Character.FirstOrDefault();
            return character?.ImagePath ?? "";
        }

        private static double? GetBestTime(User? user, MazeType mazeType, DifficultType difficulty)
        {
            if (user == null) return null;

            return user.Results
                .Where(r => r.MazeType == mazeType && r.Difficulty == difficulty)
                .Select(r => (double?)r.TimeSeconds)
                .Min();
        }
    }
}
