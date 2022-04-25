using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Security.Cryptography;

namespace LoginService.Crypto;

public class Hasher
{
    public static byte[] GenerateSalt()
    {
        var salt = new byte[128 / 8];
        using (var rng = RandomNumberGenerator.Create())
            rng.GetNonZeroBytes(salt);
        return salt;
    }

    public static bool ValidateHash(string hash, string test)
    {
        var salt = Convert.FromBase64String(hash)[0..16];
        var hashedPassword = HMACSHA256Salted(test, salt);
        return hashedPassword == hash;
    }

    public static string HMACSHA256Salted(string input, byte[] salt)
    {
        const int hashLength = 256 / 8;

        // Calculate the hash.
        byte[] inputHash = KeyDerivation.Pbkdf2(
            password: input,
            salt: salt,
            prf: KeyDerivationPrf.HMACSHA256,
            iterationCount: 100000,
            numBytesRequested: hashLength);

        byte[] hash = new byte[salt.Length + hashLength];
        Array.Copy(salt, 0, hash, 0, salt.Length);
        Array.Copy(inputHash, 0, hash, salt.Length, hashLength);
        return Convert.ToBase64String(hash);
    }
}