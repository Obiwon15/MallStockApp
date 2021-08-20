using System.ComponentModel.DataAnnotations;

namespace inventoryAppWebUi.Models
{
    public class TransactionViewModel
    {
        [Required]
        public string CardNumber { get; set; }
        [Required]
        [MinLength(3)]
        [MaxLength(4)]
        public string Cvv { get; set; }
        [Required]
        [MaxLength(2)]
        [MinLength(2)]
        public string ExpiryMonth { get; set; }
        [Required]
        [MaxLength(2)]
        [MinLength(2)]
        public string ExpiryYear { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [MaxLength(4)]
        [MinLength(4)]
        [DataType(DataType.Password)]
        public string Pin { get; set; }

        public int OrderId { get; set; }
    }
}