namespace Test_Respawn.DB;

public interface ICustomerRepository
{
    void CreateCustomer(Customer customer);
    void DeleteCustomer(int id);
    Customer? GetCustomer(int id);
    IEnumerable<Customer> GetCustomers();
    Customer UpdateCustomer(Customer customer);
}