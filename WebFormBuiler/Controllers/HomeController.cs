using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WebFormBuiler.Models;
using DALLib;
using ModelLib;
using Microsoft.Extensions.Configuration;

namespace WebFormBuiler.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        public ModelLib.Models models = new ModelLib.Models();
         
        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        { 
             
            var result = models.SetEntity("<XML><VALUE>hihihi</VALUE></XML>");
            return View();
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
}