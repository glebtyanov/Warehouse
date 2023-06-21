using System.ComponentModel.DataAnnotations;

namespace BLL.DTO.Plain
{
    public class ProductPlainDTO
    {
        [Required]
        public int ProductId { get; set; }

        [Required]
        public string Name { get; set; }

        public string Description { get; set; }

        [Required]
        public int Quantity { get; set; }

        [Required]
        public double Price { get; set; }
    }
}
