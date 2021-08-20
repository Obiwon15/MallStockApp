using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using inventoryAppDomain.Entities.Enums;

namespace inventoryAppDomain.Entities
{
    public class Notification
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Title { get; set; }
        public string NotificationDetails { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public NotificationStatus NotificationStatus { get; set; } = NotificationStatus.UN_READ;
        public NotificationCategory NotificationCategory { get; set; }
        public NotificationType NotificationType { get; set; } = NotificationType.NONREOCCURRING;

    }
}