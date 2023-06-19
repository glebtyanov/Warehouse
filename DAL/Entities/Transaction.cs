namespace DAL.Entities
{
    public class Transaction
    {
        public int TransactionId { get; set; }

        public int OrderId { get; set; }

        public DateTime TransactionDate { get; set; }

        public double Amount { get; set; }

        public string? PaymentMethod { get; set; }

        // refers one
        public Order? Order { get; set; }
    }
}
