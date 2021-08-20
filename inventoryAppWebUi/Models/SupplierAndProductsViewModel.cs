using inventoryAppDomain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace inventoryAppWebUi.Models
{
    public class SupplierAndProductsViewModel
    {
        public SupplierViewModel SupplierViewModel { get; set; }
        public IEnumerable<ProductViewModel> ProductViewModel { get; set; }
    }
}