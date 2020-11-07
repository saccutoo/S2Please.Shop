using System;
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

            //controllers vs repositories
            builder.RegisterType<S2Please.Areas.ADMIN.Controllers.ProductController>();
            Container = builder.Build();
            //set dependency resolver
            DependencyResolver.SetResolver(new AutofacDependencyResolver(Container));
        }
    }
}