namespace DAL.Entities
{
    public class DepartmentWorker
    {
        // attributes
        public int DepartmentId { get; set; }

        public int WorkerId { get; set; }

        // refered by one
        public Department? Department { get; set; }

        public Worker? Worker { get; set; } 
    }
}
