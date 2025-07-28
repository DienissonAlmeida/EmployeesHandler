using EmployeeManagement.Application.Contracts;
using EmployeeManagement.Application.Security;
using EmployeeManagement.Application.Services;
using EmployeeManagement.Application.Validations;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace EmployeeManagement.Application.Extensions
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddScoped<IEmployeeService, EmployeeService>();
            services.AddScoped<ITokenService, TokenService>();
            services.AddValidatorsFromAssemblyContaining<CreateEmployeeValidator>();
            services.AddSingleton<IPasswordHasherService, PasswordHasherService>();
            return services;
        }
    }
}
