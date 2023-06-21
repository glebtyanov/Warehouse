using System.ComponentModel.DataAnnotations;

namespace BLL.DTO.Adding
{
    public class StatusAddingDTO
    {
        [Required]
        public string Name { get; set; }
    }
}
