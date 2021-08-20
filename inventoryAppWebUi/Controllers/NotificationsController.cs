using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using inventoryAppDomain.Entities.Enums;
using inventoryAppDomain.Services;
using inventoryAppWebUi.Models;
using Microsoft.AspNetCore.Http;

namespace inventoryAppWebUi.Controllers
{
	public class NotificationsController : Controller
	{
		private readonly INotificationService _notificationService;

		public NotificationsController(INotificationService notificationService)
		{
			_notificationService = notificationService;
		}

		public ActionResult Index()
		{
			var notificationViewModel = new NotificationsPageViewModel
			{
				AllNotifications = _notificationService.GetAllNotifications(),
				NotificationsCount = _notificationService.GetNotificationsCount(NotificationStatus.UN_READ),
				UnreadNotifications = _notificationService.GetAllNonReadNotifications(),
			};
			return View(notificationViewModel);
		}

		public ActionResult GetRecentFive()
		{
			try
			{
				var notifications = _notificationService.GetRecentFive();
				return Json(notifications, JsonRequestBehavior.AllowGet);
			}
			catch (Exception e)
			{
				return Json(e.Message, JsonRequestBehavior.AllowGet);
			}
		}

		public ActionResult GetNotificationById(int id)
		{
			try
			{
				var notification = _notificationService.GetNotificationById(id);
				return Json(notification, JsonRequestBehavior.AllowGet);
			}
			catch (Exception e)
			{
				return Json(e.Message, JsonRequestBehavior.AllowGet);
			}
		}

		[HttpPost]
		public async Task<ActionResult> MarkAsRead(int id)
		{
			try
			{
				var result = await _notificationService.MarkAsRead(id);

				if (result)
				{
					return Json(new { status = "200", message = "Marked As Read Successful" }, JsonRequestBehavior.DenyGet);
				}
				return Json(new { status = "400", message = "Failed" }, JsonRequestBehavior.DenyGet);
			}
			catch (Exception e)
			{
				Response.StatusCode = StatusCodes.Status400BadRequest;
				return Json(e.Message, JsonRequestBehavior.AllowGet);
			}
		}

		[HttpPost]
		public async Task<ActionResult> MarkAllAsRead()
		{
			try
			{
				await _notificationService.MarkAllAsRead();
				return Json("Success", JsonRequestBehavior.DenyGet);
			}
			catch (Exception e)
			{
				Response.StatusCode = StatusCodes.Status400BadRequest;
				return Json(e.Message, JsonRequestBehavior.AllowGet);
			}
		}

		public ActionResult GetAllNotifications(string notificationCategory = "")
		{
			try
			{
				if (notificationCategory.Equals("") | notificationCategory.Equals("All"))
				{
					var notifications = _notificationService.GetAllNotifications();
					return View(notifications);
				}
				else
				{
					var s = (NotificationCategory)Enum.Parse(typeof(NotificationCategory), notificationCategory);
					var notifications = _notificationService.GetNotificationsByCategory(s);
					return View(notifications);

				}

			}
			catch
			{
				Response.StatusCode = StatusCodes.Status400BadRequest;
				// TODO: error page here
				return View();
			}
		}

		public ActionResult GetNotificationsCount()
		{
			try
			{
				return Json(_notificationService.GetNotificationsCount(NotificationStatus.UN_READ),
					JsonRequestBehavior.AllowGet);
			}
			catch (Exception e)
			{
				return Json(new { status = "401", message = e.Message }, JsonRequestBehavior.AllowGet);
			}
		}

		public ActionResult ShowAllNotifications()
		{
			return View();
		}

	}
}