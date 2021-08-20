using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Web;
using inventoryAppDomain.Entities;
using inventoryAppDomain.Entities.Enums;
using inventoryAppDomain.ExtensionMethods;
using inventoryAppDomain.IdentityEntities;
using inventoryAppDomain.Services;
using Microsoft.AspNet.Identity.Owin;

namespace inventoryAppDomain.Repository
{
    public class ReportService : IReportService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IOrderService _orderService;
        private readonly IProductCartService _ProductCartService;
        private readonly IProductService _ProductService;

        public ReportService(IOrderService orderService, IProductCartService ProductCartService, IProductService ProductService)
        {
            _orderService = orderService;
            _ProductCartService = ProductCartService;
            _ProductService = ProductService;
            _dbContext = HttpContext.Current.GetOwinContext().Get<ApplicationDbContext>();
        }

        private static string GenerateSalesTable(List<ProductCartItem> cartItems)
        {
            var sb = new StringBuilder();
            const string table = @"
                                <table class= "" table table-hover table-bordered text-left "">
                                <thead>
                                    <tr class= ""table-success "">
                                    <th>Product Name</th>
                                    <th>Quantity</th>
                                    <th>Price</th>
                                    </tr>
                                </thead>";
            sb.Append(table);
            foreach (var item in cartItems)
            {
                var row = $@"<tbody>
                                <tr class=""info"" style="" cursor: pointer"">
                                <td class=""font-weight-bold"">{item.Product.ProductName}</td>
                                <td class=""font-weight-bold"">{item.Amount}</td>
                                <td class=""font-weight-bold"">{item.Product.Price}</td>
                         </tr>
                         </tbody>";
                sb.Append(row);
            }

            sb.Append("</Table>");
            return sb.ToString();
        }

        public Report GetReportByFunc(Func<Report, bool> func)
        {
            return _dbContext.Reports.FirstOrDefault(func);
        }

        public bool GetReportBoolByFunc(Func<Report, bool> func)
        {
            return _dbContext.Reports.Any(func);
        }

        public Report CreateReport(TimeFrame timeFrame)
        {
            Report report;
            switch (timeFrame)
            {
                case TimeFrame.DAILY:
                {
                    Func<Report, bool> dailyFunc = report1 => report1.CreatedAt.Date == DateTime.Now.Date && report1.TimeFrame == timeFrame;
                    report = GetReportByFunc(dailyFunc) ?? new Report();
                    var orders = _orderService.GetOrdersForTheDay();
                    report.Orders = orders;
                    report.TimeFrame = timeFrame;
                    report.TotalRevenueForReport = orders.Select(order => order.Price).Sum();

                    var ProductItem = new List<ProductCartItem>();
                    var Products = new List<Product>();
                    
                    foreach (var order in orders)
                    {
                        foreach (var ProductCartItem in order.OrderItems)
                        {
                            ProductItem.Add(_ProductCartService.GetProductCartItemById(ProductCartItem.Id));
                            Products.Add(_ProductService.GetProductById(ProductCartItem.ProductId));
                        }
                    }

                    report.ProductSales = GenerateSalesTable(ProductItem);
                    report.ReportProducts = Products;

                    if (GetReportBoolByFunc(dailyFunc))
                    {
                        _dbContext.Entry(report).State = EntityState.Modified;
                    }
                    else
                    {
                        _dbContext.Reports.Add(report);
                    }

                    _dbContext.SaveChanges();
                    return report;
                }
                case TimeFrame.WEEKLY:
                {
                    var beginningOfWeek = DateTime.Now.FirstDayOfWeek();
                    var lastDayOfWeek = DateTime.Now.LastDayOfWeek();
                    Func<Report, bool> weeklyFunc = report1 =>
                        report1.CreatedAt.Month.Equals(beginningOfWeek.Month) &&
                        report1.CreatedAt.Year.Equals(beginningOfWeek.Year) && (report1.CreatedAt >= beginningOfWeek &&
                        report1.CreatedAt <= lastDayOfWeek && report1.TimeFrame == timeFrame);

                    
                    report = GetReportByFunc(weeklyFunc) ?? new Report();

                    var orders = _orderService.GetOrdersForTheWeek();
                    report.Orders = orders;
                    report.TimeFrame = timeFrame;
                    report.TotalRevenueForReport = orders.Select(order => order.Price).Sum();

                    var ProductItem = new List<ProductCartItem>();
                    var Products = new List<Product>();
                    
                    foreach (var order in orders)
                    {
                        foreach (var ProductCartItem in order.OrderItems)
                        {
                            ProductItem.Add(_ProductCartService.GetProductCartItemById(ProductCartItem.Id));
                            Products.Add(_ProductService.GetProductById(ProductCartItem.ProductId));
                        }
                    }

                    report.ReportProducts = Products;
                    report.ProductSales = GenerateSalesTable(ProductItem);

                    if (GetReportBoolByFunc(weeklyFunc))
                    {
                        _dbContext.Entry(report).State = EntityState.Modified;
                    }
                    else
                    {
                        _dbContext.Reports.Add(report);
                    }

                    _dbContext.SaveChanges();
                    return report;
                }
                case TimeFrame.MONTHLY:
                {
                    Func<Report, bool> monthlyFunc = report1 =>
                        report1.CreatedAt.Month.Equals(DateTime.Now.Month) &&
                        report1.CreatedAt.Year.Equals(DateTime.Now.Year) && report1.TimeFrame == timeFrame;
                    
                    report = GetReportByFunc(monthlyFunc);
                    if (report == null)
                    {
                        report = new Report();
                    }

                    var orders = _orderService.GetOrdersForTheMonth();
                    report.Orders = _orderService.GetOrdersForTheMonth();
                    report.TimeFrame = timeFrame;
                    report.TotalRevenueForReport = orders.Select(order => order.Price).Sum();

                    var ProductItem = new List<ProductCartItem>();
                    var Products = new List<Product>();
                    
                    foreach (var order in orders)
                    {
                        foreach (var ProductCartItem in order.OrderItems)
                        {
                            ProductItem.Add(_ProductCartService.GetProductCartItemById(ProductCartItem.Id));
                            Products.Add(_ProductService.GetProductById(ProductCartItem.ProductId));
                        }
                    }

                    report.ProductSales = GenerateSalesTable(ProductItem);
                    report.ReportProducts = Products;
                    if (GetReportBoolByFunc(monthlyFunc))
                    {
                        _dbContext.Entry(report).State = EntityState.Modified;
                    }
                    else
                    {
                        _dbContext.Reports.Add(report);
                    }

                    _dbContext.SaveChanges();
                    return report;
                }
                default: return null;
            }
        }
    }
}