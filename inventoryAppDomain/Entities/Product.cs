using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using inventoryAppDomain.Entities.Enums;

namespace inventoryAppDomain.Entities
{
    public class Product
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required]
        public int Id { get; set; }

        [Required]
        public string ProductName { get; set; }

        [Required]
        public int TotalUnitPerProducts { get; set; }

        [Required]
        public int PricePerUnit { get; set; }

        [Required]
        public int Quantity { get; set; }

        [Required]
        public decimal Price { get; set; }

        [Required]
        [Display(Name = "Expiration Date")]
        public DateTime ExpiryDate { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        [Required]
        public ProductStatus CurrentProductStatus { get; set; } = ProductStatus.NOT_EXPIRED;

        [Required]
        public string SupplierTag { get; set; }

        [Required]
        public int ProductCategoryId { get; set; }
        public ProductCategory ProductCategory { get; set; }
      
    }
}