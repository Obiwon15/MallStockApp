using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace inventoryAppWebUi.Models
{
    public class IndexPageViewModel
    {
        public int TotalNumberOfSupplier { get; set; }
        public int TotalNumberOfProducts { get; set; }
        public decimal TotalRevenue { get; set; }
        public int TotalSales { get; set; }
    }
}