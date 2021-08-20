using inventoryAppDomain.Entities.Enums;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace inventoryAppDomain.Entities
{
    public class Supplier
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required (ErrorMessage = "Supplier Name needed")]
        public string SupplierName { get; set; }

        [EmailAddress]
        [Required(ErrorMessage = "Supplier Email needed")]
        public string Email { get; set; }

        [Url]
        [Required(ErrorMessage = "Supplier web address needed")]
        public string Website { get; set; }

        public SupplierStatus Status { get; set; } = SupplierStatus.Active;

        [Required]
        public int GrossAmountOfProductsSupplied { get; set; }

        [Required]
        public string TagNumber { get; set; }
    }
}