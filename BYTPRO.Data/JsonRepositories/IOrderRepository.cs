using BYTPRO.Data.Models;

namespace BYTPRO.Data.JsonRepositories;

public interface IOrderRepository
{
    public Task Add(Order order);

    public IEnumerable<Order> GetAll();
}