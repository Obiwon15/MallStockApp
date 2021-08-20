using System;
using System.Collections.Generic;
using System.Configuration;
using System.Web.Configuration;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using AutoMapper;
using Hangfire;
using inventoryAppDomain.Entities.Enums;
using inventoryAppDomain.Infrastructure;
using inventoryAppDomain.Jobs;
using inventoryAppWebUi.Infrastructures;

namespace inventoryAppWebUi
{
    public class MvcApplication : System.Web.HttpApplication
    {
        private IEnumerable<IDisposable> GetHangfireServers()
        {
            GlobalConfiguration.Configuration
                .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                .UseSimpleAssemblyNameTypeSerializer()
                .UseRecommendedSerializerSettings()
                .UseSqlServerStorage(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);

            yield return new BackgroundJobServer();
        }

        
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            Mapper.Initialize(configuration =>
            {
                configuration.AddProfile<MappingProfile>(); 
                configuration.AddProfile<DomainMappingProfile>();
            });
            HangfireAspNet.Use(GetHangfireServers);

            NotificationReminderJob notificationReminder = new NotificationReminderJob();

            try
            {
                RecurringJob.AddOrUpdate(() => notificationReminder.RunReminder(TimeFrame.WEEKLY), Cron.Daily);
                RecurringJob.AddOrUpdate(() => notificationReminder.RunReminder(TimeFrame.MONTHLY), Cron.Daily);
                RecurringJob.AddOrUpdate(() => notificationReminder.OutOfStockReminders(), Cron.Hourly);
                RecurringJob.AddOrUpdate(() => notificationReminder.ExpireProducts(), Cron.Daily);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
