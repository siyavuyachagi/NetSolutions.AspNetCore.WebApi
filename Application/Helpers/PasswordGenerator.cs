
using System;
using System.Security.Cryptography;
using System.Linq;


namespace NetSolutions.Helpers;

public class PasswordGenerator
{
    private static readonly string ValidChars =
        "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890!@#$%^&*()-_=+";

    public static string Generate(int length = 8)
    {
        return new string(Enumerable.Range(0, length)
            .Select(_ => ValidChars[GetRandomNumber(0, ValidChars.Length)])
            .ToArray());
    }

    private static int GetRandomNumber(int min, int max)
    {
        using (var rng = RandomNumberGenerator.Create())
        {
            byte[] data = new byte[4];
            rng.GetBytes(data);
            int generatedValue = BitConverter.ToInt32(data, 0) & int.MaxValue; // Ensures positive number
            return min + (generatedValue % (max - min));
        }
    }
}
