using inventoryAppDomain.Services;
using inventoryAppWebUi.Models;
using System.Web.Mvc;
using inventoryAppDomain.Entities.Enums;
using Microsoft.AspNet.Identity;
using System;

namespace inventoryAppWebUi.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ISupplierService _supplierService;
        private readonly IProductService _ProductService;
        private readonly IProductCartService _ProductCartService;
        private readonly IOrderService _orderService;

        public HomeController( ISupplierService supplierService, IProductService ProductService, IProductCartService ProductCartService, IOrderService orderService)
        {
            _supplierService = supplierService;
            _ProductService = ProductService;
            _ProductCartService = ProductCartService;
            _orderService = orderService;
        }

        public ActionResult Index(string paymentCompleted)
        {
            //check if user already has as cart
            if (Request.IsAuthenticated && User.Identity.IsAuthenticated)
            {
                var cart = _ProductCartService.GetCart(User.Identity.GetUserId(),CartStatus.ACTIVE);
                if (cart == null)
                {
                    cart = _ProductCartService.CreateCart(User.Identity.GetUserId());   
                }
            }
            var totalNumberOfSupplier = new IndexPageViewModel
            {
                TotalNumberOfSupplier = _supplierService.TotalNumberOfSupplier(),
                TotalNumberOfProducts = _ProductService.GetAllProducts().Count,
                TotalRevenue = _orderService.GetTotalRevenue(),
                TotalSales = _orderService.GetTotalSales()

            };
            if (paymentCompleted != null)
            {
                Console.WriteLine(paymentCompleted);
                if (paymentCompleted.Equals("True"))
                {
                    TempData["dispensed"] = "dispensed";
                    ViewBag.Dispensed = "dispensed";
                }
                else
                {
                    TempData["failure"] = "failure";
                    ViewBag.Failure = "failure";
                }
            }
            return View(totalNumberOfSupplier);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}