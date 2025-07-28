using System.Collections.Generic;
using EmployeeManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagement.Infrastructure.Data
{
    public class EmployeeDbContext : DbContext
    {
        public EmployeeDbContext(DbContextOptions<EmployeeDbContext> options) : base(options) { }

        public DbSet<Employee> Employees => Set<Employee>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var admin = new Employee
            {
                Id = SeedData.AdminId,
                FirstName = "Admin",
                LastName = "User",
                Email = "admin@example.com",
                Role = Role.Director, // or Enum if you have one
                PasswordHash = "Admin",
                BirthDate = SeedData.AdminBirthDate,
                DocumentNumber = "12345678901",
            };

            modelBuilder.Entity<Employee>().HasData(admin);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(EmployeeDbContext).Assembly);
        }
    }
    public static class SeedData
    {
        public static readonly Guid AdminId = Guid.Parse("3f5a1b2c-7d6e-4a9f-b2c1-8e4f9d123456");
        public static readonly DateTime AdminBirthDate = DateTime.SpecifyKind(new DateTime(1990, 01, 01), DateTimeKind.Utc);
    }
}
