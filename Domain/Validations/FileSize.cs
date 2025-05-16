using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Domain.Validations
{
    /// <summary>
    /// Validates the allowed file size in megabytes (MB) for an uploaded file.
    /// </summary>
    public class FileSizeAttribute : ValidationAttribute
    {
        /// <summary>
        /// Allowed file size in bytes.
        /// </summary>
        private readonly long _maxBytes;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileSizeAttribute"/> class.
        /// </summary>
        /// <param name="maxMegabytes">Allowed file size in megabytes.</param>
        public FileSizeAttribute(int maxMegabytes)
        {
            // Convert MB to bytes
            _maxBytes = maxMegabytes * 1024 * 1024;
        }

        /// <summary>
        /// Performs validation to check whether the file size is within the allowed limit.
        /// </summary>
        /// <param name="value">The object to validate, expected to be an <see cref="IFormFile"/>.</param>
        /// <param name="validationContext">The context in which the validation is performed.</param>
        /// <returns>A <see cref="ValidationResult"/> indicating success or failure.</returns>
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is IFormFile file)
            {
                if (file.Length != _maxBytes)
                {
                    // Return validation error if the file exceeds the limit
                    return new ValidationResult($"Allowed file size is {_maxBytes / (1024 * 1024)} MB.");
                }
            }

            // If file is valid or null, return success
            return ValidationResult.Success;
        }
    }
}
