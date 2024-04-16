using Microsoft.EntityFrameworkCore;

namespace Test_Respawn.DB;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Address> Address => Set<Address>();
    public DbSet<Customer?> Customer => Set<Customer>();


    protected virtual void ConfigureEntities(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Customer>(e =>
        {
            e.HasKey(x => x.Id);
            e.Property(x => x.FirstName).IsRequired();
            e.Property(x => x.LastName).IsRequired();
            e.Property(x => x.Email).IsRequired();
            e.Property(x => x.PhoneNumber).IsRequired();
            e.HasOne(x => x.Address).WithMany().HasForeignKey(x => x.AddressId);
        });

        modelBuilder.Entity<Address>(e =>
        {
            e.HasKey(x => x.Id);
            e.Property(x => x.City).IsRequired();
            e.Property(x => x.PostalCode).IsRequired();
        });
    }
}