using Microsoft.AspNetCore.Mvc;
using Rocky.Models;
using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Rocky.Data;
using Rocky.ViewModels;
using Rocky.Utility;

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
            List<ShoppingCart> shoppingCartsList = new List<ShoppingCart>();

            if (HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WC.SessionCart) != null &&
                HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WC.SessionCart).Count() > 0)
            {
                shoppingCartsList = HttpContext.Session.Get<List<ShoppingCart>>(WC.SessionCart);
            }

            DetailsVM detailsVM = new DetailsVM()
            {
                Product = _db.Products.Include(p => p.Category).FirstOrDefault(p => p.Id == Id),
                ExistsInCart = false
            };

            foreach (var item in shoppingCartsList)
            {
                if (item.ProductId == Id)
                {
                    detailsVM.ExistsInCart = true;
                }
            }

            return View(detailsVM);
        }
        [HttpPost,ActionName("Details")]
        public IActionResult DetailsPost(int Id)
        {
            List<ShoppingCart> shoppingCartsList = new List<ShoppingCart>();

            if (HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WC.SessionCart) != null &&
                HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WC.SessionCart).Count() > 0)
            {
                shoppingCartsList = HttpContext.Session.Get<List<ShoppingCart>>(WC.SessionCart);
            }
            shoppingCartsList.Add(new ShoppingCart {ProductId = Id});
            HttpContext.Session.Set(WC.SessionCart, shoppingCartsList);

            return RedirectToAction(nameof(Index));
        }

        public IActionResult RemoveFromCart(int Id)
        {
            List<ShoppingCart> shoppingCartsList = new List<ShoppingCart>();

            if (HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WC.SessionCart) != null &&
                HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WC.SessionCart).Count() > 0)
            {
                shoppingCartsList = HttpContext.Session.Get<List<ShoppingCart>>(WC.SessionCart);
            }

            var itemToRemove = shoppingCartsList.SingleOrDefault(i => i.ProductId == Id);

            if (itemToRemove != null)
            {
                shoppingCartsList.Remove(itemToRemove);
            }

            HttpContext.Session.Set(WC.SessionCart, shoppingCartsList);
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Privacy()
        {
            return View();
        }
    }
}