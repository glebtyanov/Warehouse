namespace BLL.DTO.Plain
{
    public class CustomerDetailsDTO
    {
        public int CustomerId { get; set; }

        public string Name { get; set; }

        public string Address { get; set; }

        public string ContactNumber { get; set; }

        public string Email { get; set; }

        public List<OrderPlainDTO> Orders { get; set; }
    }
}
