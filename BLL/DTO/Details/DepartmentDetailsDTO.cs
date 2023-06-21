namespace BLL.DTO.Plain
{
    public class DepartmentDetailsDTO
    {
        public int DepartmentId { get; set; }

        public string Name { get; set; }

        public string Location { get; set; }

        public int Capacity { get; set; }

        public List<WorkerPlainDTO> Workers { get; set; }
    }
}
