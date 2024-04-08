using System.ComponentModel.DataAnnotations;

namespace Identity.ViewModels;

public class UserViewModel()
{
    [Required(ErrorMessage = "Логин не указан")]
    [StringLength(maximumLength: 20, MinimumLength = 4, ErrorMessage = "Длина логина должна быть от 4 до 20 символов")]
    public string Login { get; set; }
    
    [Required (ErrorMessage = "Пароль не указан")]
    [StringLength(maximumLength: 20, MinimumLength = 4, ErrorMessage = "Длина пароля должна быть от 4 до 20 символов")]
    public string Password { get; set; }
    
    [Compare("Password", ErrorMessage = "Пароли не совпадают")]
    [StringLength(maximumLength: 20, MinimumLength = 4, ErrorMessage = "Длина пароля должна быть от 4 до 20 символов")]
    public string ConfirmPassword { get; set; }
}