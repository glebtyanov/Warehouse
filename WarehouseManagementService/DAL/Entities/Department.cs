namespace DAL.Entities
{
    public class Department
    {
        // attributes
        public int DepartmentId { get; set; }

        public string? Name { get; set; }

        public string? Location { get; set; }

        public int Capacity { get; set; }

        // refered by many
        public List<Worker>? Workers { get; set; }
    }
}
