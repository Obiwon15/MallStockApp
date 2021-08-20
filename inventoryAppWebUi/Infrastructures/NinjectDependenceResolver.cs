using inventoryAppWebUi.Models;
using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using inventoryAppDomain.IdentityEntities;
using inventoryAppDomain.Jobs;
using inventoryAppDomain.Repository;
using inventoryAppDomain.Services;
using Ninject.Web.Common;
using Microsoft.AspNetCore.Http;
using inventoryAppDomain.Entities;
using inventoryAppDomain.Infrastructure;

namespace inventoryAppWebUi.Infrastructures
{
    public class NinjectDependenceResolver : IDependencyResolver
    {
        private IKernel kernel;
        public NinjectDependenceResolver(IKernel kernelParam)
        {
            kernel = kernelParam;
            AddBindings();
        }

        public object GetService(Type serviceType)
        {
            return kernel.TryGet(serviceType);
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return kernel.GetAll(serviceType);
        }

        private void AddBindings()
        {
            kernel.Bind<IRoleService>().To<RoleService>();
            kernel.Bind<IProfileService>().To<ProfileService>();
            kernel.Bind<INotificationService>().To<NotificationService>();
            kernel.Bind<ISupplierService>().To<SupplierService>();
            kernel.Bind<IProductService>().To<ProductService>();
            kernel.Bind<IOrderService>().To<OrderService>();
            kernel.Bind<IProductCartService>().To<ProductCartService>();
            kernel.Bind<IReportService>().To<ReportService>();
            kernel.Bind<NotificationReminderJob>().ToSelf();
            kernel.Bind<ReportPdfGenerator>().ToSelf();
            kernel.Bind<IPaymentService>().To<MonnifyPaymentService>();
            kernel.Bind<ITransactionService>().To<TransactionService>();
        }
    }
}