using System;
using System.Diagnostics;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Recipe.MVC.Models;

namespace Recipe.MVC.Controllers;

[Route("[controller]/[action]")]
public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    [Route("")]
    [Route("~/")]
    [Route("~/Home")]
    public IActionResult Index()
    {
        return View();
    }

    [Authorize]
    public IActionResult LoginInfo()
    {
        return View();
    }

    [Authorize]
    public IActionResult Login()
    {
        var parameters = new AuthenticationProperties{
            RedirectUri = "/Home/Index"
        };
        return SignIn(User, parameters);
    }

    [Authorize]
    public IActionResult Logout()
    {
        // var parameters = new AuthenticationProperties{
        //     RedirectUri = "/Home/Index"
        // };
        return SignOut(
        CookieAuthenticationDefaults.AuthenticationScheme,
        OpenIdConnectDefaults.AuthenticationScheme);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
