using Rocky.Models;

namespace Rocky.ViewModels
{
    public class ProductUserVM
    {
        public ProductUserVM()
        {
            ProductsList= new List<Product>();
        }

        public ApplicationUser ApplicationUser { get; set; }
        public List<Product> ProductsList { get; set; }
    }
}
