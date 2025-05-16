using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Domain.Validations
{
    /// <summary>
    /// Validates that an uploaded file matches the allowed file extensions
    /// and their corresponding MIME types.
    /// </summary>
    public class FileExtensionsAttribute : ValidationAttribute
    {
        // Store the allowed extensions (e.g., "jpg", "png")
        private readonly HashSet<string> _allowedExtensions;

        // Internal mapping of file extensions to their expected MIME types
        private readonly Dictionary<string, string[]> _mimeTypes = new(StringComparer.OrdinalIgnoreCase)
        {
            { "jpg",  ["image/jpeg"] },
            { "jpeg", ["image/jpeg"] },
            { "png",  ["image/png"] },
            { "gif",  ["image/gif"] },
            { "webp", ["image/webp"] },
            { "bmp",  ["image/bmp"] },
            { "svg",  ["image/svg+xml"] },
            { "pdf",  ["application/pdf"] },
            { "docx", ["application/vnd.openxmlformats-officedocument.wordprocessingml.document"] },
            { "doc",  ["application/msword"] },
            { "xlsx", ["application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"] },
            { "txt",  ["text/plain"] },
            { "zip",  ["application/zip"] },
        };

        /// <summary>
        /// Constructor accepts a comma-separated list of allowed extensions (e.g., "jpg,png").
        /// </summary>
        public FileExtensionsAttribute(string extensions)
        {
            // Normalize and store allowed extensions in a HashSet for fast lookup
            _allowedExtensions = extensions.Split(',')
                                           .Select(e => e.Trim().ToLowerInvariant())
                                           .ToHashSet();
        }

        /// <summary>
        /// Performs validation on the file.
        /// </summary>
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            // If no file is uploaded, there's nothing to validate
            if (value is not IFormFile file)
                return ValidationResult.Success;

            // Get the extension from the file name (e.g., "jpg" from "photo.jpg")
            var extension = Path.GetExtension(file.FileName)?
                                .TrimStart('.')       // Remove the leading dot
                                .ToLowerInvariant();  // Normalize casing

            // Check if extension is missing or not in the allowed list
            if (string.IsNullOrEmpty(extension) || !_allowedExtensions.Contains(extension))
            {
                var formatted = string.Join(", ", _allowedExtensions.Select(e => $".{e}"));
                return new ValidationResult($"Invalid file extension. Allowed: {formatted}.");
            }

            // If we know the MIME types for this extension, validate them too
            if (_mimeTypes.TryGetValue(extension, out var expectedContentTypes))
            {
                // If file's ContentType is not among expected MIME types, return error
                if (!expectedContentTypes.Contains(file.ContentType))
                {
                    var expected = string.Join(", ", expectedContentTypes);
                    return new ValidationResult($"Invalid content type '{file.ContentType}' for '.{extension}' file. Expected: {expected}.");
                }
            }

            // If everything checks out, the file is valid
            return ValidationResult.Success;
        }
    }
}


