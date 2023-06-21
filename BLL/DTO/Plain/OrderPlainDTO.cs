using System.ComponentModel.DataAnnotations;

namespace BLL.DTO.Plain
{
    public class OrderPlainDTO
    {
        [Required]
        public int OrderId { get; set; }

        [Required]
        public DateTime OrderDate { get; set; }

        [Required]
        public double ProductAmount { get; set; }

        [Required]
        public int WorkerId { get; set; }

        [Required]
        public int StatusId { get; set; }

        [Required]
        public int CustomerId { get; set; }
    }
}
