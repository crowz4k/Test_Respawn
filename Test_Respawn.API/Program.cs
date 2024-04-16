using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Test_Respawn.DB;
using static Microsoft.AspNetCore.Http.Results;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

var svc = builder.Services;

svc.AddEndpointsApiExplorer();
svc.AddSwaggerGen();
svc.AddScoped<ICustomerRepository, CustomerRepository>();


var connectionString =
    "Server=localhost;Database=Test_Respawn;User Id=sa;Password=roottoor!1;TrustServerCertificate=true;";
builder.Services.AddDbContext<AppDbContext>(opt =>
{
    opt.UseSqlServer(
        connectionString,
        sql =>
        {
            sql.MigrationsAssembly(typeof(AppDbContext).GetTypeInfo().Assembly.ToString());
            sql.EnableRetryOnFailure();
        });
    opt.EnableSensitiveDataLogging();
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("api/customer/{id:int}", (ICustomerRepository repo, int id) =>
{
    var cus = repo.GetCustomer(id);
    return cus == null ? NotFound($"Customer with provided id {id} not found.") : Ok(cus);
});

app.MapGet("api/customer/all", (ICustomerRepository repo) => Ok(repo.GetCustomers()));

app.MapPost("api/customer", (ICustomerRepository repo, Customer customer) =>
{
    repo.CreateCustomer(customer);
    return Ok("Customer created successfully.");
});

app.MapDelete("api/customer/{id:int}", (ICustomerRepository repo, int id) =>
{
    repo.DeleteCustomer(id);
    return Ok("Customer deleted successfully.");
});

app.MapPut("api/customer", (ICustomerRepository repo, Customer customer) =>
{
    var res = repo.UpdateCustomer(customer);
    return Ok(res);
});

app.Run();