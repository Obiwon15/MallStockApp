using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using inventoryAppDomain.Entities;
using inventoryAppDomain.Entities.Enums;


namespace inventoryAppWebUi.Models
{
    public class ProductViewModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public string ProductName { get; set; }


        [Required]
        [Display(Name = "Number of Units")]
        public int TotalUnitPerProducts { get; set; }


        [Required]
        [Display(Name = "Price Per Unit")]
        public int PricePerUnit { get; set; }


        [Required]
        [Display(Name = "Quantity Supplied")]
        public int Quantity { get; set; }

        [Required]
        public decimal Price { get; set; }


        [DataType(DataType.Date, ErrorMessage = "Date only")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Required]
        public DateTime ExpiryDate { get; set; }

        [Required]
        public string SupplierTag { get; set; }

        //public List<Supplier> Suppliers { get; set; }
        //public int SupplierId { get; set; }

        public List<ProductCategory> ProductCategory { get; set; }
        [Required]
        public int ProductCategoryId { get; set; }
    }

    public class CategoryViewModel
    {
        public int Id { get; set; }
        public string CategoryName { get; set; }
    }

    //public class ProductSupplier
    //{
    //    public int Id { get; set; }
    //    public string TagNumber { get; set; }
    //    public string SupplierName { get; set; }

    //}

}