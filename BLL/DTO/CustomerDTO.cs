using System.ComponentModel.DataAnnotations;

namespace BLL.DTO
{
    public class CustomerDTO
    {
        [Required]
        public int CustomerId { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Address { get; set; }

        [Required]
        public string ContactNumber { get; set; }

        [Required]
        public string Email { get; set; }
    }
}
