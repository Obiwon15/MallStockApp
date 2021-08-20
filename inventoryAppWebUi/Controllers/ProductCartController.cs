using inventoryAppDomain.Services;
using inventoryAppWebUi.Models;
using System.Web.Mvc;
using inventoryAppDomain.Entities.Enums;
using Microsoft.AspNet.Identity;
using System;
using AutoMapper;
using System.Collections.Generic;
using inventoryAppDomain.Entities;

namespace inventoryAppWebUi.Controllers
{
    public class ProductCartController : Controller
    {
        private readonly IProductCartService _ProductCartService;
        private readonly IProductService _ProductService;

        public ProductCartController(IProductCartService ProductCartService, IProductService ProductService)
        {
            _ProductCartService = ProductCartService;
            _ProductService = ProductService;
        }

        public ActionResult Index()
        {
            var userId = User.Identity.GetUserId();
            var ProductCartCountTotal = _ProductCartService.GetProductCartTotalCount(userId);
            var ProductCartViewModel = new ProductCartViewModel
            {
                CartItems = _ProductCartService.GetProductCartItems(userId, CartStatus.ACTIVE),
                ProductCartItemsTotal = ProductCartCountTotal,
                ProductCartTotal = _ProductCartService.GetProductCartSumTotal(userId),

            };
            return View(ProductCartViewModel);
        }
        public ActionResult GetProduct(int id)
        {
            var Product = _ProductCartService.GetProductById(id);
            return View(Product);
        }

        public ActionResult AddToShoppingCart(int id)
        {
            var userId = User.Identity.GetUserId();
            var selectedProduct = _ProductCartService.GetProductById(id);

        //    var prescribeVM = Mapper.Map<ProductPrescriptionViewModel>(selectedProduct);
            try
            {
                if (selectedProduct == null)
                {
                    return HttpNotFound();
                }

                _ProductCartService.AddToCart(selectedProduct, userId);
                return RedirectToAction("FilteredProductsList", "Product");
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return RedirectToAction("FilteredProductsList", "Product");
            }
         
        }


        public ActionResult RemoveFromShoppingCart(int id)
        {
            try
            {
                var userId = User.Identity.GetUserId();
                var cartItem = _ProductCartService.GetProductCartItemById(id);
                var selectedItem = _ProductCartService.GetProductById(cartItem.Product.Id);

                if (selectedItem != null)
                {
                    _ProductCartService.RemoveFromCart(selectedItem, userId);
                }

                return RedirectToAction("Index");
            }
            catch(Exception e)
            {
                ViewBag.Error = e.Message;
                return RedirectToAction("Index", "ProductCart");
            }
        }

        public ActionResult RemoveAllCart()
        {
            var userId = User.Identity.GetUserId();
            _ProductCartService.ClearCart(userId);
            return RedirectToAction("Index");
        }
    }
}
