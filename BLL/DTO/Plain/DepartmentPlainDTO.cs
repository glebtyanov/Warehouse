using System.ComponentModel.DataAnnotations;

namespace BLL.DTO.Plain
{
    public class DepartmentPlainDTO
    {
        [Required]
        public int DepartmentId { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Location { get; set; }

        [Required]
        public int Capacity { get; set; }
    }
}
