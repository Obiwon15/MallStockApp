using System;
using System.Text;
using System.Collections.Generic;

using inventoryAppDomain.Services;
using Moq;
using inventoryAppWebUi.Controllers;
using System.Web.Mvc;
using inventoryAppDomain.Entities;
using inventoryAppWebUi.Models;
using NUnit.Framework;
using inventoryAppDomain.Entities.Enums;
using AutoMapper;
using static Newtonsoft.Json.JsonConvert;

namespace InventoryAppWebUi.Test
{
   [TestFixture]
    public class ProductControllerTest
    {
        private readonly Mock<IProductService> _mockProduct;
        private readonly Mock<ISupplierService> _mockSupp;
        private readonly Mock<IProductCartService> _mockProductCart;
        private readonly ProductController _dcontroller;
        private readonly ProductCartController _cartController;

        public ProductControllerTest()
        {
            _mockProduct = new Mock<IProductService>();
            _mockSupp = new Mock<ISupplierService>();
            _mockProductCart = new Mock<IProductCartService>();
            _dcontroller = new ProductController(_mockProduct.Object, _mockSupp.Object);
            _cartController = new ProductCartController(_mockProductCart.Object, _mockProduct.Object);
        }
        [SetUp]
        public void Setup()
        {
            Mapper.Initialize(configuration => configuration.CreateMap<Product, ProductViewModel>());
            Mapper.Initialize(configuration => configuration.CreateMap<ProductCategoryViewModel, ProductCategory>());
        }

        [Test]
        public void FilteredProductListTest()
        {
            var searchString = "";
          
            _mockProduct.Setup(q => q.GetAvailableProductFilter(searchString));

            var result = _dcontroller.FilteredProductsList(searchString) as ViewResult;

            Assert.IsNotNull(result);
        }

        [Test]
        public void SaveProductTest()
        {
            var newProductCategory = new ProductCategory
            {
                CategoryName = "Pills",
                Id = 99
            };
            var ProductId = 222;
            var newProduct = new Product
            {
                Id = ProductId,
                Quantity = 45,
                Price = 55,
                SupplierTag = "afghi",
                ExpiryDate = DateTime.Today.AddDays(25),
                ProductName = "purft",
                CreatedAt = DateTime.Today,
                CurrentProductStatus = ProductStatus.NOT_EXPIRED,
                ProductCategoryId = 99
            };
            var newProductVM = new ProductViewModel
            {
                Id = ProductId,
                Quantity = 45,
                Price = 55,
                SupplierTag = "afghi",
                ExpiryDate = DateTime.Today.AddDays(25),
                ProductName = "purft"
            };

            _mockProduct.Setup(q => q.AddProduct(newProduct));

            var result = _dcontroller.SaveProduct(newProductVM);

            Assert.That(result, Is.Not.Null);
        }
        [Test]
        public void SaveProductCategoryTest()
        {
            var newProductCategory = new ProductCategory
            {
                CategoryName = "Pills",
                Id = 99
            };
            var newProductCategoryVm = new ProductCategoryViewModel
            {
                CategoryName = "Pills"
                
            };
            _mockProduct.Setup(v => v.AddProductCategory(newProductCategory));

            if (_dcontroller.SaveProductCategory(newProductCategoryVm) is JsonResult result)
            {
                var response = DeserializeObject<JsonResponse>(SerializeObject(result.Data))
                    ?.response;
                Assert.True(response != null && response.Equals("success"));
            }

        }

        [Test]
        public void RemoveProductCategoryTest()
        {
            var newProductCategory = new ProductCategory
            {
                CategoryName = "Pills",
                Id = 99
            };

            _mockProduct.Setup(z => z.RemoveProductCategory(newProductCategory.Id));

            var result = _dcontroller.RemoveProductCategory(newProductCategory.Id) as ViewResult;

            Assert.That(result, Is.EqualTo(null));
        }

        [Test]
        public void RemoveProductTest()
        {
            var ProductId = 222;
            var newProduct = new Product
            {
                Id = ProductId,
                Quantity = 45,
                Price = 55,
                SupplierTag = "afghi",
                ExpiryDate = DateTime.Today.AddDays(25),
                ProductName = "purft",
                CreatedAt = DateTime.Today,
                CurrentProductStatus = ProductStatus.NOT_EXPIRED,
                ProductCategoryId = 99
            };

            _mockProduct.Setup(z => z.RemoveProduct(newProduct.Id));

            var result = _cartController.GetProduct(ProductId) as ViewResult;

            Assert.That(result != null);
        }

        [Test]
        public void AddProductTest()
        {
            var result = _dcontroller.AllProducts() as ViewResult;

            Assert.AreNotEqual("AllProducts", result.Model);
        }

     

        private class JsonResponse
        {
            public string status { get; set; }
            public string response { get; set; }
        }
    }
}
