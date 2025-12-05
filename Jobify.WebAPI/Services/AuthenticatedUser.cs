namespace Jobify.API.Services;

public class AuthenticatedUser : IAuthenticatedUser
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public AuthenticatedUser(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public Guid  Id
    {
        get
        {
            string? userIdString = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
            return Guid.TryParse(userIdString, out var userId) ? userId : Guid.Empty;
        }
    }
    public List<string>? Roles => _httpContextAccessor.HttpContext?.User.FindAll(ClaimTypes.Role).Select(x => x.Value).ToList();
}
