using TaskFlow.Api.DTOs;

namespace TaskFlow.Api.Interfaces
{
    public interface IAuthService
    {
        Task<AuthResponseDto?> LoginAsync(string email, string password);
    }
}
