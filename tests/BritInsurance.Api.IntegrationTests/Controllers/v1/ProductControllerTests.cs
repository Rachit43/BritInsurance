using BritInsurance.Application.Dto;
using System.Net.Http.Json;

namespace BritInsurance.Api.IntegrationTests.Controllers.v1
{
    public class ProductControllerTests
    {
        [Fact]
        public async Task ValidateProductApiApiRequests()
        {
            var client = new BritInsuranceWebFactory().CreateClient();
            await AddProducts(client);
            await GetProducts(client);
        }

        private static async Task GetProducts(HttpClient client)
        {
            var httpResponse = await client.GetAsync("/api/v1/product/1");

            Assert.Equal(System.Net.HttpStatusCode.OK, httpResponse.StatusCode);
        }

        private static async Task AddProducts(HttpClient client)
        {
            var response = await client.PostAsJsonAsync("/api/v1/product", new CreateProductDto
            {
                ProductName = "product1",
                Items = new CreateItemFromProductDto[] { new CreateItemFromProductDto { Quantity = 2 } }
            });

            Assert.Equal(System.Net.HttpStatusCode.Created, response.StatusCode);
        }
    }
}
