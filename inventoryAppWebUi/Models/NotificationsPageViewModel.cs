using System.Collections.Generic;
using inventoryAppDomain.Entities;

namespace inventoryAppWebUi.Models
{
    public class NotificationsPageViewModel
    {
        public int NotificationsCount { get; set; }
        public List<Notification> UnreadNotifications { get; set; }
        public List<Notification> ReadNotifications { get; set; }
        public List<Notification> AllNotifications { get; set; }
        
    }
}