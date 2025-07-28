using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EmployeeManagement.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "Id", "BirthDate", "DocumentNumber", "Email", "FirstName", "LastName", "ManagerId", "PasswordHash", "PhoneNumbers", "Role" },
                values: new object[] { new Guid("3f5a1b2c-7d6e-4a9f-b2c1-8e4f9d123456"), new DateTime(1990, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "12345678901", "admin@example.com", "Admin", "User", null, "Admin", "", 3 });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: new Guid("3f5a1b2c-7d6e-4a9f-b2c1-8e4f9d123456"));
        }
    }
}
