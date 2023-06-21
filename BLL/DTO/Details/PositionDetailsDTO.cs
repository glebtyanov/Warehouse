using DAL.Entities;
using System.ComponentModel.DataAnnotations;

namespace BLL.DTO.Plain
{
    public class PositionDetailsDTO
    {
        public int PositionId { get; set; }

        public string Name { get; set; }

        public List<WorkerPlainDTO> Workers { get; set; } 
    }
}
