using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace inventoryAppWebUi.Models
{
    public class ProductSearchViewModel
    {
        public string SearchString { get; set; }
        public List<inventoryAppDomain.Entities.Product> Products { get; set; }
    }
}