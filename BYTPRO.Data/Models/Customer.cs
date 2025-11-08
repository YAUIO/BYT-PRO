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
    DateTime registrationDate)
    : Person(id, name, surname, phone, email, password)
{
    public DateTime RegistrationDate { get; set; } = registrationDate;

    public const decimal LoyaltyDiscountPercentage = 0.03m;

    public Dictionary<string, OnlineOrder> OnlineOrders { get; set; } = new Dictionary<string, OnlineOrder>();
    
    public bool IsLoyal
    {
        get
        {
            bool yearsCondition = (DateTime.Now - RegistrationDate).TotalDays > (365.25 * 2);
            bool ordersCondition = SuccessfulOrders() > 12;
            return yearsCondition && ordersCondition;
        }
    }
    
    public void GrantLoyaltyStatuses()
    {
        if (IsLoyal)
        {
            Console.WriteLine($"Customer {Name} {Surname} has been granted loyalty status.");
        }
        else
        {
            Console.WriteLine($"Customer {Name} {Surname} is not yet eligible.");
        }
    }
    
    private int SuccessfulOrders()
    {
        return OnlineOrders.Values.Count(o => o.Status == OrderStatus.Completed);
    }
}