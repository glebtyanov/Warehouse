using System.ComponentModel.DataAnnotations;

namespace BLL.DTO.Plain
{
    public class PositionPlainDTO
    {
        [Required]
        public int PositionId { get; set; }

        [Required]
        public string Name { get; set; }
    }
}
