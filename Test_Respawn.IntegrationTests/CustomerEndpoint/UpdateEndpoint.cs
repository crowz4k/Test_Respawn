using Test_Respawn.DB;
using static Test_Respawn.IntegrationTests.SharedCollectionsNames;

namespace Test_Respawn.IntegrationTests.CustomerEndpoint;

[Collection(SHARED_API_COLLECTION)]
public class UpdateEndpoint(CustomerAPIFactory apiFactory)
{
    private readonly HttpClient _httpClient = apiFactory.HttpClient;

    private Customer _customer => new()
    {
        FirstName = "John",
        LastName = "Doe",
        Email = "johndoe@johndoe.com",
        PhoneNumber = "123456789",
    };

    [Fact]
    public async Task Update_FirstName_Only_ShouldNotUpdateOtherProperties()
    {
        // Arrange
        var customer = _customer;

        // Act
        await _httpClient.PostAsJsonAsync("api/customer", customer);

        // Intentionally bad code to demonstrate the problem
        var updateResponse = await _httpClient.PutAsJsonAsync("api/customer", new Customer
        {
            Id = 1,
            FirstName = "Jane"
        });

        var updatedCustomer = await updateResponse.Content.ReadFromJsonAsync<Customer>();

        // Assert
        updateResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        updatedCustomer!.FirstName.Should().Be("Jane");
        updatedCustomer.Should().BeEquivalentTo(_customer, opt =>
        {
            opt.Excluding(e => e.Id);
            opt.Excluding(e => e.FirstName);
            return opt;
        });
    }
}