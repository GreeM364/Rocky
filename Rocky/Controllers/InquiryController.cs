using Microsoft.AspNetCore.Mvc;

namespace Rocky.Controllers
{
    public class InquiryController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
