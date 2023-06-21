using System.ComponentModel.DataAnnotations;

namespace BLL.DTO.Plain
{
    public class CustomerPlainDTO
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
