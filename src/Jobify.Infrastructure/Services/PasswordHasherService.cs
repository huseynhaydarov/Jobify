namespace Jobify.Infrastructure.Services;

public sealed class PasswordHasherService : IPasswordHasherService
{
    private const int SaltSize = 16;
    private const int HashSize = 32;
    private const int Iterations = 10000;

    private static readonly HashAlgorithmName Algorithm = HashAlgorithmName.SHA256;

    public async Task<string> HashPasswordAsync(string password)
    {
        byte[] salt = RandomNumberGenerator.GetBytes(SaltSize);

        byte[] hash = Rfc2898DeriveBytes.Pbkdf2(password, salt, Iterations, Algorithm, HashSize);

        return await Task.FromResult($"{Convert.ToHexString(hash)}-{Convert.ToHexString(salt)}");
    }

    public Task<bool> VerifyHashedPasswordAsync(string password, string hashedPassword)
    {
        if (string.IsNullOrEmpty(hashedPassword) || !hashedPassword.Contains('-'))
        {
            throw new ArgumentException("Invalid hashed password format.", nameof(hashedPassword));
        }

        string[] parts = hashedPassword.Split('-');

        if (parts.Length != 2)
        {
            throw new ArgumentException("Invalid hashed password format.", nameof(hashedPassword));
        }

        try
        {
            byte[] hash = Convert.FromHexString(parts[0]);
            byte[] salt = Convert.FromHexString(parts[1]);

            byte[] inputHash = Rfc2898DeriveBytes.Pbkdf2(password, salt, Iterations, Algorithm, HashSize);

            return Task.FromResult(CryptographicOperations.FixedTimeEquals(inputHash, hash));
        }
        catch (FormatException)
        {
            throw new ArgumentException("Invalid hashed password format.", nameof(hashedPassword));
        }
    }
}
