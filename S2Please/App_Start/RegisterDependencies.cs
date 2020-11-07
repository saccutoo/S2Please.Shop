﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Autofac;
using Autofac.Integration.Mvc;
using Repository;

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

            //controller base
            builder.RegisterType<S2Please.Controllers.BaseController>();

            //controllers admin
            builder.RegisterType<S2Please.Areas.ADMIN.Controllers.ProductController>();
            builder.RegisterType<S2Please.Areas.ADMIN.Controllers.OrderController>();
            builder.RegisterType<S2Please.Areas.ADMIN.Controllers.AuthenController>();
            builder.RegisterType<S2Please.Areas.ADMIN.Controllers.DashboardController>();
            builder.RegisterType<S2Please.Areas.ADMIN.Controllers.ExecController>();
            builder.RegisterType<S2Please.Areas.ADMIN.Controllers.FilterController>();

            //build all
            Container = builder.Build();

            //set dependency resolver
            DependencyResolver.SetResolver(new AutofacDependencyResolver(Container));
        }
    }
}