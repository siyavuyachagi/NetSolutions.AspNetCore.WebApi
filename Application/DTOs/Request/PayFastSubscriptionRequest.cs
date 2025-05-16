using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Application.DTOs.Request
{
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

}
