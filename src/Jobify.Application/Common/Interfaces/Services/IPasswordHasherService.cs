namespace Jobify.Application.Common.Interfaces.Services;

public interface IPasswordHasherService
{
    Task<string> HashPasswordAsync(string password);
    Task<bool> VerifyHashedPasswordAsync(string password, string hashedPassword);
}
