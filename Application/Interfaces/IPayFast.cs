using Application.DTOs.Request;
using Application.Helpers;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Application.Interfaces
{
    public interface IPayFast
    {
        public enum Environment
        {
            Sandbox,
            Live
        }

        /// <summary>
        /// Initializes a once-off payment and returns the redirect/payment URL.
        /// </summary>
        Task<Result<string>> InitAsync(PayFastRequest request, Environment environment = Environment.Live);

        /// <summary>
        /// Initializes a subscription payment and returns the redirect/payment URL.
        /// </summary>
        Task<Result<string>> SubscribeAsync(PayFastSubscriptionRequest request, Environment environment = Environment.Live);

        /// <summary>
        /// Validates the payment notification from PayFast (IPN).
        /// </summary>
        Task<bool> ValidatePaymentNotificationAsync(Dictionary<string, string> formData, HttpContext httpContext);

        /// <summary>
        /// Cancels a subscription using its token or reference.
        /// </summary>
        Task<Result<bool>> CancelSubscriptionAsync(string subscriptionReference);
    }
}

public enum BillingCycle
{
    None, // No billing cycle selected
    [Display(Name = "Fourth night")]
    FourthNight,   // Every 2 weeks
    Weekly,        // Every 7 days
    [Display(Name = "Monthly")]
    Monthly,       // Every month
    Quarterly,     // Every 3 months
    Yearly         // Every year
}
