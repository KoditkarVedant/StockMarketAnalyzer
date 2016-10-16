using Autofac;
using Autofac.Integration.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Compilation;
using System.Web.Mvc;

namespace StockMarketAnalyzer.App_Start
{
    public class DependencyConfig
    {
        public static void Configuration(ContainerBuilder builder)
        {
            builder.RegisterControllers(typeof(MvcApplication).Assembly).PropertiesAutowired();

            var assemblies = BuildManager.GetReferencedAssemblies().Cast<Assembly>().ToList();

            foreach (var assembly in assemblies)
            {
                if (assembly.FullName.ToLower().Contains("system") ||
                  assembly.FullName.ToLower().Contains("autofac")) continue;

                builder.RegisterAssemblyModules(assembly);
            }

            var container = builder.Build();
            var dependencyResolver = new AutofacDependencyResolver(container);
            DependencyResolver.SetResolver(dependencyResolver);
        }
    }
}