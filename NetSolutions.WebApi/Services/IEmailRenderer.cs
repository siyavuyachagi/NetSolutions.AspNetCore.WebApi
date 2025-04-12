using RazorLight;

namespace NetSolutions.WebApi.Services;

public interface IRazorLightEmailRenderer
{
    Task<string> RenderEmailTemplateAsync<T>(string templateName, T model);
}

public class RazorLightEmailRenderer : IRazorLightEmailRenderer
{
    private readonly RazorLightEngine _engine;

    public RazorLightEmailRenderer()
    {
        // Initialize RazorLight to use templates from the file system
        _engine = new RazorLightEngineBuilder()
            .UseFileSystemProject(Path.Combine(Directory.GetCurrentDirectory(), "Templates", "Emails"))
            .UseMemoryCachingProvider()
            .Build();
    }

    public async Task<string> RenderEmailTemplateAsync<T>(string templateName, T model)
    {
        return await _engine.CompileRenderAsync($"{templateName}.cshtml", model);
    }
}
