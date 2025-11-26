namespace TaskFlow.Api.DTOs;

public record MeResponse(string Email, string FullName, string[] Roles, string[] Permissions);
