using DataAccessLayer.DBAccess;
using Domain;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Services.ProductServices;

namespace LuftbornTask.Tests
{
    public class ProductServiceTests
    {
        private AppDbContext GetDbContext()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) // unique DB per test
                .Options;

            var context = new AppDbContext(options);
            return context;
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnAllProducts()
        {
            // Arrange
            var context = GetDbContext();
            context.Products.AddRange(
                new Product { Id = 1, Name = "Product A", Price = 10 },
                new Product { Id = 2, Name = "Product B", Price = 20 }
            );
            await context.SaveChangesAsync();

            var service = new ProductService(context);

            // Act
            var result = await service.GetAllAsync();

            // Assert
            result.Should().HaveCount(2);
            result.Should().Contain(p => p.Name == "Product A");
            result.Should().Contain(p => p.Name == "Product B");
        }


        [Fact]
        public async Task GetByIdAsync_ShouldReturnProduct_WhenExists()
        {
            var context = GetDbContext();
            var product = new Product { Id = 1, Name = "Test", Price = 100 };
            context.Products.Add(product);
            await context.SaveChangesAsync();

            var service = new ProductService(context);
            var result = await service.GetByIdAsync(1);

            result.Should().NotBeNull();
            result!.Name.Should().Be("Test");
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnNull_WhenNotExists()
        {
            var service = new ProductService(GetDbContext());
            var result = await service.GetByIdAsync(999);
            result.Should().BeNull();
        }

        [Fact]
        public async Task CreateAsync_ShouldAddNewProduct()
        {
            var context = GetDbContext();
            var service = new ProductService(context);
            var product = new Product { Name = "New", Price = 50 };

            var created = await service.CreateAsync(product);

            var result = await context.Products.FindAsync(created.Id);
            result.Should().NotBeNull();
            result!.Name.Should().Be("New");
        }

        [Fact]
        public async Task UpdateAsync_ShouldModifyProduct_WhenIdMatches()
        {
            var context = GetDbContext();
            var product = new Product { Id = 1, Name = "Old", Price = 10 };
            context.Products.Add(product);
            await context.SaveChangesAsync();

            // Detach the entity to avoid tracking conflict
            context.Entry(product).State = EntityState.Detached;

            var service = new ProductService(context);
            var updated = new Product { Id = 1, Name = "Updated", Price = 99 };

            var result = await service.UpdateAsync(1, updated);

            result.Should().BeTrue();
            (await context.Products.FindAsync(1))!.Name.Should().Be("Updated");
        }

        [Fact]
        public async Task UpdateAsync_ShouldReturnFalse_WhenIdMismatch()
        {
            var service = new ProductService(GetDbContext());
            var updated = new Product { Id = 2, Name = "Wrong ID", Price = 99 };

            var result = await service.UpdateAsync(1, updated);

            result.Should().BeFalse();
        }

        [Fact]
        public async Task DeleteAsync_ShouldRemoveProduct_WhenExists()
        {
            var context = GetDbContext();
            context.Products.Add(new Product { Id = 1, Name = "ToDelete", Price = 15 });
            await context.SaveChangesAsync();

            var service = new ProductService(context);
            var result = await service.DeleteAsync(1);

            result.Should().BeTrue();
            (await context.Products.FindAsync(1)).Should().BeNull();
        }

        [Fact]
        public async Task DeleteAsync_ShouldReturnFalse_WhenNotExists()
        {
            var service = new ProductService(GetDbContext());
            var result = await service.DeleteAsync(999);

            result.Should().BeFalse();
        }

    }
}
