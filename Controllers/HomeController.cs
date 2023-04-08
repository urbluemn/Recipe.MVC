using System;
using System.Diagnostics;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Recipe.MVC.Models;

namespace Recipe.MVC.Controllers;

[Route("[controller]")]
public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    [Route("~/")]
    [Route("~/Home")]
    public IActionResult Index()
    {
        return View();
    }

    [Authorize]
    [Route("[action]")]
    public IActionResult LoginInfo()
    { 
        return View();
    }

    [Route("[action]")]
    public IActionResult Privacy()
    {
        return View();
    }

    [Authorize]
    [Route("[action]")]
    public IActionResult Login()
    {
        var parameters = new AuthenticationProperties{
            RedirectUri = "/Home"
        };
        return Challenge(parameters, OpenIdConnectDefaults.AuthenticationScheme);
    }

    [Authorize]
    [Route("[action]")]
    public IActionResult Logout()
    {
        var parameters = new AuthenticationProperties{
            RedirectUri = "/Home"
        };
        return SignOut(parameters,
        CookieAuthenticationDefaults.AuthenticationScheme,
        OpenIdConnectDefaults.AuthenticationScheme);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    [Route("[action]")]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
