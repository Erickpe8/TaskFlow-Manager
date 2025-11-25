using TaskFlow.Api.Domain;

namespace TaskFlow.Api.Interfaces;

public interface ITokenService
{
    Task<(string Token, DateTime ExpiresAt)> IssueTokenAsync(
        User user,
        IList<string> roles,
        IList<string> permissions,
        bool revokeExisting = true);

    Task<bool> IsTokenActiveAsync(string token, int userId, DateTime expiresAtUtc);
}
