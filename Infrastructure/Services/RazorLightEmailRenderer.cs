using Application.Interfaces;
using Microsoft.Extensions.Logging;
using RazorLight;

namespace Infrastructure.Services
{
    public class RazorLightEmailRenderer : IRazorLightEmailRenderer
    {
        private readonly RazorLightEngine _engine;
        private readonly ILogger<RazorLightEmailRenderer> _logger;

        public RazorLightEmailRenderer(ILogger<RazorLightEmailRenderer> logger)
        {
            var basePath = AppContext.BaseDirectory;
            var templateRoot = Path.GetFullPath(Path.Combine(basePath, @"..\..\..\..\Infrastructure\Templates\Emails"));

            _engine = new RazorLightEngineBuilder()
                .UseFileSystemProject(templateRoot)
                .UseMemoryCachingProvider()
                .EnableDebugMode()
                .Build();

            _logger = logger;
        }

        public async Task<string> RenderEmailTemplateAsync<T>(string templateKeyWithoutExtension, T model)
        {
            var templateKey = templateKeyWithoutExtension.EndsWith(".cshtml")
                ? templateKeyWithoutExtension
                : $"{templateKeyWithoutExtension}.cshtml";

            try
            {
                return await _engine.CompileRenderAsync(templateKey, model);
            }
            catch (TemplateNotFoundException ex)
            {
                _logger.LogError(ex, "Template not found: {TemplateKey}", templateKey);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error rendering template: {TemplateKey}", templateKey);
                throw;
            }
        }
    }


}


