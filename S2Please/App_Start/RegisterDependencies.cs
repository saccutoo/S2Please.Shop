﻿using System.Web.Mvc;
using Autofac;
using Autofac.Integration.Mvc;
using Repository;
using S2Please.Jobs;
namespace S2Please
{
    public static class RegisterDependencies
    {
        private static IContainer Container { get; set; }

        public static void Register()
        {
            var builder = new ContainerBuilder();
            //repositories
            builder.RegisterType<CommonRepository>().InstancePerLifetimeScope();
            builder.RegisterType<ProductRepository>().As<IProductRepository>().InstancePerLifetimeScope();
            builder.RegisterType<TableRepository>().As<ITableRepository>().InstancePerLifetimeScope();
            builder.RegisterType<OrderRepository>().As<IOrderRepository>().InstancePerLifetimeScope();
            builder.RegisterType<SystemRepository>().As<ISystemRepository>().InstancePerLifetimeScope();
            builder.RegisterType<AuthenRepository>().As<IAuthenRepository>().InstancePerLifetimeScope();
            builder.RegisterType<DashboardRepository>().As<IDashboardRepository>().InstancePerLifetimeScope();
            builder.RegisterType<CustomerRepository>().As<ICustomerRepository>().InstancePerLifetimeScope();
            builder.RegisterType<MailQueueRepository>().As<IMailQueueRepository>().InstancePerLifetimeScope();
            builder.RegisterType<ProductTypeRepository>().As<IProductTypeRepository>().InstancePerLifetimeScope();
            builder.RegisterType<MenuRepository>().As<IMenuRepository>().InstancePerLifetimeScope();
            builder.RegisterType<ProductGroupRepository>().As<IProductGroupRepository>().InstancePerLifetimeScope();
            builder.RegisterType<MenuAdminRepository>().As<IMenuAdminRepository>().InstancePerLifetimeScope();
            builder.RegisterType<PermissionRepository>().As<IPermissonRepository>().InstancePerLifetimeScope();
            builder.RegisterType<UserRepository>().As<IUserRepository>().InstancePerLifetimeScope();
            builder.RegisterType<MessengerRepository>().As<IMessengerRepository>().InstancePerLifetimeScope();

            //controller base
            builder.RegisterType<S2Please.Controllers.BaseController>();

            //controllers admin
            builder.RegisterType<S2Please.Areas.ADMIN.Controllers.ProductController>();
            builder.RegisterType<S2Please.Areas.ADMIN.Controllers.OrderController>();
            builder.RegisterType<S2Please.Areas.ADMIN.Controllers.AuthenController>();
            builder.RegisterType<S2Please.Areas.ADMIN.Controllers.DashboardController>();
            builder.RegisterType<S2Please.Areas.ADMIN.Controllers.ExecController>();
            builder.RegisterType<S2Please.Areas.ADMIN.Controllers.FilterController>();
            builder.RegisterType<S2Please.Areas.ADMIN.Controllers.SystemController>();
            builder.RegisterType<S2Please.Areas.ADMIN.Controllers.TestController>();
            builder.RegisterType<S2Please.Areas.ADMIN.Controllers.ProductTypeController>();
            builder.RegisterType<S2Please.Areas.ADMIN.Controllers.ProductGroupController>();
            builder.RegisterType<S2Please.Areas.ADMIN.Controllers.MenuController>();
            builder.RegisterType<S2Please.Areas.ADMIN.Controllers.MailQueueController>();
            builder.RegisterType<S2Please.Areas.ADMIN.Controllers.MenuAdminController>();
            builder.RegisterType<S2Please.Areas.ADMIN.Controllers.PermissionController>();
            builder.RegisterType<S2Please.Areas.ADMIN.Controllers.NotificationController>();

            //controllers web
            builder.RegisterType<S2Please.Areas.WEB_SHOP.Controllers.AuthenController>();
            builder.RegisterType<S2Please.Areas.WEB_SHOP.Controllers.CartController>();
            builder.RegisterType<S2Please.Areas.WEB_SHOP.Controllers.HomeController>();
            builder.RegisterType<S2Please.Areas.WEB_SHOP.Controllers.MenuController>();
            builder.RegisterType<S2Please.Areas.WEB_SHOP.Controllers.ProductController>();
            builder.RegisterType<S2Please.Areas.WEB_SHOP.Controllers.SearchController>();
            builder.RegisterType<S2Please.Areas.WEB_SHOP.Controllers.UserController>();

            //SignalR
            //builder.RegisterType<S2Please.SignalR.MessengerHub>();

            //build all
            Container = builder.Build();

            //set dependency resolver
            DependencyResolver.SetResolver(new AutofacDependencyResolver(Container));
        }
    }
}