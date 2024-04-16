using Microsoft.EntityFrameworkCore;

namespace Test_Respawn.DB;

public class CustomerRepository(AppDbContext context) : ICustomerRepository
{
    public void CreateCustomer(Customer customer)
    {
        context.Customer.Add(customer);
        context.SaveChanges();
    }

    public void DeleteCustomer(int id)
    {
        var customer = context.Customer.FirstOrDefault(c => c!.Id == id);
        if (customer != null)
        {
            context.Customer.Remove(customer);
        }

        context.SaveChanges();
    }

    public Customer? GetCustomer(int id)
    {
        return context.Customer.FirstOrDefault(c => c!.Id == id);
    }

    public IEnumerable<Customer> GetCustomers()
    {
        return context.Customer.AsEnumerable()!;
    }

    public Customer UpdateCustomer(Customer customer)
    {
        var cus = context.Customer
            .Include(t => t.Address)
            .FirstOrDefault(c => c!.Id == customer.Id);
        if (cus != null)
        {
            cus.FirstName = customer.FirstName ?? cus.FirstName;
            cus.LastName = customer.LastName ?? cus.LastName;
            cus.Email = customer.Email ?? cus.Email;
            cus.PhoneNumber = customer.PhoneNumber ?? cus.PhoneNumber;

            return cus;
        }

        return default!;
    }
}