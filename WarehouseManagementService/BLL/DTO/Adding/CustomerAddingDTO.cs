using System.ComponentModel.DataAnnotations;

namespace BLL.DTO.Adding
{
    public class CustomerAddingDTO
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Address { get; set; }

        [Phone]
        public string? ContactNumber { get; set; }

        [EmailAddress]
        public string? Email { get; set; }
    }
}
