using System.Security.Claims;
using Identity.Models;
using Identity.Models.Entities;
using Identity.Repository;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;

namespace Identity.Controllers;

[Route("{action}")]
public class IdentityController : Controller
{
    private IRepository<User> _repository;
    
    public IdentityController(IRepository<User> repository)
    {
        _repository = repository;
    }
    
    [HttpGet]
    public IActionResult Login() => View();

    [HttpPost]
    public async Task<IActionResult> Login(Person person)
    {
        var p = await _repository.Get(x => x.Login == person.Login && x.Password == person.Password);
        if (p is not null)
        {
            var claims = new List<Claim> { new Claim(ClaimTypes.Name, person.Login) };
            
            var claimsIdentity = new ClaimsIdentity(claims, "Cookies");

            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
            
            await HttpContext.SignInAsync(claimsPrincipal);
            
            var user = HttpContext.User.Identity;
            var b = user.IsAuthenticated;
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
    public async Task<IActionResult> Registration(Person person)
    {
        var p = await _repository.Get(p => p.Login == person.Login);
        if (ModelState.IsValid && p is null)
        {
            var user = new User()
            {
                Login = person.Login,
                Password = person.Password
            };
            await _repository.Add(user);
                
            var claims = new List<Claim> { new Claim(ClaimTypes.Name, person.Login) };
                
            var claimsIdentity = new ClaimsIdentity(claims, "Cookies");
            
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));
                
            return RedirectPermanent($"/");
        }
        

        return RedirectPermanent("/registration");
    }
}