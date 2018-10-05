using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using Autofac;
using Autofac.Integration.WebApi;

namespace Fanex.Athena.UnitTest.Common.Helpers
{
    public static class WebApiHelper
    {
        public static void SetupResolver(ContainerBuilder builder, HttpActionContext actionContext)
        {
            var config = CreateHttpConfig(builder);
            actionContext.Request.SetConfiguration(config);
        }

        public static void SetupResolver(ContainerBuilder builder, HttpActionExecutedContext actionContext)
        {
            var config = CreateHttpConfig(builder);
            actionContext.Request.SetConfiguration(config);
        }

        private static HttpConfiguration CreateHttpConfig(ContainerBuilder builder)
        {
            var container = builder.Build();

            var resolver = new AutofacWebApiDependencyResolver(container);
            var config = new HttpConfiguration();
            config.DependencyResolver = resolver;
            config.EnsureInitialized();
            return config;
        }
    }
}