using System.ComponentModel.DataAnnotations;

namespace MazeGenerator.ViewModels
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Введите логин")]
        [StringLength(20, MinimumLength = 3, ErrorMessage = "Логин от 3 до 20 символов")]
        [Display(Name = "Логин")]
        public string Username { get; set; } = "";

        [Required(ErrorMessage = "Введите пароль")]
        [MinLength(6, ErrorMessage = "Пароль минимум 6 символов")]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль")]
        public string Password { get; set; } = "";

        [Compare("Password", ErrorMessage = "Пароли не совпадают")]
        [DataType(DataType.Password)]
        [Display(Name = "Подтвердите пароль")]
        public string ConfirmPassword { get; set; } = "";
    }
}
