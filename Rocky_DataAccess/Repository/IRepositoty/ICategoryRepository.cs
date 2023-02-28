using Rocky_Models.Models;

namespace Rocky_DataAccess.Repository.IRepositoty
{
    public interface ICategoryRepository : IRepository<Category>
    {
        void Update(Category category);
    }
}
