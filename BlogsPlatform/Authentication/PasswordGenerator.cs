using BlogsPlatform.Abstractions.Authentication;
using System.Security.Cryptography;

namespace BlogsPlatform.Authentication;

public class PasswordGenerator : IPasswordGenerator
{
    private const int PasswordLength = 12;

    private static readonly char[] Uppercase = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();
    private static readonly char[] Lowercase = "abcdefghijklmnopqrstuvwxyz".ToCharArray();
    private static readonly char[] Digits = "0123456789".ToCharArray();
    private static readonly char[] SpecialChars = "!@#$%^&*()-_=+[]{}|;:,.<>?".ToCharArray();

    private static readonly char[] AllChars =
        [.. Uppercase, .. Lowercase, .. Digits, .. SpecialChars];

    public string Generate()
    {
        var password = new char[PasswordLength];

        // Ensure at least one of each category
        password[0] = GetRandomChar(Uppercase);
        password[1] = GetRandomChar(Lowercase);
        password[2] = GetRandomChar(Digits);
        password[3] = GetRandomChar(SpecialChars);

        // Fill the rest randomly
        for (int i = 4; i < PasswordLength; i++)
        {
            password[i] = GetRandomChar(AllChars);
        }

        // Shuffle to avoid predictable character positions
        Shuffle(password);

        return new string(password);
    }

    private static char GetRandomChar(char[] charSet)
    {
        return charSet[RandomNumberGenerator.GetInt32(charSet.Length)];
    }

    private static void Shuffle(char[] array)
    {
        for (int i = array.Length - 1; i > 0; i--)
        {
            int j = RandomNumberGenerator.GetInt32(i + 1);
            (array[i], array[j]) = (array[j], array[i]);
        }
    }
}