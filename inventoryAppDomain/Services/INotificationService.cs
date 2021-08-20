using System.Collections.Generic;
using System.Threading.Tasks;
using inventoryAppDomain.Entities;
using inventoryAppDomain.Entities.Enums;

namespace inventoryAppDomain.Services
{
    public interface INotificationService
    {
        Task<Notification> CreateNotification(string title, string details, NotificationType notificationType, NotificationCategory notificationCategory);
        List<Notification> GetAllNotifications();
        List<Notification> GetRecentFive();
        int GetNotificationsCount(NotificationStatus notificationStatus);
        List<Notification> GetAllNonReadNotifications();
        List<Notification> GetNotificationsByCategory(NotificationCategory notificationCategory);
        List<Notification> GetAllReOccurringNotifications();
        Notification GetNotificationById(int id);
        Task<bool> MarkAsRead(int id);
        Task MarkAllAsRead();
    }
}