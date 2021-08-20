using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
//using Microsoft.VisualStudio.TestTools.UnitTesting;
using inventoryAppDomain.Services;
using Moq;
using inventoryAppWebUi.Controllers;
using System.Web.Mvc;
using NUnit.Framework;
using inventoryAppDomain.Entities;
using inventoryAppDomain.IdentityEntities;
using Microsoft.AspNet.Identity.EntityFramework;
using inventoryAppDomain.Entities.Enums;
using inventoryAppDomain.Repository;
using System.Web;
using System.Threading.Tasks;
using AutoMapper;
using inventoryAppWebUi.Models;

namespace InventoryAppWebUi.Test
{
    [TestFixture]
    public class ProductCartControllerTest
    {
        private readonly Mock<IProductCartService> _mockProductCart;
        private readonly Mock<IProductService> _mockProduct;
        private readonly ProductCartController _cartController;

        public ProductCartControllerTest()
        {
            _mockProductCart = new Mock<IProductCartService>();
            _cartController = new ProductCartController(_mockProductCart.Object, _mockProduct.Object);
        }

        [SetUp]
        public void Setup()
        {
            Mapper.Initialize(configuration => configuration.CreateMap<Product, ProductViewModel>());
        }

        [Test]
        public void GetProduct()
        {
            var userId = Guid.NewGuid().ToString();

            var user = new List<ApplicationUser>
            {
                new ApplicationUser
                {
                    Id = userId, Email = "abc@abc.com", UserName = "abc@abc.com", PhoneNumber = "0908777"
                },
                new ApplicationUser
                {
                    Id = "utsr", Email = "efg@efg.com", UserName = "efg@efg.com", PhoneNumber = "0908777"
                }
            };

            var roles = new List<IdentityRole>
            {
                new IdentityRole
                {
                    Id = "zxy", Name = "Audit"
                },
                new IdentityRole
                {
                    Id = "wvu", Name = "Cashier"
                }
            };
            var newProduct = new List<Product>
            {
                new Product
                {
                    Id = 45, ProductName = "drax", Price = 4000, Quantity = 55, CreatedAt = DateTime.Today,
                    ExpiryDate = DateTime.Today.AddDays(9), CurrentProductStatus = ProductStatus.NOT_EXPIRED
                },
                new Product
                {
                    Id = 77, ProductName = "antrax", Price = 7000, Quantity = 35, CreatedAt = DateTime.Today,
                    ExpiryDate = DateTime.Today.AddDays(9), CurrentProductStatus = ProductStatus.NOT_EXPIRED
                }
            };
            var singleProduct = new Product
            {
                Id = 88,
                ProductName = "antraxe",
                Price = 8000,
                Quantity = 35,
                CreatedAt = DateTime.Today,
                ExpiryDate = DateTime.Today.AddDays(9),
                CurrentProductStatus = ProductStatus.NOT_EXPIRED
            };
            var newProductCartItems = new List<ProductCartItem>
            {
                new ProductCartItem
                {
                    Id = 80, Amount = 4000, ProductId = 45, Product = newProduct.Find(v => v.Id == 45), ProductCartId = 191
                }
            };

            var newCart = new ProductCart
            {
                Id = 191,
                CartStatus = CartStatus.ACTIVE,
                ApplicationUser = user.Find(z => z.Id == userId),
                ApplicationUserId = userId,
                ProductCartItems = newProductCartItems
            };
            _mockProductCart.Setup(b => b.GetProductById(88)).Returns(singleProduct);

            var result = _cartController.GetProduct(88) as ViewResult;

            Assert.AreEqual(result.Model, singleProduct);
        }


      

        [Test]
        public void ProductCartTotalCountTest()
        {
            var newUser = new ApplicationUser
            {
                Id = "utsr",
                Email = "efg@efg.com",
                UserName = "efg@efg.com",
                PhoneNumber = "0908777"
            };
            var singleProduct = new Product
            {
                Id = 88,
                ProductName = "antraxe",
                Price = 8000,
                Quantity = 35,
                CreatedAt = DateTime.Today,
                ExpiryDate = DateTime.Today.AddDays(9),
                CurrentProductStatus = ProductStatus.NOT_EXPIRED
            };
            var newProductCartItems = new List<ProductCartItem>
            {
                new ProductCartItem
                {
                    Id = 80, Amount = 4000, ProductId = 45, Product = singleProduct, ProductCartId = 191
                }
            };

            var newCart = new ProductCart
            {
                Id = 191,
                CartStatus = CartStatus.ACTIVE,
                ApplicationUser = newUser,
                ApplicationUserId = newUser.Id,
                ProductCartItems = newProductCartItems
            };

            _mockProductCart.Setup(x => x.GetProductCartTotalCount(newUser.Id));
        }

        [Test]
        public void ProductCartSumTotalTest()
        {
            var newUser = new ApplicationUser
            {
                Id = "utsr",
                Email = "efg@efg.com",
                UserName = "efg@efg.com",
                PhoneNumber = "0908777"
            };
            var singleProduct = new Product
            {
                Id = 88,
                ProductName = "antraxe",
                Price = 8000,
                Quantity = 35,
                CreatedAt = DateTime.Today,
                ExpiryDate = DateTime.Today.AddDays(9),
                CurrentProductStatus = ProductStatus.NOT_EXPIRED
            };
            var newProductCartItems = new List<ProductCartItem>
            {
                new ProductCartItem
                {
                    Id = 80, Amount = 4000, ProductId = 45, Product = singleProduct, ProductCartId = 191
                }
            };

            var newCart = new ProductCart
            {
                Id = 191,
                CartStatus = CartStatus.ACTIVE,
                ApplicationUser = newUser,
                ApplicationUserId = newUser.Id,
                ProductCartItems = newProductCartItems
            };
            _mockProductCart.Setup(z => z.GetProductCartTotalCount(newUser.Id));
        }

        [Test]
        public void RemoveFromShoppingCartTest()
        {
            var newUser = new ApplicationUser
            {
                Id = "utsr",
                Email = "efg@efg.com",
                UserName = "efg@efg.com",
                PhoneNumber = "0908777"
            };
            var singleProduct = new Product
            {
                Id = 88,
                ProductName = "antraxe",
                Price = 8000,
                Quantity = 35,
                CreatedAt = DateTime.Today,
                ExpiryDate = DateTime.Today.AddDays(9),
                CurrentProductStatus = ProductStatus.NOT_EXPIRED
            };
            var newProductCartItems = new List<ProductCartItem>
            {
                new ProductCartItem
                {
                    Id = 80, Amount = 4000, ProductId = 45, Product = singleProduct, ProductCartId = 191
                }
            };

            var newCart = new ProductCart
            {
                Id = 191,
                CartStatus = CartStatus.ACTIVE,
                ApplicationUser = newUser,
                ApplicationUserId = newUser.Id,
                ProductCartItems = newProductCartItems
            };
            _mockProductCart.Setup(b => b.RemoveFromCart(singleProduct, newUser.Id));

            var result = _cartController.RemoveFromShoppingCart(80) as ViewResult;

            Assert.That(result, Is.EqualTo(null));
        }
    }
}