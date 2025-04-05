using Moq;
using Xunit;
using ems.Models;
using ems.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

public class ProductServiceTests
{
    [Fact]
    public async Task GetAllProducts_ReturnsList()
    {
        // Arrange
        var mockService = new Mock<IProductService>();
        var products = new List<Product>
        {
            new Product { Id = 1, Name = "Product 1", Price = 10 },
            new Product { Id = 2, Name = "Product 2", Price = 20 }
        };

        mockService.Setup(service => service.GetAllProductsAsync()).ReturnsAsync(products);
        var service = mockService.Object;

        // Act
        var result = await service.GetAllProductsAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count);
    }
}
