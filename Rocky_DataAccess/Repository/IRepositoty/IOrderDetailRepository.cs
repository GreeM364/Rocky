using Rocky_Models.Models;

namespace Rocky_DataAccess.Repository.IRepositoty
{
    public interface IOrderDetailRepository : IRepository<OrderDetail>
    {
        void Update(OrderDetail orderDetail);
    }
}
