using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NetSolutions.WebApi.Data;

namespace NetSolutions.WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TestsController : ControllerBase
{
    private readonly ILogger<TestsController> logger;
    private readonly ApplicationDbContext context;

    public TestsController(
        ILogger<TestsController> logger,
        ApplicationDbContext context)
    {
        this.logger = logger;
        this.context = context;
    }


    public class FileUploadModel
    {
        public List<IFormFile> Files { get; set; }
        public string? AdditionalNotes { get; set; }
    }
    [HttpPost]
    public async Task<IActionResult> Create(FileUploadModel model)
    {
        try
        {
            if (!ModelState.IsValid) return ValidationProblem(ModelState);

            return Created(string.Empty, "Resource created successful");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message);
            throw;
        }
    }
}
