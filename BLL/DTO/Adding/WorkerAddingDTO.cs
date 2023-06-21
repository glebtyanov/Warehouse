using System.ComponentModel.DataAnnotations;

namespace BLL.DTO.Adding
{
    public class WorkerAddingDTO
    {
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public string Address { get; set; }

        [Phone]
        public string ContactNumber { get; set; }

        [EmailAddress]
        public string? Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
