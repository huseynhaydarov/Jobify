using Jobify.Application.UseCases.Auths.ChangePassword.Commands;

namespace Jobify.API.Controllers.V1;

[Route("api/[controller]")]
[ApiController]
public class AuthsController : ControllerBase
{
    private readonly IMediator _mediator;

    public AuthsController(IMediator mediator, ITokenService tokenService)
    {
        _mediator = mediator;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginCommand command)
    {
        var data = await _mediator.Send(command);

        if (!data.Success)
        {
            return Unauthorized(new { message = "Invalid credentials" });
        }

        Response.Cookies.Append("RefreshToken", data.RefreshToken, new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Strict,
            Expires = DateTime.UtcNow.AddDays(14)
        });

        return Ok(new
        {
            AccessToken = data.Token,
            data.Email,
            data.Role
        });
    }

    [HttpPost("refresh-token")]
    [AllowAnonymous]
    public async Task<IActionResult> RefreshToken(CancellationToken cancellationToken)
    {
        if (!Request.Cookies.TryGetValue("RefreshToken", out var refreshToken))
        {
            return BadRequest("Missing RefreshToken cookie.");
        }

        var rawHeader = Request.Headers["Authorization"].FirstOrDefault();

        if (string.IsNullOrEmpty(rawHeader) || !rawHeader.StartsWith("Bearer "))
        {
            return BadRequest("Missing Authorization header.");
        }

        var accessToken = rawHeader["Bearer ".Length..].Trim();

        var command = new RefreshTokenCommand(accessToken, refreshToken);

        var result = await _mediator.Send(command, cancellationToken);

        Response.Cookies.Append("RefreshToken", result.RefreshToken, new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Strict,
            Expires = DateTime.UtcNow.AddDays(14)
        });

        return Ok(result);
    }

    [HttpPost("change_password")]
    [Authorize(Roles = UserRoles.EmployerOrJobSeeker)]
    public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordCommand command)
    {
        var data = await _mediator.Send(command);

        return Ok(data);
    }
}

