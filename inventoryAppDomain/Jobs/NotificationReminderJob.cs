using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.ModelBinding;
using inventoryAppDomain.Entities;
using inventoryAppDomain.Entities.Enums;
using inventoryAppDomain.ExtensionMethods;
using inventoryAppDomain.IdentityEntities;
using inventoryAppDomain.Repository;
using inventoryAppDomain.Services;
using Microsoft.AspNet.Identity.Owin;

namespace inventoryAppDomain.Jobs
{
    public class NotificationReminderJob: IDisposable
    {
        private static ApplicationDbContext _dbContext;
        

        public NotificationReminderJob()
        {
            _dbContext = new ApplicationDbContext();
        }

        public void RunReminder(TimeFrame timeFrame)
        {
            switch (timeFrame)
            {
                case TimeFrame.WEEKLY:
                {
                    var beginningOfWeek = DateTime.Now.FirstDayOfWeek();
                    var endOfWeek = DateTime.Now.LastDayOfWeek();

                    Func<Product,bool> func = Product => DateTime.Now.Month == Product.ExpiryDate.Month
                                       && DateTime.Now.Year == Product.ExpiryDate.Year;

                    var Products = _dbContext.Products.Where(func)
                        .Where(Product => Product.ExpiryDate >= beginningOfWeek && Product.ExpiryDate <= endOfWeek).ToList();
                    Products.ForEach(Product =>
                    {
                        var notification = new Notification()
                        {
                            Title = "Product Expiration",
                            NotificationDetails = $"{Product.ProductName} is Expiring this Week.",
                            NotificationType = NotificationType.REOCCURRING,
                            NotificationCategory = NotificationCategory.EXPIRATION
                        };
                        _dbContext.Notifications.Add(notification);
                    }); 
                    _dbContext.SaveChanges();
                    break;
                }
                case TimeFrame.MONTHLY:
                {
                    Func<Product, bool> func = Product => DateTime.Now.Month.Equals(Product.ExpiryDate.Month)
                                                    && DateTime.Now.Year.Equals(Product.ExpiryDate.Year);
                    var Products = _dbContext.Products.Where(func).ToList();
                    Products.ForEach(Product =>
                    {
                        var notification = new Notification()
                        {
                            Title = "Product Expiration",
                            NotificationDetails = $"{Product.ProductName} is Expiring this Month.",
                            NotificationType = NotificationType.REOCCURRING,
                            NotificationCategory = NotificationCategory.EXPIRATION
                        };
                        _dbContext.Notifications.Add(notification);
                    });
                    _dbContext.SaveChanges();
                    break;
                }
            }
        }

        public void OutOfStockReminders()
        {
            var Products = _dbContext.Products.Where(Product => Product.Quantity <= 20).ToList();
            
            Products.ForEach(Product =>
            {
                var notification = new Notification()
                {
                    Title = "Running Out Stock",
                    NotificationDetails = $"{Product.ProductName} is Running Out.",
                    NotificationType = NotificationType.REOCCURRING,
                    NotificationCategory = NotificationCategory.RUNNING_OUT_OF_STOCK
                };
                _dbContext.Notifications.Add(notification);
            });
            _dbContext.SaveChanges();
        }

        public void ExpireProducts()
        {
            var Products = _dbContext.Products.Where(Product => Product.ExpiryDate.Equals(DateTime.Today)).ToList();
            
            Products.ForEach(Product =>
            {
                Product.CurrentProductStatus = ProductStatus.EXPIRED;
                _dbContext.Entry(Product).State = EntityState.Modified;
            });

            _dbContext.SaveChanges();
        }
        public void Dispose()
        {
            _dbContext.Dispose();
        }
    }
}