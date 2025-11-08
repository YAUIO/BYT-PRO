using BYTPRO.Data.JsonUoW;
using BYTPRO.Data.Models;

namespace BYTPRO.Data.JsonRepositories;

public class OrderRepository(IUnitOfWork uow) : IOrderRepository
{ 
    public async Task Add(Order order)
    {
        uow.Orders.Add(order);
        await uow.SaveChangesAsync();
    }

    public IEnumerable<Order> GetAll()
    {
        return uow.Orders.ToList();
    }
}