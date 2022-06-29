using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WebFormBuiler.Models;
using DALLib;
using ModelLib;
using Microsoft.Extensions.Configuration;

namespace WebFormBuiler.Controllers
{
    public class ProjectController : Controller
    {
        private readonly ILogger<ProjectController> _logger;
        public ModelLib.Models models = new ModelLib.Models();

        public ProjectController(ILogger<ProjectController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            mfbProject_Collection pList = new mfbProject_Collection();
            var result = models.GetProjectList(ref pList);

            return View(pList);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(mfbProject obj)
        {
            string pXML = "";
            pXML = ConfigurationLib.XMLLib.XmlSerialize<mfbProject>(obj);
            var result = models.SetProject("ADD", pXML);
            return RedirectToAction("Index");
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
