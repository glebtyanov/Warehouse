using System.ComponentModel.DataAnnotations;

namespace BLL.DTO.Adding
{
    public class TransactionAddingDTO
    {
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
