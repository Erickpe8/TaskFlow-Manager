using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using TaskFlow.Api.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskFlow.Api.Interfaces;

namespace TaskFlow.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly ITokenService _tokenService;

    public AuthController(IUserService userService, ITokenService tokenService)
    {
        _userService = userService;
        _tokenService = tokenService;
    }

    [HttpPost("register")]
    [AllowAnonymous]
    public async Task<IActionResult> Register(RegisterRequest request)
    {
        try
        {
            var user = await _userService.RegisterAsync(request);
            return CreatedAtAction(nameof(Register), new { user.Id }, new { user.Id, user.Email, user.FullName });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login(LoginRequest request)
    {
        var validation = await _userService.ValidateUserAsync(request.Email, request.Password);
        if (validation == null)
        {
            return Unauthorized();
        }

        var (user, roles, permissions) = validation.Value;
        var (token, expiresAt) = await _tokenService.IssueTokenAsync(user, roles, permissions);

        var response = new AuthResponse(token, expiresAt, user.Email, user.FullName, roles.ToArray(), permissions.ToArray());
        return Ok(response);
    }

    [HttpPost("refresh")]
    [Authorize]
    public async Task<IActionResult> Refresh()
    {
        var subjectId = User.FindFirstValue(JwtRegisteredClaimNames.Sub) ?? User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrWhiteSpace(subjectId) || !int.TryParse(subjectId, out var userId))
        {
            return Unauthorized();
        }

        var userWithClaims = await _userService.GetUserWithClaimsAsync(userId);
        if (userWithClaims == null)
        {
            return Unauthorized();
        }

        var (user, roles, permissions) = userWithClaims.Value;
        var (token, expiresAt) = await _tokenService.IssueTokenAsync(user, roles, permissions);

        var response = new AuthResponse(token, expiresAt, user.Email, user.FullName, roles.ToArray(), permissions.ToArray());
        return Ok(response);
    }

    [HttpGet("me")]
    [Authorize]
    public IActionResult Me()
    {
        var email = User.FindFirstValue(ClaimTypes.Email) ?? string.Empty;
        var fullName = User.FindFirstValue("name") ?? string.Empty;
        var roles = User.FindAll(ClaimTypes.Role).Select(c => c.Value).ToArray();
        var permissions = User.FindAll("permissions").Select(c => c.Value).ToArray();

        var response = new MeResponse(email, fullName, roles, permissions);
        return Ok(response);
    }
}
