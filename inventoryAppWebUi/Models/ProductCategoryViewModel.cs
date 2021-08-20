using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace inventoryAppWebUi.Models
{
    public class ProductCategoryViewModel
    {
        [Required (ErrorMessage = "Category Name should not be empty")]
        [Display(Name = "Category Name")]
        public string CategoryName { get; set; }
    }
}