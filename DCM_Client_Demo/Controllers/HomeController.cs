using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using DCM_Client_Demo.Models;
using DCM_Client_Demo.Utils;
using Microsoft.AspNetCore.Authorization;

namespace DCM_Client_Demo.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IHttpClientDemo _httpClient;

    public HomeController(ILogger<HomeController> logger, IHttpClientDemo httpClient )
    {
        _logger = logger;
        _httpClient = httpClient;
    }

    public IActionResult Index()
    {
        return View();
    }

    [Authorize]
    public IActionResult Privacy()
    {
        return View();
    }
    
    [Authorize(Roles = "Admin")]
    public IActionResult Secured()
    {
        return View();
    }
    
    public IActionResult AccessDenied()
    {
        return View();
    }
    
    public IActionResult NoPermission()
    {
        return View();
    }
    
    public IActionResult ViewHospitals()
    {
        try
        {
            var hospitals =
                (List<Hospital>)_httpClient.GetAsync("Hospital/GetAllHospitals", typeof(List<Hospital>)).Result;
            if(!hospitals.Any())
                RedirectToAction("NoPermission");
            return View(hospitals);
        }
        catch (Exception)
        {
           return  RedirectToAction("AccessDenied");
        }

        return View();
    }
    
    public IActionResult ViewDoctors()
    {
        try{
            var docs =  (List<Doctor>)_httpClient.GetAsync("Hospital/GetAllDoctors",typeof(List<Doctor>)).Result;
            if(!docs.Any())
                RedirectToAction("NoPermission");
            return View(docs);
        }
        catch (Exception)
        {
           return RedirectToAction("AccessDenied");
        }
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
