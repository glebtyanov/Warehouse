namespace BLL.DTO.Plain
{
    public class WorkerDetailsDTO
    {
        public int WorkerId { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Address { get; set; }

        public string ContactNumber { get; set; }

        public string Email { get; set; }

        public DateTime HireDate { get; set; }

        public PositionPlainDTO? Position { get; set; }

        // refered by many
        public List<OrderPlainDTO>? Orders { get; set; }

        public List<DepartmentPlainDTO>? Departments { get; set; }
    }
}
