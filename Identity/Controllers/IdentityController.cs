using System.Security.Claims;
using Identity.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;

namespace Identity.Controllers;

[Route("{action}")]
public class IdentityController : Controller
{
    private List<Person> _persons = new List<Person>()
    {
        new Person()
        {
            Login = "test",
            Password = "1234"
        }
    };
    
    [HttpGet]
    public IActionResult Login() => View();

    [HttpPost]
    public async Task<IActionResult> Login(Person person)
    {
        if (_persons.Any(p => p.Login == person.Login && p.Password == person.Password))
        {
            var claims = new List<Claim> { new Claim(ClaimTypes.Name, person.Login) };
            
            var claimsIdentity = new ClaimsIdentity(claims, "Cookies");

            var context = HttpContext;
            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
            
            await context.SignInAsync(claimsPrincipal);
            
            var user = context.User.Identity;
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
        if (ModelState.IsValid && _persons.All(p => p.Login != person.Login))
        {
            _persons.Add(person);
                
            var claims = new List<Claim> { new Claim(ClaimTypes.Name, person.Login) };
                
            var claimsIdentity = new ClaimsIdentity(claims, "Cookies");

            var context = HttpContext;
            
            await context.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));
                
            return RedirectPermanent($"/");
        }
        

        return RedirectPermanent("/registration");
    }
}