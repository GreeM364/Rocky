using Microsoft.AspNetCore.Mvc;
using Rocky.Models;
using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Rocky.Data;
using Rocky.ViewModels;

namespace Rocky.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _db;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext db)
        {
            _logger = logger;
            _db = db;
        }

        public IActionResult Index()
        {
            HomeVM homeVm = new HomeVM()
            {
                Products = _db.Products.Include(c => c.Category),
                Categories = _db.Categories
            };

            return View(homeVm);
        }

        public IActionResult Details(int Id)
        {
            DetailsVM detailsVM = new DetailsVM()
            {
                Product = _db.Products.Include(p => p.Category).FirstOrDefault(p => p.Id == Id),
                ExistsInCart = false
            };
            return View(detailsVM);
        }

        public IActionResult Privacy()
        {
            return View();
        }
    }
}