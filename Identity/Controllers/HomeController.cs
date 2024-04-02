using Microsoft.AspNetCore.Mvc;

namespace Identity.Controllers;

[Route("")]
public class HomeController : Controller
{
    public IActionResult Index()
    {
        var user = HttpContext.User.Identity;
        if (user is null || !user.IsAuthenticated)
            return RedirectPermanent("/login");
        return View();
    }
}