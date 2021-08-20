using inventoryAppDomain.Entities;
using inventoryAppDomain.IdentityEntities;
using inventoryAppDomain.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using inventoryAppDomain.Entities.Enums;
using inventoryAppWebUi.Models;
using Microsoft.AspNet.Identity;

namespace inventoryAppWebUi.Controllers
{
    [Authorize]
    public class OrderController : Controller
    {
        private readonly IProductCartService _ProductCartService;
        private readonly IOrderService _orderService;
        public OrderController(IOrderService orderService, IProductCartService ProductCartService)
        {
            _ProductCartService = ProductCartService;
            _orderService = orderService;
        }
        
        public ActionResult Invoice(string payWithCash)
        {
            if (payWithCash != null)
            {
                ViewBag.PayWithCash = true;
            }
            return View();
        }
        
        
        [HttpPost]
        public ActionResult Checkout(OrderViewModel viewModel)
        {
            var userId = User.Identity.GetUserId();
            var items = _ProductCartService.GetProductCartItems(userId, CartStatus.ACTIVE);

            if (!items.Any())
            {
                ModelState.AddModelError("", @"Your cart is empty");
            }

            if (ModelState.IsValid)
            {
                var clearedBy = User.Identity.GetUserName();

                var order = _orderService.CreateOrder(Mapper.Map<OrderViewModel, Order>(viewModel), userId, clearedBy);
                _ProductCartService.RefreshCart(userId);
                return RedirectToAction("ProcessPayment", "Payment", new {orderId = order.OrderId});
             
            }
            return View("Invoice", viewModel);

        }
        
        [HttpPost]
        public ActionResult CheckoutWithCash(OrderViewModel viewModel)
        {
            var userId = User.Identity.GetUserId();
            var items = _ProductCartService.GetProductCartItems(userId,CartStatus.ACTIVE);

            if (!items.Any())
            {
                ModelState.AddModelError("", @"Your cart is empty");
            }

            if (ModelState.IsValid)
            {
                var clearedBy = User.Identity.GetUserName();

                var order = _orderService.CreateOrder(Mapper.Map<OrderViewModel, Order>(viewModel), userId, clearedBy);
                _ProductCartService.RefreshCart(userId);
                TempData["dispensed"] = "dispensed";
                return RedirectToAction("FilteredProductsList", "Product");
            }
            return View("Invoice", viewModel);

        }

        public ActionResult CheckoutComplete()
        {
            ViewBag.CheckoutCompleteMessage = "Product Dispensed";
            return View();
        }
    }
}