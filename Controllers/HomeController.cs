
using System.Reflection;
using Microsoft.AspNetCore.Mvc;
using FirmSheetApp.Models;


namespace FirmSheetApp.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        SetVersionInfo();

    }

    public IActionResult Privacy()
    {
        SetVersionInfo();

        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {

        SetVersionInfo();
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    private void SetVersionInfo()
    {
        var assembly = Assembly.GetExecutingAssembly();
        ViewBag.AppName = assembly.GetCustomAttribute<AssemblyProductAttribute>()?.Product ?? "FirmSheetApp";
        ViewBag.AppVersion = assembly.GetName().Version?.ToString(3) ?? "1.0.0";
    }
}

