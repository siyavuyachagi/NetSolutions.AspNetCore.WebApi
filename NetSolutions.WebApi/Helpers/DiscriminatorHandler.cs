using System;
using System.Text.RegularExpressions;
using System.Globalization;

/// <summary>
/// Defines different casing styles for string formatting
/// </summary>
public enum Casing
{
    /// <summary>
    /// PascalCase (default): Keeps the original formatting with spaces inserted
    /// Example: "HTMLParser" -> "HTML Parser"
    /// </summary>
    Pascal,

    /// <summary>
    /// camelCase: Converts to lowercase with spaces removed
    /// Example: "HTMLParser" -> "html parser"
    /// </summary>
    Camel,

    /// <summary>
    /// snake_case: Converts to lowercase with underscores
    /// Example: "HTMLParser" -> "html_parser"
    /// </summary>
    Snake,

    /// <summary>
    /// kebab-case: Converts to lowercase with hyphens
    /// Example: "HTMLParser" -> "html-parser"
    /// </summary>
    Kebab,

    /// <summary>
    /// Title Case: Converts to title case with spaces
    /// Example: "HTMLParser" -> "Html Parser"
    /// </summary>
    Title
}

/// <summary>
/// Extension methods for string formatting
/// </summary>
public static class StringExtensions
{
    /// <summary>
    /// Formats a string by inserting spaces before capital letters and applying the specified casing
    /// </summary>
    /// <param name="input">The input string to format</param>
    /// <param name="casing">The desired casing style (defaults to PascalCase)</param>
    /// <returns>Formatted string</returns>
    public static string ToFormattedString(this string input, Casing casing = Casing.Pascal)
    {
        // Handle null or whitespace input
        if (string.IsNullOrWhiteSpace(input)) return string.Empty;

        // Special handling for consecutive uppercase sequences and camel case
        // This regex inserts a space before a capital letter only if it's preceded by a lowercase letter
        // and the next characters are not all uppercase
        string formatted = Regex.Replace(input, @"(?<=[a-z])(?=[A-Z])(?![A-Z]+\b)", " ");

        // Apply the specified casing style
        return casing switch
        {
            Casing.Camel => formatted.ToLower(),
            Casing.Snake => formatted.ToLower().Replace(" ", "_"),
            Casing.Kebab => formatted.ToLower().Replace(" ", "-"),
            Casing.Title => CultureInfo.CurrentCulture.TextInfo.ToTitleCase(formatted),
            _ => formatted // PascalCase (default)
        };
    }
}