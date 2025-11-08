using BYTPRO.Data.Models;

namespace BYTPRO.Data.JsonRepositories;

public interface IOrderRepository
{
    public void Add(Order order);

    public IEnumerable<Order> GetAll();
}