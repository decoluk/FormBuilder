using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WebFormBuiler.Models;
using DALLib;
using ModelLib;
using Microsoft.Extensions.Configuration;

namespace WebFormBuiler.Controllers
{
    public class EntityController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        public ModelLib.Models models = new ModelLib.Models();

        public EntityController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            mfbEntity_Collection pList = new mfbEntity_Collection();
            var result = models.GetEntityList(ref pList);

            return View(pList);
        }

        public IActionResult Create()
        {
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