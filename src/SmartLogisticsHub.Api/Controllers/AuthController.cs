using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SmartLogisticsHub.Infrastructure.Auth;

namespace SmartLogisticsHub.Api.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly UserManager<AppUser> _users;
    private readonly SignInManager<AppUser> _signIn;
    private readonly IJwtTokenService _jwt;

    public AuthController(
        UserManager<AppUser> users,
        SignInManager<AppUser> signIn,
        IJwtTokenService jwt)
    {
        _users = users;
        _signIn = signIn;
        _jwt = jwt;
    }

    [HttpPost("register")]
    public async Task<ActionResult<AuthResponse>> Register(RegisterRequest req)
    {
        var user = new AppUser { UserName = req.Email, Email = req.Email };
        var result = await _users.CreateAsync(user, req.Password);

        if (!result.Succeeded)
            return BadRequest(result.Errors.Select(e => e.Description));

        var roles = await _users.GetRolesAsync(user);
        var token = _jwt.CreateToken(user, roles);

        return Ok(new AuthResponse(token, user.Email!));
    }

    [HttpPost("login")]
    public async Task<ActionResult<AuthResponse>> Login(LoginRequest req)
    {
        var user = await _users.FindByEmailAsync(req.Email);
        if (user is null) return Unauthorized(new { error = "Invalid credentials" });

        var ok = await _signIn.CheckPasswordSignInAsync(user, req.Password, lockoutOnFailure: false);
        if (!ok.Succeeded) return Unauthorized(new { error = "Invalid credentials" });

        var roles = await _users.GetRolesAsync(user);
        var token = _jwt.CreateToken(user, roles);

        return Ok(new AuthResponse(token, user.Email!));
    }

    [HttpGet("/test")]
    [Authorize]
    public Task<IActionResult> Test()
    {
        return Task.FromResult<IActionResult>(Ok("Success"));
    }
}