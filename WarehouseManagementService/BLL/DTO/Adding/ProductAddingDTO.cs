using System.ComponentModel.DataAnnotations;

namespace BLL.DTO.Adding
{
    public class ProductAddingDTO
    {
        [Required]
        public string Name { get; set; }

        public string Description { get; set; }

        [Required]
        public int Quantity { get; set; }

        [Required]
        public double Price { get; set; }
    }
}
