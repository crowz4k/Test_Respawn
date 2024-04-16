using Test_Respawn.DB;
using static Test_Respawn.IntegrationTests.SharedCollectionsNames;

namespace Test_Respawn.IntegrationTests.CustomerEndpoint;

[Collection(SHARED_API_COLLECTION)]
public class CreateEndpoint(CustomerAPIFactory apiFactory) : IAsyncLifetime
{
    private readonly HttpClient _httpClient = apiFactory.HttpClient;

    private static readonly Faker<Address> _addressFaker = new Faker<Address>()
        .RuleFor(x => x.City, f => f.Address.City())
        .RuleFor(x => x.PostalCode, f => f.Address.ZipCode());

    private static readonly Faker<Customer> _customerFaker = new Faker<Customer>()
        .RuleFor(x => x.FirstName, f => f.Person.FirstName)
        .RuleFor(x => x.LastName, f => f.Person.LastName)
        .RuleFor(x => x.Email, f => f.Person.Email)
        .RuleFor(x => x.PhoneNumber, f => f.Person.Phone)
        .RuleFor(x => x.Address, _ => _addressFaker.Generate());

    [Fact]
    public async Task Create_WhenValid_ShouldReturnId()
    {
        // Arrange
        var customer = _customerFaker.Generate();
        // Act
        var response = await _httpClient.PostAsJsonAsync("api/customer", customer);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    public Task InitializeAsync() => Task.CompletedTask;
    public async Task DisposeAsync() => await apiFactory.ResetDatabaseAsync();
}