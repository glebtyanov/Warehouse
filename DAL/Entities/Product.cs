namespace DAL.Entities
{
    public class Product
    {
        public int ProductId { get; set; }

        public string? Name { get; set; }
        
        public string? Description { get; set; }
        
        public int Quantity { get; set; }
        
        public double Price { get; set; }

        // refered by many
        public ICollection<Order>? Orders { get; set; }
    }
}
