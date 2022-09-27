using Microsoft.AspNetCore.Mvc.Rendering;
using Rocky.Models;

namespace Rocky.ViewModels
{
    public class ProductVM
    {
        public Product Product { get; set; }
        public IEnumerable<SelectListItem> CategorySelectList { get; set; }
    }
}
