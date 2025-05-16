
namespace Application.Interfaces
{
    public interface IRazorLightEmailRenderer
    {
        /// <summary>
        /// Renders a Razor template to an HTML string using the provided model.
        /// </summary>
        /// <typeparam name="T">The type of the model used in the Razor template.</typeparam>
        /// <param name="templateName">The name of the template file (without the .cshtml extension).</param>
        /// <param name="model">The data model passed to the template during rendering.</param>
        /// <returns>
        /// A task that resolves to the rendered HTML string.
        /// </returns>
        /// <example>
        /// var html = await renderer.RenderEmailTemplateAsync("AccountRegistrationConfirmation", model);
        /// </example>
        Task<string> RenderEmailTemplateAsync<T>(string templateName, T model);
    }
}

