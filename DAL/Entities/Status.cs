namespace DAL.Entities
{
    public class Status
    {
        public int StatusId { get; set; }

        public string? Name { get; set; }

        // refered by many
        public ICollection<Order>? Orders { get; set; }
    }
}
