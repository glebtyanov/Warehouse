using System.ComponentModel.DataAnnotations;

namespace BLL.DTO.Adding
{
    public class PositionAddingDTO
    {
        [Required]
        public string Name { get; set; }
    }
}
