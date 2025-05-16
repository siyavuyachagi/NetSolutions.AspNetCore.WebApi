using Application.DTOs.Request;
using Application.Helpers;
using Application.Interfaces;
using Infrastructure.Configurations;
using Infrastructure.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace Infrastructure.Services
{
    public class PayFast : IPayFast
    {
        private readonly string _sandboxUrl = "https://sandbox.payfast.co.za/eng/process";
        private readonly string _liveUrl = "https://www.payfast.co.za/eng/process";
        private readonly ApplicationDbContext _context;
        private readonly PayFastConfig _payFastCreds;
        private readonly ILogger<PayFast> _logger;

        public PayFast(
            ApplicationDbContext context, 
            PayFastConfig payFastCreds, 
            ILogger<PayFast> logger)
        {
            _context = context;
            _payFastCreds = payFastCreds;
            _logger = logger;
        }

        public async Task<Result<string>> InitAsync(PayFastRequest request, IPayFast.Environment environment)
        {
            try
            {
                // Merchant details
                request.MerchantId = _payFastCreds.MerchantId;
                request.MerchantKey = _payFastCreds.MerchantKey;

                // Generate signature
                if (environment == IPayFast.Environment.Live) request.Signature = GenerateSignature(request);

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
                return Result.Failure<string>(ex.Message);
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
                return Result.Failure<string>(ex.Message);
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

        public async Task<Result<string>> SubscribeAsync(PayFastSubscriptionRequest request, IPayFast.Environment environment = IPayFast.Environment.Live)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> ValidatePaymentNotificationAsync(Dictionary<string, string> formData, HttpContext httpContext)
        {
            try
            {
                // 1. Check if the notification is from the correct environment
                bool isSandbox = true;
                string expectedHost = isSandbox ? "sandbox.payfast.co.za" : "www.payfast.co.za";

                // Get client IP
                string clientIp = httpContext.Connection.RemoteIpAddress?.ToString() ?? "";

                // In production, validate that the request is coming from PayFast's IP range
                // For sandbox testing, you might skip this check
                if (!isSandbox)
                {
                    // Simple example - in production you'd check against PayFast's IP ranges
                    // This is a placeholder for the actual IP validation
                    var payFastIps = new List<string> { "204.93.162.1", "204.93.163.1" };
                    if (!payFastIps.Contains(clientIp))
                    {
                        return false;
                    }
                }

                // 2. Verify the signature
                string receivedSignature = formData.ContainsKey("signature") ? formData["signature"] : "";

                // Create a dictionary without the signature for validation
                var dataForSignature = formData
                    .Where(kvp => kvp.Key != "signature")
                    .ToDictionary(kvp => kvp.Key, kvp => kvp.Value);

                // Calculate the expected signature
                string calculatedSignature = CalculateSignature(dataForSignature);

                if (receivedSignature != calculatedSignature)
                {
                    return false;
                }

                // 3. Verify the payment data (optional but recommended)
                // Check if the amount matches your records
                if (formData.ContainsKey("m_payment_id"))
                {
                    string paymentId = formData["m_payment_id"];
                    var transaction = await _context.PaymentTransactions
                        .FirstOrDefaultAsync(t => t.Id == Guid.Parse(paymentId));

                    if (transaction != null && formData.ContainsKey("amount_gross"))
                    {
                        decimal notifiedAmount = decimal.Parse(formData["amount_gross"]);
                        if (transaction.Amount != notifiedAmount)
                        {
                            return false;
                        }
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return false;
            }
        }

        public async Task<Result<bool>> CancelSubscriptionAsync(string subscriptionReference)
        {
            throw new NotImplementedException();
        }

        private string CalculateSignature(Dictionary<string, string> formData)
        {
            try
            {
                // Order parameters alphabetically
                var orderedData = formData.OrderBy(d => d.Key);

                // Create parameter string
                var parameterString = string.Join("&",
                    orderedData.Select(d => $"{d.Key}={Uri.EscapeDataString(d.Value)}"));

                // Add passphrase if configured
                var passphrase = _payFastCreds.PassPhrase;
                if (!string.IsNullOrEmpty(passphrase))
                {
                    parameterString += $"&passphrase={Uri.EscapeDataString(passphrase)}";
                }

                // Generate MD5 hash
                using var md5 = MD5.Create();
                byte[] hashBytes = md5.ComputeHash(Encoding.UTF8.GetBytes(parameterString));

                // Convert to hex string
                return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        }
    }
}


