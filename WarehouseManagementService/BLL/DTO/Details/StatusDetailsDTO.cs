namespace BLL.DTO.Plain
{
    public class StatusDetailsDTO
    {
        public int StatusId { get; set; }

        public string Name { get; set; }

        public List<OrderPlainDTO>? Orders { get; set; }
    }
}
