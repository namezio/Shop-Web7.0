using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Shop_Web7._0_WebApp.Models;
using WebApp.Database;

namespace Shop_Web7._0_WebApp.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    readonly WebAppEntities _entities = new WebAppEntities();
    
    public IActionResult Index()
    {
        var user = _entities.Users.ToList();
        var model = user.Select(x => new UsersModel()
        {
            email = x.Email,
            name = x.FullName,
            gender = x.Gender,
            username = x.UserName
        }).ToList();
        return View(model);
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