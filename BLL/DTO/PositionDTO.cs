using DAL.Entities;
using System.ComponentModel.DataAnnotations;

namespace BLL.DTO
{
    public class OrderDTO
    {
        [Required]
        public int OrderId { get; set; }

        [Required]
        public int CustomerId { get; set; }

        [Required]
        public DateTime OrderDate { get; set; }

        [Required]
        public double ProductAmount { get; set; }

        [Required]
        public int WorkerId { get; set; }
    }
}
