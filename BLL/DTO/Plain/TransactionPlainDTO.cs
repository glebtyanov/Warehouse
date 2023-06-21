using System.ComponentModel.DataAnnotations;

namespace BLL.DTO.Plain
{
    public class TransactionPlainDTO
    {
        [Required]
        public int TransactionId { get; set; }

        [Required]
        public int OrderId { get; set; }

        [Required]
        public DateTime TransactionDate { get; set; }

        [Required]
        public double Amount { get; set; }

        [Required]
        public string? PaymentMethod { get; set; }
    }
}
