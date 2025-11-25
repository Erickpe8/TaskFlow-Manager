namespace TaskFlow.Api.DTOs;

public record AuthResponse(string Token, DateTime ExpiresAt, string Email, string FullName, string[] Roles, string[] Permissions);
