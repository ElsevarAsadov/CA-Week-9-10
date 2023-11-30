using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pronia.Data;
using Pronia.Models;
using System.Diagnostics;

namespace Pronia.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly AppDbContext DB;
        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        [ActivatorUtilitiesConstructor]
        public HomeController(AppDbContext context)
        {
            DB = context;
        }

        public IActionResult Index()
        {
            return View(DB.Products.Include(x=>x.Images).ToList());
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