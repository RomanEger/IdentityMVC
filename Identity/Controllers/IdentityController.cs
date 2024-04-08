using System.Security.Claims;
using Identity.Models;
using Identity.Models.Entities;
using Identity.Repository;
using Identity.Services;
using Identity.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;

namespace Identity.Controllers;

[Route("{action}")]
public class IdentityController : Controller
{
    private readonly IRepository<User> _repository;
    
    public IdentityController(IRepository<User> repository)
    {
        _repository = repository;
    }
    
    [HttpGet]
    public IActionResult Login() => View();

    [HttpPost]
    public async Task<IActionResult> Login(UserViewModel userViewModel)
    {
        var passwordHash = PasswordHash.EncodePasswordToBase64(userViewModel.Password);
        var p = await _repository.Get(x => x.Login == userViewModel.Login && x.Password == passwordHash);
        if (p is not null)
        {
            var claims = new List<Claim> { new Claim(ClaimTypes.Name, userViewModel.Login) };
            
            var claimsIdentity = new ClaimsIdentity(claims, "Cookies");

            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
            
            await HttpContext.SignInAsync(claimsPrincipal);
            
            return RedirectPermanent($"/");
        }
        
        return RedirectPermanent("/login");
    }

    [HttpPost]
    public async Task<IActionResult> Logout()
    {
        var context = HttpContext;
        await context.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectPermanent("/login");
    }
    
    [HttpGet]
    public IActionResult Registration() => View();


    [HttpPost]
    public async Task<IActionResult> Registration(UserViewModel userViewModel)
    {
        if(!ModelState.IsValid)
            return RedirectPermanent("/registration");
        var p = await _repository.Get(p => p.Login == userViewModel.Login);
        if (p is null)
        {
            var user = new User()
            {
                Login = userViewModel.Login,
                Password = userViewModel.Password
            };
            await _repository.Add(user);
                
            var claims = new List<Claim> { new Claim(ClaimTypes.Name, userViewModel.Login) };
                
            var claimsIdentity = new ClaimsIdentity(claims, "Cookies");
            
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));
                
            return RedirectPermanent($"/");
        }
        
        return RedirectPermanent("/registration");
    }
}