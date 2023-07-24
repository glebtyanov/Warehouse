using System.ComponentModel.DataAnnotations;

namespace BLL.DTO.Adding
{
    public class DepartmentWorkerAddingDTO
    {
        [Required]
        public int DepartmentId { get; set; }

        [Required]
        public int WorkerId { get; set; }
    }
}
