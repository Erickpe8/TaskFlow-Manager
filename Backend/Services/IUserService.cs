using TaskFlow.Api.Domain;
using TaskFlow.Api.DTOs;

namespace TaskFlow.Api.Services;

public interface IUserService
{
    Task<User> RegisterAsync(RegisterRequest request);
    Task<(User user, IList<string> roles, IList<string> permissions)?> ValidateUserAsync(string email, string password);
    Task<(User user, IList<string> roles, IList<string> permissions)?> GetUserWithClaimsAsync(int userId);
}
