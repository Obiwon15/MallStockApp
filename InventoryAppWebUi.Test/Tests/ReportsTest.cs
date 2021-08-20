using System;
using System.Collections.Generic;
using System.Web.Mvc;
using AutoMapper;
using inventoryAppDomain.Entities;
using inventoryAppDomain.Entities.Enums;
using inventoryAppDomain.Services;
using inventoryAppWebUi.Controllers;
using inventoryAppWebUi.Models;
using Moq;
using Newtonsoft.Json;
using NUnit.Framework;

namespace InventoryAppWebUi.Test.Tests
{
    /// <summary>
    /// Summary description for ReportControllerTest
    /// </summary>
    [TestFixture]
    public class ReportsTest
    {
        private Mock<IReportService> _mockReportService = new Mock<IReportService>();
        private Mock<IOrderService> _mockOrderService = new Mock<IOrderService>();
        private List<Order> _orders;

        [SetUp]
        public void Setup()
        {
            _orders = new List<Order>
            {
                new Order()
                {
                    Email = "tochukwu@gmail.com",
                    Price = 1000,
                    CreatedAt = DateTime.Now,
                    FirstName = "TKc",
                    LastName = "TYsmnd",
                    OrderId = 1,
                    OrderItems = new List<ProductCartItem>(),
                    PhoneNumber = "0094902302"
                },
                new Order()
                {
                    Email = "tochukwu@gmail.com",
                    Price = 1000,
                    CreatedAt = DateTime.Now,
                    FirstName = "TKc",
                    LastName = "TYsmnd",
                    OrderId = 2,
                    OrderItems = new List<ProductCartItem>(),
                    PhoneNumber = "0094902302"
                }
            };

            Mapper.Initialize(configuration => configuration.CreateMap<Report, ReportViewModel>());
        }

        [Test]
        [TestCase(TimeFrame.DAILY)]
        [TestCase(TimeFrame.WEEKLY)]
        [TestCase(TimeFrame.MONTHLY)]
        public void CreateReportTest_Success(TimeFrame timeFrame)
        {
            var report = new Report
            {
                Id = 1,
                Orders = _orders,
                CreatedAt = DateTime.Now,
                ProductSales = It.IsAny<string>(),
                ReportProducts = new List<Product>(),
                TimeFrame = timeFrame,
                TotalRevenueForReport = 1000,
            };

            var viewModel = new ReportViewModel
            {
                Orders = report.Orders,
                ReportProducts = report.ReportProducts,
                TotalRevenueForReport = report.TotalRevenueForReport
            };

            var controller = new ReportController(_mockReportService.Object);
            _mockOrderService.Setup(service => service.GetOrdersForTheDay()).Returns(_orders);
            _mockOrderService.Setup(service => service.GetOrdersForTheMonth()).Returns(_orders);
            _mockOrderService.Setup(service => service.GetOrdersForTheWeek()).Returns(_orders);
            _mockReportService.Setup(service => service.CreateReport(timeFrame)).Returns(report);

            if (controller.Index(timeFrame) is ViewResult result)
            {
                var resultModel =
                    JsonConvert.DeserializeObject<ReportViewModel>(JsonConvert.SerializeObject(result.Model));
                Assert.That(resultModel != null && resultModel.Orders.Count == viewModel.Orders.Count);
            }
        }

        [Test]
        [TestCase(TimeFrame.DAILY)]
        [TestCase(TimeFrame.WEEKLY)]
        [TestCase(TimeFrame.MONTHLY)]
        public void CreateReportTest_NoSales(TimeFrame timeFrame)
        {
            var controller = new ReportController(_mockReportService.Object);
            _mockOrderService.Setup(service => service.GetOrdersForTheDay()).Returns(new List<Order>());
            _mockOrderService.Setup(service => service.GetOrdersForTheMonth()).Returns(new List<Order>());
            _mockOrderService.Setup(service => service.GetOrdersForTheWeek()).Returns(new List<Order>());
            _mockReportService.Setup(service => service.CreateReport(timeFrame)).Returns(new Report());

            if (controller.Index(timeFrame) is ViewResult result)
            {
                var resultModel =
                    JsonConvert.DeserializeObject<ReportViewModel>(JsonConvert.SerializeObject(result.Model));
                Assert.That(resultModel != null && resultModel.Orders.Count == 0);
            }
        }
    }
}