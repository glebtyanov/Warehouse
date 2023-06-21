namespace BLL.DTO.Plain
{
    public class OrderDetailsDTO
    {
        public int OrderId { get; set; }

        public DateTime OrderDate { get; set; }

        public double ProductAmount { get; set; }

        public CustomerPlainDTO? Customer { get; set; }

        public StatusPlainDTO? Status { get; set; }

        public TransactionPlainDTO? Transaction { get; set; }

        public WorkerPlainDTO? Worker { get; set; }

        public List<ProductPlainDTO>? Products { get; set; }
    }
}
