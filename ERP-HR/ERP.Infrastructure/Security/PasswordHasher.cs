using System.Security.Cryptography;
using ERP.Application.Interfaces;

namespace ERP.Infrastructure.Security;

/// <summary>
/// implement IPasswordHasher ด้วย PBKDF2 (HMAC-SHA256) จาก BCL — ไม่ต้องพึ่ง package ภายนอก
/// รูปแบบที่เก็บ: "{iterations}.{saltBase64}.{hashBase64}" → ตรวจย้อนได้โดยไม่ต้องเก็บ salt แยก
/// </summary>
public class PasswordHasher : IPasswordHasher
{
    private const int SaltSize = 16;        // 128-bit salt
    private const int KeySize = 32;         // 256-bit hash
    private const int Iterations = 100_000;
    private const char Delimiter = '.';
    private static readonly HashAlgorithmName Algorithm = HashAlgorithmName.SHA256;

    public string Hash(string password)
    {
        var salt = RandomNumberGenerator.GetBytes(SaltSize);
        var hash = Rfc2898DeriveBytes.Pbkdf2(password, salt, Iterations, Algorithm, KeySize);

        return string.Join(Delimiter,
            Iterations,
            Convert.ToBase64String(salt),
            Convert.ToBase64String(hash));
    }

    public bool Verify(string password, string hash)
    {
        var parts = hash.Split(Delimiter);
        if (parts.Length != 3) return false;

        var iterations = int.Parse(parts[0]);
        var salt = Convert.FromBase64String(parts[1]);
        var storedHash = Convert.FromBase64String(parts[2]);

        var inputHash = Rfc2898DeriveBytes.Pbkdf2(password, salt, iterations, Algorithm, storedHash.Length);
        return CryptographicOperations.FixedTimeEquals(inputHash, storedHash);
    }
}
