using System.ComponentModel.DataAnnotations;

namespace BLL.DTO.Adding
{
    public class DepartmentAddingDTO
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Location { get; set; }

        [Required]
        public int Capacity { get; set; }
    }
}
