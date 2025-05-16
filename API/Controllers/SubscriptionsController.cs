using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class SubscriptionsController : ControllerBase
{
    private readonly ILogger<SubscriptionsController> _logger;
    private readonly ISubscription _subscriptionRepository;

    public SubscriptionsController(
        ILogger<SubscriptionsController> logger,
        ISubscription subscriptionRepository)
    {
        _logger = logger;
        _subscriptionRepository = subscriptionRepository;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        try
        {
            var subscriptions = await _subscriptionRepository.GetSubscriptionsAsync();
            return Ok(subscriptions);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            throw;
        }
    }

    [HttpGet("{Id}")]
    public async Task<IActionResult> Details([FromRoute] Guid Id)
    {
        try
        {
            var subscription = await _subscriptionRepository.GetSubscriptionAsync(Id);
            return Ok(subscription);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            throw;
        }
    }
}
