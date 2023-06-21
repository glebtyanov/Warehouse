namespace DAL.Entities
{
    public class Order
    {
        // attributes
        public int OrderId { get; set; }

        public int CustomerId { get; set; }

        public DateTime OrderDate { get; set; }

        public double ProductAmount { get; set; }

        public int StatusId { get; set; }

        public int WorkerId { get; set; }

        // refers one
        public Customer? Customer { get; set; }

        public Status? Status { get; set; }

        // refered by one
        public Transaction? Transaction { get; set; }
        public Worker? Worker { get; set; }

        // refered by many
        public ICollection<Product>? Products { get; set; }

    }
}
