using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TaskFlow.Api.Data;
using TaskFlow.Api.Domain;
using TaskFlow.Api.Interfaces;

namespace TaskFlow.Api.Services;

public class TokenService : ITokenService
{
    private readonly AppDbContext _context;
    private readonly IConfiguration _configuration;

    public TokenService(AppDbContext context, IConfiguration configuration)
    {
        _context = context;
        _configuration = configuration;
    }

    public async Task<(string Token, DateTime ExpiresAt)> IssueTokenAsync(
        User user,
        IList<string> roles,
        IList<string> permissions,
        bool revokeExisting = true)
    {
        var jwtSection = _configuration.GetSection("Jwt");
        var key = jwtSection.GetValue<string>("Key")
            ?? throw new InvalidOperationException("Jwt:Key is not configured.");
        var issuer = jwtSection.GetValue<string>("Issuer")
            ?? throw new InvalidOperationException("Jwt:Issuer is not configured.");
        var audience = jwtSection.GetValue<string>("Audience")
            ?? throw new InvalidOperationException("Jwt:Audience is not configured.");
        var expiresMinutes = jwtSection.GetValue<int?>("ExpiresMinutes") ?? 60;

        var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
        var credentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new(JwtRegisteredClaimNames.Email, user.Email),
            new(JwtRegisteredClaimNames.Name, user.FullName)
        };

        foreach (var role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }

        foreach (var permission in permissions)
        {
            claims.Add(new Claim("permissions", permission));
        }

        var expiresAt = DateTime.UtcNow.AddMinutes(expiresMinutes);

        var token = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: claims,
            expires: expiresAt,
            signingCredentials: credentials);

        var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

        if (revokeExisting)
        {
            var existingTokens = await _context.UserTokens
                .Where(t => t.UserId == user.Id && t.RevokedAt == null && t.ExpiresAt > DateTime.UtcNow)
                .ToListAsync();

            foreach (var existing in existingTokens)
            {
                existing.RevokedAt = DateTime.UtcNow;
            }
        }

        var entry = new UserToken
        {
            UserId = user.Id,
            Token = tokenString,
            ExpiresAt = expiresAt,
            CreatedAt = DateTime.UtcNow
        };

        _context.UserTokens.Add(entry);
        await _context.SaveChangesAsync();

        return (tokenString, expiresAt);
    }

    public async Task<bool> IsTokenActiveAsync(string token, int userId, DateTime expiresAtUtc)
    {
        var record = await _context.UserTokens
            .FirstOrDefaultAsync(t => t.Token == token);

        if (record == null)
        {
            // Compatibility path: if a valid token was issued before we started
            // persisting tokens, register it now and enforce single-session by
            // revoking any other active entries for the same user.
            var existingTokens = await _context.UserTokens
                .Where(t => t.UserId == userId && t.RevokedAt == null && t.ExpiresAt > DateTime.UtcNow)
                .ToListAsync();

            foreach (var existing in existingTokens)
            {
                existing.RevokedAt = DateTime.UtcNow;
            }

            _context.UserTokens.Add(new UserToken
            {
                UserId = userId,
                Token = token,
                ExpiresAt = expiresAtUtc,
                CreatedAt = DateTime.UtcNow,
            });

            await _context.SaveChangesAsync();
            return true;
        }

        var isExpired = record.ExpiresAt <= DateTime.UtcNow;
        var isRevoked = record.RevokedAt.HasValue;
        return !isExpired && !isRevoked;
    }
}
