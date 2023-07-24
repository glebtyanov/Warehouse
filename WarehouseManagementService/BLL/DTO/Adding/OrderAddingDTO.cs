using System.ComponentModel.DataAnnotations;

namespace BLL.DTO.Adding
{
    public class OrderAddingDTO
    {
        [Required]
        public int CustomerId { get; set; }

        [Required]
        public DateTime OrderDate { get; set; }

        [Required]
        public double ProductAmount { get; set; }

        [Required]
        public int WorkerId { get; set; }

        [Required]
        public int StatusId { get; set; } = 1;
    }
}
