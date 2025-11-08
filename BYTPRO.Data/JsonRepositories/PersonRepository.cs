using BYTPRO.Data.JsonUoW;
using BYTPRO.Data.Models;

namespace BYTPRO.Data.JsonRepositories;

public class OrderRepository(IUnitOfWork uow) : IOrderRepository
{ 
    public void Add(Order order)
    {
        uow.Orders.Add(order);
    }

    public IEnumerable<Order> GetAll()
    {
        return uow.Orders.ToList();
    }
}