using System.ComponentModel.DataAnnotations;

namespace Identity.Models;

public class Person()
{
    [Required]
    [StringLength(maximumLength: 20, MinimumLength = 4, ErrorMessage = "Длина логина должна быть от 4 до 20 символов")]
    public string Login { get; set; }
    
    [Required (ErrorMessage = "Пароль не указан")]
    public string Password { get; set; }
    
    [Compare("Password", ErrorMessage = "Пароли не совпадают")]
    public string ConfirmPassword { get; set; }
}