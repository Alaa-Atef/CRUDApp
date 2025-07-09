using DataAccessLayer.DBAccess;
using Domain;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Services.StudentServices;

namespace LuftbornTask.Tests
{
    public class StudentServiceTests
    {
        private AppDbContext GetDbContext()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) // Unique DB per test
                .Options;

            return new AppDbContext(options);
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnAllStudents()
        {
            var context = GetDbContext();
            context.Students.AddRange(
                new Student { Name = "Alice", Age = 20 },
                new Student { Name = "Bob", Age = 22 });
            await context.SaveChangesAsync();

            var service = new StudentService(context);
            var result = await service.GetAllAsync();

            result.Should().HaveCount(2);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnStudent_WhenExists()
        {
            var context = GetDbContext();
            var student = new Student { Id = 1, Name = "Alice", Age = 20 };
            context.Students.Add(student);
            await context.SaveChangesAsync();

            var service = new StudentService(context);
            var result = await service.GetByIdAsync(1);

            result.Should().NotBeNull();
            result!.Name.Should().Be("Alice");
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnNull_WhenNotFound()
        {
            var context = GetDbContext();
            var service = new StudentService(context);

            var result = await service.GetByIdAsync(99);

            result.Should().BeNull();
        }

        [Fact]
        public async Task CreateAsync_ShouldAddStudent()
        {
            var context = GetDbContext();
            var service = new StudentService(context);

            var student = new Student { Name = "Charlie", Age = 25 };

            var result = await service.CreateAsync(student);

            result.Id.Should().NotBe(0);
            (await context.Students.CountAsync()).Should().Be(1);
        }

        [Fact]
        public async Task UpdateAsync_ShouldModifyStudent_WhenIdMatches()
        {
            var context = GetDbContext();
            var student = new Student { Id = 1, Name = "David", Age = 18 };
            context.Students.Add(student);
            await context.SaveChangesAsync();

            // Detach original to prevent tracking conflict
            context.Entry(student).State = EntityState.Detached;

            var service = new StudentService(context);
            var updated = new Student { Id = 1, Name = "Daniel", Age = 20 };

            var result = await service.UpdateAsync(1, updated);

            result.Should().BeTrue();
            (await context.Students.FindAsync(1))!.Name.Should().Be("Daniel");
        }

        [Fact]
        public async Task UpdateAsync_ShouldReturnFalse_WhenIdMismatch()
        {
            var context = GetDbContext();
            var service = new StudentService(context);

            var updated = new Student { Id = 2, Name = "Mismatch", Age = 20 };

            var result = await service.UpdateAsync(1, updated);

            result.Should().BeFalse();
        }

        [Fact]
        public async Task DeleteAsync_ShouldRemoveStudent_WhenExists()
        {
            var context = GetDbContext();
            var student = new Student { Id = 1, Name = "Eva", Age = 22 };
            context.Students.Add(student);
            await context.SaveChangesAsync();

            var service = new StudentService(context);
            var result = await service.DeleteAsync(1);

            result.Should().BeTrue();
            (await context.Students.FindAsync(1)).Should().BeNull();
        }

        [Fact]
        public async Task DeleteAsync_ShouldReturnFalse_WhenNotFound()
        {
            var context = GetDbContext();
            var service = new StudentService(context);

            var result = await service.DeleteAsync(99);

            result.Should().BeFalse();
        }
    }
}
