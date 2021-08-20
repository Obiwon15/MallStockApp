using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
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
    /// Summary description for NotificationsControllerTest
    /// </summary>
    [TestFixture]
    public class NotificationsTest
    {
        private Mock<INotificationService> _mockNotifications = new Mock<INotificationService>();
        private List<Notification> _notifications;

        [SetUp]
        public void Setup()
        {
            _notifications = new List<Notification>
            {
                new Notification
                {
                    Id = 1,
                    Title = "Test Notification",
                    CreatedAt = DateTime.Now,
                    NotificationCategory = NotificationCategory.RUNNING_OUT_OF_STOCK,
                    NotificationDetails = "Tests for Notifications",
                    NotificationStatus = NotificationStatus.READ,
                    NotificationType = NotificationType.REOCCURRING
                },
                new Notification
                {
                    Id = 2,
                    Title = "Test Notification",
                    CreatedAt = DateTime.Now,
                    NotificationCategory = NotificationCategory.EXPIRATION,
                    NotificationDetails = "Tests for Notifications",
                    NotificationStatus = NotificationStatus.UN_READ,
                    NotificationType = NotificationType.REOCCURRING
                },
                new Notification
                {
                    Id = 3,
                    Title = "Test Notification",
                    CreatedAt = DateTime.Now,
                    NotificationCategory = NotificationCategory.RUNNING_OUT_OF_STOCK,
                    NotificationDetails = "Tests for Notifications",
                    NotificationStatus = NotificationStatus.UN_READ,
                    NotificationType = NotificationType.REOCCURRING
                },
                new Notification
                {
                    Id = 4,
                    Title = "Test Notification",
                    CreatedAt = DateTime.Now,
                    NotificationCategory = NotificationCategory.RUNNING_OUT_OF_STOCK,
                    NotificationDetails = "Tests for Notifications",
                    NotificationStatus = NotificationStatus.UN_READ,
                    NotificationType = NotificationType.REOCCURRING
                },
                new Notification
                {
                    Id = 5,
                    Title = "Test Notification",
                    CreatedAt = DateTime.Now,
                    NotificationCategory = NotificationCategory.EXPIRATION,
                    NotificationDetails = "Tests for Notifications",
                    NotificationStatus = NotificationStatus.READ,
                    NotificationType = NotificationType.REOCCURRING
                },
                new Notification
                {
                    Id = 1,
                    Title = "Test Notification",
                    CreatedAt = DateTime.Now,
                    NotificationCategory = NotificationCategory.RUNNING_OUT_OF_STOCK,
                    NotificationDetails = "Tests for Notifications",
                    NotificationStatus = NotificationStatus.READ,
                    NotificationType = NotificationType.REOCCURRING
                },
            };
        }

        [Test]
        public void IndexTest_For_Notifications()
        {
            var controller = new NotificationsController(_mockNotifications.Object);
            _mockNotifications.Setup(service => service.GetAllNotifications()).Returns(_notifications.ToList());
            _mockNotifications.Setup(service => service.GetNotificationsCount(NotificationStatus.UN_READ))
                .Returns(_notifications
                    .Where(notification => notification.NotificationStatus == NotificationStatus.UN_READ).Count);
            _mockNotifications.Setup(service => service.GetAllNonReadNotifications()).Returns(_notifications
                .Where(notification => notification.NotificationStatus == NotificationStatus.UN_READ).ToList());

            if (controller.Index() is ViewResult result)
            {
                Assert.True(result.Model is NotificationsPageViewModel model && model.AllNotifications.Any());
            }
        }

        [Test]
        public void IndexTest_For_No_New_Notification()
        {
            var controller = new NotificationsController(_mockNotifications.Object);
            _mockNotifications.Setup(service => service.GetAllNotifications()).Returns(_notifications.ToList());
            _mockNotifications.Setup(service => service.GetNotificationsCount(NotificationStatus.UN_READ))
                .Returns(0);
            _mockNotifications.Setup(service => service.GetAllNonReadNotifications()).Returns(new List<Notification>());

            if (controller.Index() is ViewResult result)
            {
                Assert.False(result.Model is NotificationsPageViewModel model && model.UnreadNotifications.Any());
            }
        }

        [Test]
        public void GetRecentFive_Up_To_Five()
        {
            var controller = new NotificationsController(_mockNotifications.Object);
            _mockNotifications.Setup(service => service.GetRecentFive())
                .Returns(_notifications.Skip(Math.Max(0, _notifications.Count - 5)).Take(5).ToList());

            if (controller.GetRecentFive() is JsonResult result)
            {
                Assert.True(result.Data is List<Notification> model && model.Any() && model.Count <= 5);
            }
        }

        [Test]
        [TestCase(1)]
        public void GetByIdTest(int id)
        {
            var notification = _notifications.FirstOrDefault(notification1 => notification1.Id == id);
            var controller = new NotificationsController(_mockNotifications.Object);
            _mockNotifications.Setup(service => service.GetNotificationById(id))
                .Returns(_notifications.FirstOrDefault(notification1 => notification1.Id == id));

            if (controller.GetNotificationById(id) is JsonResult result)
            {
                Assert.NotNull(result.Data);
                Assert.AreEqual(result.Data, notification);
            }
        }

        [Test]
        [TestCase(1000)]
        public void GetByIdTest_Not_Exist(int id)
        {
            var controller = new NotificationsController(_mockNotifications.Object);
            _mockNotifications.Setup(service => service.GetNotificationById(id))
                .Returns(_notifications.FirstOrDefault(notification1 => notification1.Id == id));

            if (controller.GetNotificationById(id) is JsonResult result)
            {
                Assert.Null(result.Data);
            }
        }

        [Test]
        [TestCase(1)]
        public async Task MarkAsRead_Success(int id)
        {
            var controller = new NotificationsController(_mockNotifications.Object);
            _mockNotifications.Setup(service => service.MarkAsRead(id)).ReturnsAsync(() => true);

            if (await controller.MarkAsRead(id) is JsonResult result)
            {
                var message = JsonConvert.DeserializeObject<JsonResponse>(JsonConvert.SerializeObject(result.Data))?.message;
                Assert.True(message != null && message.Equals("Marked As Read Successful"));
            }
        }

        [Test]
        [TestCase(1000)]
        public async Task MarkAsRead_Failed(int id)
        {
            var controller = new NotificationsController(_mockNotifications.Object);
            _mockNotifications.Setup(service => service.MarkAsRead(id)).ReturnsAsync(() => true);

            if (await controller.MarkAsRead(id) is JsonResult result)
            {
                var message = JsonConvert.DeserializeObject<JsonResponse>(JsonConvert.SerializeObject(result.Data))
                    ?.message;
                Assert.False(message != null && message.Equals("Failed"));
            }
        }

        private class JsonResponse
        {
            public string status { get; set; }
            public string message { get; set; }
        }
    }
}