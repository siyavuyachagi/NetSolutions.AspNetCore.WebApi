namespace Application.DTOs.Response
{
    public class PaymentTransactionDto
    {
        public Guid Id { get; set; }
        public decimal Amount { get; set; }
        public string Status { get; set; } // string for better serialization and readability
        public string? TransactionReference { get; set; }
        public string? AdditionalNotes { get; set; }
        public string? PaymentMethod { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
