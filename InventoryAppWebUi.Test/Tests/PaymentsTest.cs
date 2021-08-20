using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;
using inventoryAppDomain.Entities;
using inventoryAppDomain.Entities.Enums;
using inventoryAppDomain.Entities.MonnifyDtos;
using inventoryAppDomain.Services;
using inventoryAppWebUi.Controllers;
using Moq;
using NUnit.Framework;

namespace InventoryAppWebUi.Test.Tests
{
    [TestFixture]
    public class PaymentsTest
    {
        private Mock<IPaymentService> _mockPaymentService = new Mock<IPaymentService>();
        private Mock<IOrderService> _mockOrderService = new Mock<IOrderService>();
        private Mock<ITransactionService> _mockTransactionService = new Mock<ITransactionService>();

        [Test]
        [TestCase(1)]
        public async Task InitiatePayment_Success(int orderId)
        {
            var order = new Order()
            {
                Email = "tochukwu@gmail.com",
                Price = 1000,
                CreatedAt = DateTime.Now,
                FirstName = "TKc",
                LastName = "TYsmnd",
                OrderId = 1,
                OrderItems = new List<ProductCartItem>(),
                PhoneNumber = "0094902302"
            };

            var transaction = new Transaction()
            {
                Amount = "1000",
                Id = 1,
                Order = order,
                OrderId = 1,
                ReferenceNumber = It.IsAny<string>(),
                TransactionStatus = TransactionStatus.PENDING,
                GeneratedReferenceNumber = It.IsAny<string>()
            };
            var controller = new PaymentController(_mockPaymentService.Object);
            _mockOrderService.Setup(service => service.GetOrderById(orderId)).Returns(order);
            _mockTransactionService
                .Setup(service => service.CreateTransaction(It.IsAny<string>(), It.IsAny<string>(), orderId))
                .ReturnsAsync(transaction);
            _mockPaymentService.Setup(service => service.InitiatePayment(orderId)).ReturnsAsync(() =>
                new InitTransactionResponseBody
                {
                    apiKey = It.IsAny<String>(),
                    checkoutUrl = "www.google.com",
                    merchantName = It.IsAny<string>(),
                    paymentReference = It.IsAny<string>(),
                    transactionReference = It.IsAny<string>(),
                    enabledPaymentMethod = new[] {"CARD"},
                    incomeSplitConfig = It.IsAny<object[]>()
                });
            
            var result = await controller.ProcessPayment(orderId) as RedirectResult;
            Assert.That(result?.Url != null);
            
        }

        [Test]
        [TestCase(1)]
        public async Task InitiatePayment_Failure(int orderId)
        {
            var controller = new PaymentController(_mockPaymentService.Object);
            _mockPaymentService.Setup(service => service.InitiatePayment(orderId)).ReturnsAsync(() =>
                new InitTransactionResponseBody
                {
                    apiKey = It.IsAny<String>(),
                    checkoutUrl = null,
                    merchantName = It.IsAny<string>(),
                    paymentReference = It.IsAny<string>(),
                    transactionReference = It.IsAny<string>(),
                    enabledPaymentMethod = new[] {"CARD"},
                    incomeSplitConfig = It.IsAny<object[]>()
                });
            
            var result = await controller.ProcessPayment(orderId) as RedirectResult;
            Assert.That(result?.Url == null);
        }

        [Test]
        [TestCase("sdjsdsn")]
        public async Task VerifyPayment_Success(string paymentReference)
        {
            var transaction = new Transaction()
            {
                Amount = "1000",
                Id = 1,
                Order = It.IsAny<Order>(),
                OrderId = 1,
                ReferenceNumber = It.IsAny<string>(),
                TransactionStatus = TransactionStatus.PENDING,
                GeneratedReferenceNumber = It.IsAny<string>()
            };
            var controller = new PaymentController(_mockPaymentService.Object);
            _mockTransactionService.Setup(service => service.GetTransactionByGeneratedRef(paymentReference))
                .ReturnsAsync(transaction);
            _mockPaymentService.Setup(service => service.VerifyPayment(paymentReference)).ReturnsAsync(() => true);

            await controller.VerifyPayment(paymentReference);
            Assert.That(controller.ViewBag.PaymentResponse);
        }

        [Test]
        [TestCase("sdjsdsn")]
        public async Task VerifyPayment_Failure(string paymentReference)
        {
            var controller = new PaymentController(_mockPaymentService.Object);
            _mockTransactionService.Setup(service => service.GetTransactionByGeneratedRef(paymentReference))
                .ReturnsAsync(() => null);
            _mockPaymentService.Setup(service => service.VerifyPayment(paymentReference)).ReturnsAsync(() => false);

            await controller.VerifyPayment(paymentReference);
            Assert.That(!controller.ViewBag.PaymentResponse);
        }
    }
}