using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace BlogsPlatform.Abstractions.Helpers;

public static class SlugGenerator
{
    public static string Create(string input)
    {
        if (string.IsNullOrWhiteSpace(input))
        {
            return string.Empty;
        }

        // Normalize to remove accents
        string normalized = input.Normalize(NormalizationForm.FormD);
        var builder = new StringBuilder();

        foreach (char c in normalized)
        {
            var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
            if (unicodeCategory != UnicodeCategory.NonSpacingMark)
            {
                builder.Append(c);
            }
        }

        string cleaned = builder
            .ToString()
            .Normalize(NormalizationForm.FormC)
            .ToLowerInvariant();

        // Replace & with "and"
        cleaned = cleaned.Replace("&", "and");

        // Remove invalid characters
        cleaned = Regex.Replace(cleaned, @"[^a-z0-9\s-]", string.Empty);

        // Replace spaces and underscores with hyphens
        cleaned = Regex.Replace(cleaned, @"[\s_]+", "-");

        // Remove multiple hyphens
        cleaned = Regex.Replace(cleaned, @"-+", "-");

        // Trim hyphens
        return cleaned.Trim('-');
    }
}