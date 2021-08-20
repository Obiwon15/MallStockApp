using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using inventoryAppDomain.Entities;
using inventoryAppDomain.Entities.Enums;
using inventoryAppDomain.IdentityEntities;
using inventoryAppDomain.Services;
using Microsoft.AspNet.Identity.Owin;

namespace inventoryAppDomain.Repository
{
    public class NotificationService : INotificationService
    {
        private readonly ApplicationDbContext _dbContext;

        public NotificationService()
        {
            _dbContext = HttpContext.Current.GetOwinContext().Get<ApplicationDbContext>();
        }

        public async Task<Notification> CreateNotification(string title, string details, NotificationType notificationType, NotificationCategory notificationCategory)
        {
            Notification notification = null;
            switch (notificationType)
            {
                case NotificationType.NONREOCCURRING:
                {
                    notification = new Notification
                    {
                        Title = title,
                        NotificationDetails = details,
                        NotificationType = notificationType,
                        NotificationCategory = notificationCategory
                    };
                    _dbContext.Notifications.Add(notification);
                    break;
                }
                case NotificationType.REOCCURRING:
                {
                    notification = await _dbContext.Notifications.FirstOrDefaultAsync(notification1 =>
                        notification1.NotificationDetails.Equals(details));

                    if (notification == null)
                    {
                        notification = new Notification
                        {
                            Title = title,
                            NotificationDetails = details,
                            NotificationType = notificationType,
                            NotificationCategory = notificationCategory
                        };
                        _dbContext.Notifications.Add(notification);
                    }
                    break;
                }
            }
            await _dbContext.SaveChangesAsync();
            return notification;
        }

        public List<Notification> GetAllNotifications()
        {
            return _dbContext.Notifications.ToList()                
                .OrderBy(n => n.NotificationStatus == NotificationStatus.READ).ToList();
        }

        public List<Notification> GetRecentFive()
        {
            return GetAllNotifications().Where(notification => notification.NotificationStatus == NotificationStatus.UN_READ).OrderByDescending(notification => notification.Id).Take(5).ToList();
        }

        public int GetNotificationsCount(NotificationStatus notificationStatus)
        {
            return GetAllNotifications().Count(notification => notification.NotificationStatus == notificationStatus);
        }

        public List<Notification> GetAllNonReadNotifications()
        {
            return GetAllNotifications()
                .Where(notification => notification.NotificationStatus == NotificationStatus.UN_READ).ToList();
        }

        public List<Notification> GetNotificationsByCategory(NotificationCategory notificationCategory)
        {
            return GetAllNotifications()
                .Where(notification => notification.NotificationCategory == notificationCategory).ToList();
        }
        public List<Notification> GetAllReOccurringNotifications()
        {
            return _dbContext.Notifications
                .Where(notification => notification.NotificationType == NotificationType.REOCCURRING).ToList();
        }

        public Notification GetNotificationById(int id)
        {
            return GetAllNotifications().FirstOrDefault(notification => notification.Id == id);
        }
        
        public async Task<bool> MarkAsRead(int id)
        {
            var currentNotification = GetAllNonReadNotifications().FirstOrDefault(notification => notification.Id == id);
            if (currentNotification != null)
            {
                currentNotification.NotificationStatus = NotificationStatus.READ;
                _dbContext.Entry(currentNotification).State = EntityState.Modified;
                await _dbContext.SaveChangesAsync();
                return true;
            }
            throw new Exception("Notification Not Found");
        }

        public async Task MarkAllAsRead()
        {
            GetAllNonReadNotifications()
                .ForEach(notification =>
                {
                    notification.NotificationStatus = NotificationStatus.READ;
                    _dbContext.Entry(notification).State = EntityState.Modified;
                });
            await _dbContext.SaveChangesAsync();
        }
    }
}