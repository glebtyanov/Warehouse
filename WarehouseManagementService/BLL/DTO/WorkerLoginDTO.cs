using System.ComponentModel.DataAnnotations;

namespace BLL.DTO
{
    public class WorkerLoginDTO
    {
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
