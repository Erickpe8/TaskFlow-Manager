using TaskFlow.Api.Data;
using TaskFlow.Api.Domain;
using TaskFlow.Api.DTOs;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TaskFlow.Api.Interfaces;

namespace TaskFlow.Api.Services;

public class UserService : IUserService
{
    private readonly AppDbContext _context;
    private readonly PasswordHasher<User> _passwordHasher;

    public UserService(AppDbContext context)
    {
        _context = context;
        _passwordHasher = new PasswordHasher<User>();
    }

    public async Task<User> RegisterAsync(RegisterRequest request)
    {
        var existing = await _context.Users.FirstOrDefaultAsync(u => u.Email == request.Email);
        if (existing != null)
        {
            throw new InvalidOperationException("El email ya está registrado.");
        }

        var user = new User
        {
            Email = request.Email,
            FullName = request.FullName,
            IsActive = true
        };
        user.PasswordHash = _passwordHasher.HashPassword(user, request.Password);

        var studentRole = await _context.Roles.FirstOrDefaultAsync(r => r.Name == "Student");
        if (studentRole == null)
        {
            throw new InvalidOperationException("El rol Student no está configurado.");
        }

        user.UserRoles.Add(new UserRole { RoleId = studentRole.Id, User = user });

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        return user;
    }

    public async Task<(User user, IList<string> roles, IList<string> permissions)?> ValidateUserAsync(string email, string password)
    {
        var user = await _context.Users
            .Include(u => u.UserRoles)
            .ThenInclude(ur => ur.Role)
            .ThenInclude(r => r.RolePermissions)
            .ThenInclude(rp => rp.Permission)
            .FirstOrDefaultAsync(u => u.Email == email);

        if (user == null || !user.IsActive)
        {
            return null;
        }

        var result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, password);
        if (result == PasswordVerificationResult.Failed)
        {
            return null;
        }

        var roles = user.UserRoles.Select(ur => ur.Role!.Name).ToList();
        var permissions = user.UserRoles
            .SelectMany(ur => ur.Role!.RolePermissions)
            .Select(rp => rp.Permission!.Name)
            .Distinct()
            .ToList();

        return (user, roles, permissions);
    }

    public async Task<(User user, IList<string> roles, IList<string> permissions)?> GetUserWithClaimsAsync(int userId)
    {
        var user = await _context.Users
            .Include(u => u.UserRoles)
            .ThenInclude(ur => ur.Role)
            .ThenInclude(r => r.RolePermissions)
            .ThenInclude(rp => rp.Permission)
            .FirstOrDefaultAsync(u => u.Id == userId);

        if (user == null || !user.IsActive)
        {
            return null;
        }

        var roles = user.UserRoles.Select(ur => ur.Role!.Name).ToList();
        var permissions = user.UserRoles
            .SelectMany(ur => ur.Role!.RolePermissions)
            .Select(rp => rp.Permission!.Name)
            .Distinct()
            .ToList();

        return (user, roles, permissions);
    }
}
