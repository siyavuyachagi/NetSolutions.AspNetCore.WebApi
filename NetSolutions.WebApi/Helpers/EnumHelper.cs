


using System;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace NetSolutions.Helpers;
public static class EnumHelper
{
    /// <summary>
    /// Gets the display name of an enum value based on the <see cref="DisplayAttribute"/>.
    /// If no display name is found, it returns the enum's name as a string.
    /// </summary>
    /// <param name="value">The enum value.</param>
    /// <returns>The display name if found; otherwise, the enum name.</returns>
    public static string GetDisplayName(Enum value)
    {
        var field = value.GetType().GetField(value.ToString());
        var attribute = field?.GetCustomAttribute<DisplayAttribute>();
        return attribute?.Name ?? value.ToString();
    }

    /// <summary>
    /// Retrieves an enum value based on its display name defined in the <see cref="DisplayAttribute"/>.
    /// </summary>
    /// <typeparam name="T">The enum type.</typeparam>
    /// <param name="displayName">The display name of the enum value.</param>
    /// <returns>The corresponding enum value.</returns>
    /// <exception cref="ArgumentException">Thrown when no matching enum value is found.</exception>
    public static T GetEnumFromDisplayName<T>(string displayName) where T : Enum
    {
        foreach (var field in typeof(T).GetFields())
        {
            var attribute = field.GetCustomAttribute<DisplayAttribute>();
            if (attribute != null && attribute.Name == displayName)
            {
                return (T)field.GetValue(null);
            }
        }
        throw new ArgumentException($"No matching enum value found for display name: {displayName}", nameof(displayName));
    }
}

