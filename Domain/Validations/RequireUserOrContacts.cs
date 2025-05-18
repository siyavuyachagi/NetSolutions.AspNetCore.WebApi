using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace Domain.Validations
{
    /// <summary>
    /// Validation attribute that ensures either a UserId or an Email is present on the model.
    /// </summary>
    /// <remarks>
    /// This attribute can be applied to a model class to enforce that either the "UserId"
    /// or the "Email" property (or both) must be provided. It uses reflection to check for
    /// those property names and their values.
    /// </remarks>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class RequireUserAccountAttribute : ValidationAttribute
    {
        /// <summary>
        /// Performs the validation logic to ensure the model contains either a non-empty UserId or Email.
        /// </summary>
        /// <param name="value">The value being validated (not used here as we're validating the object as a whole).</param>
        /// <param name="validationContext">Provides context about the object being validated.</param>
        /// <returns>
        /// A <see cref="ValidationResult"/> indicating success or an error message if neither property is set.
        /// </returns>
        protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
        {
            var instance = validationContext.ObjectInstance;
            var type = instance.GetType();

            // Get required properties using reflection
            var userIdProp = type.GetProperty("UserId", BindingFlags.Public | BindingFlags.Instance);
            var emailProp = type.GetProperty("Email", BindingFlags.Public | BindingFlags.Instance);

            // Ensure required properties exist on the model
            if (userIdProp == null || emailProp == null)
            {
                return new ValidationResult("Model must define UserId and Email properties.");
            }

            // Extract values from the properties
            var userId = userIdProp.GetValue(instance)?.ToString();
            var email = emailProp.GetValue(instance)?.ToString();

            // Validate presence of either UserId or Email
            bool hasUserId = !string.IsNullOrWhiteSpace(userId);
            bool hasEmail = !string.IsNullOrWhiteSpace(email);

            if (hasUserId || hasEmail)
            {
                return ValidationResult.Success!;
            }

            return new ValidationResult("Either UserId or Email must be provided.");
        }
    }
}

