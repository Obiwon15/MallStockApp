using inventoryAppDomain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace inventoryAppWebUi.Models
{
    public class ProductCartViewModel
    {

        public List<ProductCartItem> CartItems { get; set; }
        
        public decimal ProductCartTotal { get; set; }
        public int ProductCartItemsTotal { get; set; }
        public int ProductId { get; set; }
    }
}