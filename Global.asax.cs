using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace WebApplication1
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            DependencyConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            // Initialize database and seed data
            var resolver = GlobalConfiguration.Configuration.DependencyResolver;
            using (var scope = resolver.BeginScope())
            {
                var context = (DesktopShop.Infrastructure.Data.ApplicationDbContext)
                    scope.GetService(typeof(DesktopShop.Infrastructure.Data.ApplicationDbContext));
                DesktopShop.Infrastructure.Identity.IdentitySeed.SeedAsync(context).Wait();
            }
        }
    }
}
