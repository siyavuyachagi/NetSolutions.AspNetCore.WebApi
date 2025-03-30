using Google.Apis.Drive.v3.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Any;
using NetSolutions.Helpers;
using NetSolutions.WebApi.Data;
using NetSolutions.WebApi.Models.Domain;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json.Serialization;
using System.Web;

namespace NetSolutions.Services;

public interface IPayFast
{
    public enum Environment
    {
        Sandbox,Live
    }
    Task<Result<string>> InitAsync(PayFastRequest request, IPayFast.Environment environment = IPayFast.Environment.Live);
    Task<Result<string>> SubscribeAsync(PayFastSubscriptionRequest request);
}

public class PayFast : IPayFast
{
    private readonly PayFastCreds _payFastCreds;
    private readonly string _sandboxUrl = "https://sandbox.payfast.co.za/eng/process";
    private readonly string _liveUrl = "https://www.payfast.co.za/eng/process";

    public PayFast(PayFastCreds payFastCreds, ApplicationDbContext context)
    {
        _payFastCreds = payFastCreds;
    }

    public async Task<Result<string>> InitAsync(PayFastRequest request, IPayFast.Environment environment)
    {
        try
        {
            // Merchant details
            request.MerchantId = _payFastCreds.MerchantId;
            request.MerchantKey = _payFastCreds.MerchantKey;

            // Generate signature
            if(environment == IPayFast.Environment.Live) request.Signature = GenerateSignature(request);

            // Convert to JSON using Newtonsoft.Json
            var jsonPaymentRequest = JsonConvert.SerializeObject(request, new JsonSerializerSettings
            {
                ContractResolver = new Newtonsoft.Json.Serialization.DefaultContractResolver
                {
                    NamingStrategy = new Newtonsoft.Json.Serialization.SnakeCaseNamingStrategy() // Ensures lowercase JSON keys
                }
            });

            // Deserialize JSON back to a dictionary
            var kvpPaymentRequest = JsonConvert.DeserializeObject<Dictionary<string, string>>(jsonPaymentRequest);

            // Convert dictionary to query string
            var queryString = string.Join("&", kvpPaymentRequest.Select(kv => $"{HttpUtility.UrlEncode(kv.Key)}={HttpUtility.UrlEncode(kv.Value)}"));

            string url = IPayFast.Environment.Live == environment ? _liveUrl : _sandboxUrl;
            string requestUrl = $"{url}?{queryString}";

            return Result.Success(requestUrl);
        }
        catch (Exception ex)
        {
            return Result.Failed<string>(ex.Message);
        }
    }

    public async Task<Result<string>> SubscribeAsync(PayFastSubscriptionRequest request)
    {
        try
        {
            request.MerchantId = _payFastCreds.MerchantId;
            request.MerchantKey = _payFastCreds.MerchantKey;
            // Generate signature
            //request.Signature = GenerateSignature(request);

            // Convert to JSON using Newtonsoft.Json
            var jsonPaymentRequest = JsonConvert.SerializeObject(request, new JsonSerializerSettings
            {
                ContractResolver = new Newtonsoft.Json.Serialization.DefaultContractResolver
                {
                    NamingStrategy = new Newtonsoft.Json.Serialization.SnakeCaseNamingStrategy() // Ensures lowercase JSON keys
                }
            });

            // Deserialize JSON back to a dictionary
            var kvpPaymentRequest = JsonConvert.DeserializeObject<Dictionary<string, string>>(jsonPaymentRequest);

            // Convert dictionary to query string
            var queryString = string.Join("&", kvpPaymentRequest.Select(kv => $"{HttpUtility.UrlEncode(kv.Key)}={HttpUtility.UrlEncode(kv.Value)}"));

            string requestUrl = $"{_sandboxUrl}?{queryString}";

            return Result.Success(requestUrl);
        }
        catch (Exception ex)
        {
            return Result.Failed<string>(ex.Message);
        }
    }


    // Helper method to generate PayFast signature
    private string GenerateSignature(object request)
    {
        // Get all properties, exclude "signature", and remove null or empty values
        var properties = request.GetType().GetProperties()
            .Where(p => p.Name.ToLower() != "signature" && p.GetValue(request) != null && p.GetValue(request).ToString() != "")
            .OrderBy(p => p.Name.ToLower()) // Sort alphabetically by lowercase name
            .Select(p => $"{p.Name.ToLower()}={HttpUtility.UrlDecode(p.GetValue(request).ToString())}"); // Decode values

        // Create the parameter string
        var parameterString = string.Join("&", properties);

        // Get the passphrase if set
        var passphrase = _payFastCreds.PassPhrase;
        if (!string.IsNullOrEmpty(passphrase))
        {
            parameterString += $"&passphrase={HttpUtility.UrlDecode(passphrase)}";
        }

        // Calculate the MD5 hash
        using var md5 = MD5.Create();
        var hash = md5.ComputeHash(Encoding.UTF8.GetBytes(parameterString));

        // Convert to hexadecimal string (lowercase)
        return BitConverter.ToString(hash).Replace("-", "").ToLower();
    }


    private static string ConvertToFormUrlEncoded(object obj)
    {
        var properties = obj.GetType().GetProperties()
            .Where(p => p.GetValue(obj) != null)
            .Select(p => $"{WebUtility.UrlEncode(p.Name)}={WebUtility.UrlEncode(p.GetValue(obj)?.ToString())}");

        return string.Join("&", properties);
    }
}


public class PayFastCreds
{
    public string MerchantId { get; set; }
    public string MerchantKey { get; set; }
    public string PassPhrase { get; set; }
}

public class PayFastRequest
{
    // Merchant details
    [JsonPropertyName("merchant_id"), Required]
    public string MerchantId { get; set; }

    [JsonPropertyName("merchant_key"), Required]
    public string MerchantKey { get; set; }

    [JsonPropertyName("return_url"), Required, Url]
    public string ReturnUrl { get; set; }

    [JsonPropertyName("cancel_url"), Required, Url]
    public string CancelUrl { get; set; }

    [JsonPropertyName("notify_url"), Required, Url]
    public string NotifyUrl { get; set; }

    [JsonPropertyName("signature")]
    public string Signature { get; set; }

    // Buyer details
    [JsonPropertyName("name_first"), Required, MaxLength(100)]
    public string FirstName { get; set; }

    [JsonPropertyName("name_last"), Required, MaxLength(100)]
    public string LastName { get; set; }

    [JsonPropertyName("email_address"), Required, EmailAddress]
    public string EmailAddress { get; set; }
    [JsonPropertyName("cell_number"), MaxLength(20)]
    public string? CellNumber { get; set; }

    // Transaction details
    [JsonPropertyName("m_payment_id")]
    public string PaymentId { get; set; }

    [JsonPropertyName("amount"), Range(1, double.MaxValue)]
    public string Amount { get; set; }

    [JsonPropertyName("item_name"), Required]
    public string ItemName { get; set; }
    [JsonPropertyName("item_description"), MaxLength(250)]
    public string ItemDescription { get; set; }

    // Optional transaction details
    [JsonPropertyName("custom_str1")]
    public string? CustomStr1 { get; set; }
    [JsonPropertyName("custom_str2")]
    public string? CustomStr2 { get; set; }
}

public class PayFastSubscriptionRequest
{
    //Merchant details
    [JsonPropertyName("merchant_id"), Required]
    public string MerchantId { get; set; }

    [JsonPropertyName("merchant_key"), Required]
    public string MerchantKey { get; set; }

    [JsonPropertyName("return_url"), Required, Url]
    public string ReturnUrl { get; set; }

    [JsonPropertyName("cancel_url"), Required, Url]
    public string CancelUrl { get; set; }

    [JsonPropertyName("notify_url"), Required, Url]
    public string NotifyUrl { get; set; }
    [JsonPropertyName("signature")]
    public string Signature { get; set; }

    // **Buyer Details**
    [JsonPropertyName("name_first"), Required, MaxLength(100)]
    public string FirstName { get; set; }

    [JsonPropertyName("name_last"), Required, MaxLength(100)]
    public string LastName { get; set; }

    [JsonPropertyName("email_address"), Required, EmailAddress]
    public string EmailAddress { get; set; }

    [JsonPropertyName("cell_number"), MaxLength(20)]
    public string? CellNumber { get; set; }

    //Subscription details
    [JsonPropertyName("m_payment_id")]
    public Guid PaymentId { get; set; } // Optional: Your internal payment reference ID
    [JsonPropertyName("item_name"), Required, MaxLength(100)]
    public string ItemName { get; set; } // Subscription name

    [JsonPropertyName("item_description"), MaxLength(255)]
    public string ItemDescription { get; set; }

    [JsonPropertyName("amount"), Required, Range(1, double.MaxValue, ErrorMessage = "Amount must be greater than 0.")]
    public decimal Amount { get; set; } // Amount per cycle

    [JsonPropertyName("currency"), Required]
    public string Currency { get; set; } = "ZAR";

    [JsonPropertyName("subscription_type"), Required]
    public int SubscriptionType { get; set; } = 1; // Always 1 for PayFast subscriptions

    [JsonPropertyName("recurring_amount"), Required, Range(1, double.MaxValue, ErrorMessage = "Recurring amount must be greater than 0.")]
    public decimal RecurringAmount { get; set; } // Monthly recurring amount

    [JsonPropertyName("frequency"), Required, Range(1, 12, ErrorMessage = "Recurring frequency must be between 1 (monthly) and 12 (yearly).")]
    public int Frequency { get; set; } = 3; // 3 = Monthly (PayFast code)

    [JsonPropertyName("cycles"), Required, Range(1, 60, ErrorMessage = "Recurring cycle must be between 1 and 60.")]
    public int Cycles { get; set; } = 0; // Number of months subscription should run 0 = Infinite

    [JsonPropertyName("payment_method"), Required, RegularExpression("^(cc|dc)$", ErrorMessage = "Payment method must be 'cc' (credit card) or 'dc' (debit card).")]
    public string PaymentMethod { get; set; } = "cc"; // Payment method

    [JsonPropertyName("recurring")]
    public bool Recurring { get; set; } = true; // Always true for subscriptions

    // Optional transaction details
    [JsonPropertyName("custom_str1")]
    public string? CustomStr1 { get; set; } // Custom field for extra info

    [JsonPropertyName("custom_str2")]
    public string? CustomStr2 { get; set; } // Another custom field
}

