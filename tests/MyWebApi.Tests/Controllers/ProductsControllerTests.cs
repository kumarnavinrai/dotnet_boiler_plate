using Xunit;
using Moq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using MyWebAPI.Controllers;
using MyWebAPI.Data;
using MyWebAPI.Models;
using MyWebAPI.DTOs;
using System.Linq;

namespace MyWebApi.Tests.Controllers
{
    public class ProductsControllerTests
    {
        private readonly Mock<AppDbContext> _mockContext;
        private readonly ProductsController _controller;

        public ProductsControllerTests()
        {
            // Mock DbContext with In-Memory Database
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDb")
                .Options;

            var dbContext = new AppDbContext(options);
            _mockContext = new Mock<AppDbContext>(options);
            _controller = new ProductsController(dbContext);

            // Seed test data
            dbContext.Products.AddRange(new List<Product>
            {
                new Product { Id = 1, Name = "Product A", Price = 10.5m, Description = "Test Product A", Stock = 100 },
                new Product { Id = 2, Name = "Product B", Price = 15.0m, Description = "Test Product B", Stock = 50 }
            });
            dbContext.SaveChanges();
        }

        [Fact]
        public async Task GetProducts_ReturnsAllProducts()
        {
            // Act
            var result = await _controller.GetProducts();
            var okResult = Assert.IsType<ActionResult<IEnumerable<Product>>>(result);
            var products = Assert.IsType<List<Product>>(okResult.Value);

            // Assert
            Assert.Equal(2, products.Count);
        }

        [Fact]
        public async Task GetProduct_ReturnsProduct_WhenProductExists()
        {
            // Act
            var result = await _controller.GetProduct(1);
            var okResult = Assert.IsType<ActionResult<Product>>(result);
            var product = Assert.IsType<Product>(okResult.Value);

            // Assert
            Assert.Equal("Product A", product.Name);
        }

        [Fact]
        public async Task GetProduct_ReturnsNotFound_WhenProductDoesNotExist()
        {
            // Act
            var result = await _controller.GetProduct(99);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result.Result);
        }

        [Fact]
        public async Task PostProduct_AddsProductSuccessfully()
        {
            // Arrange
            var productDto = new ProductDto
            {
                Name = "Product C",
                Price = 20.0m,
                Description = "New Product",
                Stock = 25
            };

            // Act
            var result = await _controller.PostProduct(productDto);
            var createdResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            var product = Assert.IsType<Product>(createdResult.Value);

            // Assert
            Assert.Equal("Product C", product.Name);
        }

        [Fact]
        public async Task PutProduct_UpdatesProductSuccessfully()
        {
            // Arrange
            var productDto = new ProductDto
            {
                Name = "Updated Product A",
                Price = 12.0m,
                Description = "Updated Description",
                Stock = 90
            };

            // Act
            var result = await _controller.PutProduct(1, productDto);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task DeleteProduct_RemovesProductSuccessfully()
        {
            // Act
            var result = await _controller.DeleteProduct(1);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task DeleteProduct_ReturnsNotFound_WhenProductDoesNotExist()
        {
            // Act
            var result = await _controller.DeleteProduct(99);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
        }
    }
}