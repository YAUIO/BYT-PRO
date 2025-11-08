using BYTPRO.Data.Models;
using BYTPRO.Data.Models.Orders;

namespace BYTPRO.Data.JsonRepositories;

public interface IOrderRepository
{
    public Task Add(Order order);

    public IEnumerable<Order> GetAll();
}