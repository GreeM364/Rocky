using Rocky_Models.Models;

namespace Rocky_DataAccess.Repository.IRepositoty
{
    public interface IOrderHeaderRepository : IRepository<OrderHeader>
    {
        void Update(OrderHeader orderHeader);
    }
}
