using System.ComponentModel.DataAnnotations;

namespace BLL.DTO.Adding
{
    public class OrderProductAddingDTO
    {
        [Required]
        public int ProductId { get; set; }

        [Required]
        public int OrderId { get; set; }
    }
}
