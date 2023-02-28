using Microsoft.AspNetCore.Mvc.Rendering;
using Rocky_Models.Models;

namespace Rocky_DataAccess.Repository.IRepositoty
{
    public interface IProductRepository : IRepository<Product>
    {
        void Update(Product product);

        IEnumerable<SelectListItem> GetAllDropdownList(string obj);
    }
}
