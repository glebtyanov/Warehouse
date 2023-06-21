namespace DAL.Entities
{
    public class Position
    {
        public int PositionId { get; set; }

        public string? Name { get; set; }

        // refered by many
        public ICollection<Worker>? Workers { get; set; }
    }
}
