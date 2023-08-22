using DapperAdvancedAPI.Models;

namespace DapperAdvancedAPI.Repositories
{
    public interface IOrderRepository
    {
        Task CreateOrder(IEnumerable<OrderDetail> OrderDetails);
        Task<IEnumerable<OrderDetail>> OrderDetails();
    }
}
