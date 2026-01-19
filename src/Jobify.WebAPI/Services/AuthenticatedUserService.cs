namespace Jobify.WebAPI.Services;

public class AuthenticatedUserService : IAuthenticatedUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public AuthenticatedUserService(IHttpContextAccessor httpContextAccessor) => _httpContextAccessor = httpContextAccessor;

    public Guid? Id
    {
        get
        {
            string? userIdString =
                _httpContextAccessor.HttpContext?.User
                    .FindFirstValue(ClaimTypes.NameIdentifier);

            return Guid.TryParse(userIdString, out Guid userId)
                ? userId
                : null;
        }
    }

    public List<string>? Roles =>
        _httpContextAccessor.HttpContext?.User.FindAll(ClaimTypes.Role).Select(x => x.Value).ToList();
}
