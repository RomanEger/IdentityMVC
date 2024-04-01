using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Identity.Controllers;

[Route("")]
public class HomeController : Controller
{
    
    [Authorize]
    public IActionResult Index()
    {
        return View();
    }
}