namespace BLL.DTO.Plain
{
    public class ProductDetailsDTO
    {
        public int ProductId { get; set; }

        public string Name { get; set; }
        
        public string Description { get; set; }
        
        public int Quantity { get; set; }
        
        public double Price { get; set; }

        public List<OrderPlainDTO>? Orders { get; set; }
    }
}
