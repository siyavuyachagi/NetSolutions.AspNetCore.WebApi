using RazorLight;


namespace NetSolutions.Templates.Emails;

public class ContactUsViewModel(string Email, string Subject, string Message)
{
    public string Email { get; set; } = Email;
    public string Subject { get; set; } = Subject;
    public string Message { get; set; } = Message;


    // Method to fetch the Razor view and return the rendered HTML body
    public async Task<string> HtmlBodyAsync(IWebHostEnvironment webHost)
    {
        // Get the absolute path using IWebHostEnvironment
        string templatePath = Path.Combine(webHost.ContentRootPath, "Templates", "Emails", "ContactUs.cshtml");

        var engine = new RazorLightEngineBuilder()
            .UseFileSystemProject(webHost.ContentRootPath)
            .UseMemoryCachingProvider()
            .Build();

        string templateContent = await File.ReadAllTextAsync(templatePath);

        // Use "this" as the model since we're passing the current instance
        return await engine.CompileRenderStringAsync("template-emails-contactus", templateContent, this);
    }
}