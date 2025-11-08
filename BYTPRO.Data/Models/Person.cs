namespace BYTPRO.Data.Models;

public abstract class Person(int id, string name, string surname, string phone, string email, string password)
{
    public int Id { get; set; } = id;
    public string Name { get; set; } = name;
    public string Surname { get; set; } = surname;
    public string Phone { get; set; } = phone;
    public string Email { get; set; } = email;
    public string Password { get; set; } = password;

    public virtual void Login()
    {
        Console.WriteLine($"User {Email} is logging in...");
    }
    public static bool CheckEmailExistence(string email)
    {
        Console.WriteLine($"Checking if email {email} exists...");
        return false; 
    }
}