using System;
using System.Security.Cryptography;
using menu_users.Domain.Interfaces.Users;
using Microsoft.AspNetCore.Identity;


namespace menu_users.Application.Services;

public class PasswordHasherService : IPasswordHasher
{
    private const int SaltSize = 16; // 128 bits
    private const int HashSize = 32; // 256 bits

    private const int Iterations = 10000;

    private readonly HashAlgorithmName _algorithm = HashAlgorithmName.SHA512;

    public string Hash(string password)
    {
        byte[] salt = RandomNumberGenerator.GetBytes(SaltSize);
        byte[] hash = Rfc2898DeriveBytes.Pbkdf2(
            password,
            salt,
            Iterations,
            _algorithm,
            HashSize);
        return $"{Convert.ToHexString(salt)}-{Convert.ToHexString(hash)}";
    }

    public bool Verify(string hashedPassword, string providedPassword)
    {
        string[] parts = hashedPassword.Split('-');
        if (parts.Length != 2)
        {
            return false;
        }

        byte[] salt = Convert.FromHexString(parts[0]);
        byte[] hash = Convert.FromHexString(parts[1]);

        byte[] inputHash = Rfc2898DeriveBytes.Pbkdf2(
            providedPassword,
            salt,
            Iterations,
            _algorithm,
            HashSize);

        return CryptographicOperations.FixedTimeEquals(hash, inputHash);
    }

    public string GetRandomGeneratedPassword()
    {
        // Genera una string aleatoria de 12 caracteres que incluya letras mayúsculas, minúsculas, números y símbolos
        const string upper = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        const string lower = "abcdefghijklmnopqrstuvwxyz";
        const string digits = "0123456789";
        const string symbols = "!@#$%^&*()-_=+[]{}|;:,.<>?";
        const string allChars = upper + lower + digits + symbols;

        var random = new Random();
        char[] password = new char[12];

        for (int i = 0; i < password.Length; i++)
        {
            password[i] = allChars[random.Next(allChars.Length)];
        }

        return new string(password);
    }
}
