using inventoryAppDomain.Entities.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace inventoryAppWebUi.Models
{
    public class SupplierViewModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required(ErrorMessage = "Supplier Name needed")]
        public string SupplierName { get; set; }

        [EmailAddress]
        [Required(ErrorMessage = "Supplier Email needed")]
        public string Email { get; set; }

        public SupplierStatus Status { get; set; }

        [Url]
        [Required(ErrorMessage = "Supplier web address needed")]
        public string Website { get; set; }

        [Required]
        public string TagNumber { get; set; }
    }
    public class SupplierErrorsMessages
    {
        public const string supplierNotFound = "Supplier not found!";
    }
}