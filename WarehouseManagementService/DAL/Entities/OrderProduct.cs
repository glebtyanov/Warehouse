namespace DAL.Entities
{
    public class OrderProduct
    {
        public int OrderId { get; set; }

        public int ProductId { get; set; }

        // refers one
        public Order? Order { get; set; }
        public Product? Product { get; set; }
    }
}
