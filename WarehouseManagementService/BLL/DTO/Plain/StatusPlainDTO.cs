using System.ComponentModel.DataAnnotations;

namespace BLL.DTO.Plain
{
    public class StatusPlainDTO
    {
        [Required]
        public int StatusId { get; set; }

        [Required]
        public string Name { get; set; }
    }
}
