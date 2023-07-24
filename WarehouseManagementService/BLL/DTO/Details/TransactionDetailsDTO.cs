namespace BLL.DTO.Plain
{
    public class TransactionDetailsDTO
    {
        public int TransactionId { get; set; }

        public DateTime TransactionDate { get; set; }

        public double Amount { get; set; }

        public string PaymentMethod { get; set; }

        public OrderPlainDTO? Order { get; set; }
    }
}
