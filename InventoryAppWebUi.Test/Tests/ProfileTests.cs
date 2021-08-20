using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;
using AutoMapper;
using inventoryAppDomain.IdentityEntities;
using inventoryAppDomain.Repository;
using inventoryAppDomain.Services;
using inventoryAppWebUi.Controllers;
using inventoryAppWebUi.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using Moq;
using NUnit.Framework;

namespace InventoryAppWebUi.Test.Tests
{
    [TestFixture]
    public class ProfileTests
    {
        private readonly Mock<IRoleService> _mockRoleService = new Mock<IRoleService>();
        private readonly Mock<IProfileService> _mockProfileService = new Mock<IProfileService>();
        private readonly Mock<INotificationService> _mockNotificationService = new Mock<INotificationService>();

        [SetUp]
        public void Setup()
        {
            Mapper.Initialize(configuration => configuration.CreateMap<UpdateUserRoleViewModel, MockViewModel>());
        }

        [Test]
        [TestCase("userId")]
        public async Task RemoveUser_Success(string userId)
        {
            var controller = new AccountController(_mockRoleService.Object, _mockProfileService.Object,
                _mockNotificationService.Object);
            var role = new IdentityRole
            {
                Name = "TestRole",
                Id = Guid.NewGuid().ToString(),
            };
            var applicationUser = new ApplicationUser
            {
                Email = "tk.@gmail.com",
            };
            var identityUserRole = new IdentityUserRole
            {
                RoleId = role.Id,
                UserId = applicationUser.Id
            };
            role.Users.Add(identityUserRole);
            _mockRoleService.Setup(service => service.GetRoleByUser(userId)).ReturnsAsync(role.Name);
            _mockProfileService.Setup(service => service.RemoveUser(userId)).Returns(() => Task.CompletedTask);


            var result = await controller.RemoveUser(userId);
            if (result != null)
            {
                Assert.That(controller.ViewBag.Error == null);
            }
        }

        [Test]
        [TestCase("userId")]
        public async Task RemoveUser_Failure(string userId)
        {
            var controller = new AccountController(_mockRoleService.Object, _mockProfileService.Object,
                _mockNotificationService.Object);
            _mockRoleService.Setup(service => service.GetRoleByUser(userId)).ReturnsAsync("");
            _mockProfileService.Setup(service => service.RemoveUser(userId))
                .Callback(() => throw new Exception("Cashiers not found"));


            var result = await controller.RemoveUser(userId);
            if (result != null)
            {
                Assert.That(controller.ViewBag.Error != null);
            }
        }

        [Test]
        [TestCase("userId", "Test2")]
        public async Task UpdateUserRole_Success(string userId, string roleName)
        {
            var controller = new AccountController(_mockRoleService.Object, _mockProfileService.Object,
                _mockNotificationService.Object);

            var mockViewModel = new MockViewModel
            {
                UserId = userId,
                UpdatedUserRole = roleName,
            };
            var viewModel = new UpdateUserRoleViewModel
            {
                Email = "tl.com",
                Roles = new List<string> {"roleName"},
                UserId = userId,
                UpdatedUserRole = roleName
            };

            _mockRoleService.Setup(service => service.ChangeUserRole(userId, roleName))
                .Returns(() => Task.CompletedTask);
            _mockProfileService.Setup(service => service.ChangeUserRole(mockViewModel))
                .ReturnsAsync(new ApplicationUser());

            if (await controller.UpdateUserRole(viewModel) is RedirectToRouteResult)
            {
                Assert.That(controller.ViewBag.RoleChangeSuccessful != null);
            }
        }
    }
}