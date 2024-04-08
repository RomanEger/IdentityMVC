using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace Identity.Models.Entities;

[Index("Login", IsUnique = true)]
public class User
{
    public Guid Id { get; set; }
    
    [MinLength(4)]
    [MaxLength(20)]
    public string Login { get; set; }
    
    [MinLength(4)]
    [MaxLength(100)]
    public string Password { get; set; }
}