namespace DAL.Entities
{
    public class Customer
    {
        // attributes
        public int CustomerId { get; set; }

        public string? Name { get; set; }

        public string? Address { get; set; }

        public string? ContactNumber { get; set; }
        
        public string? Email { get; set; }

        // refered by many
        public ICollection<Order>? Orders { get; set; }
    }
}
