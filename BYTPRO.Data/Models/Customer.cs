using BYTPRO.Data.JsonUoW;
using BYTPRO.Data.Models.Enums;
using BYTPRO.Data.Models.Orders;

namespace BYTPRO.Data.Models;

public class Customer(
    int id,
    string name,
    string surname,
    string phone,
    string email,
    string password,
    DateTime registrationDate,
    IUnitOfWork uow)
    : Person(id, name, surname, phone, email, password, uow)
{
    public DateTime RegistrationDate { get; set; } = registrationDate;

    public const decimal LoyaltyDiscountPercentage = 0.03m;

    public Dictionary<string, OnlineOrder> OnlineOrders { get; set; } = [];
    
    public bool IsLoyal
    {
        get
        {
            var yearsCondition = (DateTime.Today - RegistrationDate).TotalDays > (365.25 * 2);
            var ordersCondition = SuccessfulOrders() > 12;
            return yearsCondition && ordersCondition;
        }
    }
    
    public void GrantLoyaltyStatuses()
    {
        Console.WriteLine(IsLoyal
            ? $"Customer {Name} {Surname} has been granted loyalty status."
            : $"Customer {Name} {Surname} is not yet eligible.");
    }
    
    private int SuccessfulOrders() // TODO move order store logic to repositories and persistence, out of Models
    {
        return OnlineOrders.Values.Count(o => o.Status == OrderStatus.Completed);
    }
}