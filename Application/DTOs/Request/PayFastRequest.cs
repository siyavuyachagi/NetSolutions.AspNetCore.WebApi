using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Application.DTOs.Request
{
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

}
