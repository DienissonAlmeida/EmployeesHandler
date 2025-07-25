//using EmployeeManagement.Domain.Interfaces;
//using Microsoft.AspNetCore.Identity;

//namespace EmployeeManagement.Infrastructure.Security;

//public class PasswordHasher : IPasswordHasher
//{
//    private readonly IPasswordHasher<object> _hasher = new PasswordHasher<object>();

//    public string Hash(string password) =>
//        _hasher.HashPassword(null!, password);

//    public bool Verify(string hash, string plainPassword) =>
//        _hasher.VerifyHashedPassword(null!, hash, plainPassword) == PasswordVerificationResult.Success;
//}