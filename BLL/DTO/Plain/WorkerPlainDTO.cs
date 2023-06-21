using System.ComponentModel.DataAnnotations;

namespace BLL.DTO.Plain
{
    public class WorkerPlainDTO
    {
        [Required]
        public int WorkerId { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public string Address { get; set; }

        [Phone]
        public string ContactNumber { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public int PositionId { get; set; }

        [Required]
        public DateTime HireDate { get; set; }
    }
}
