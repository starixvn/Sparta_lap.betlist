// -----------------------------------------------------------------------
// <copyright file="RouteConfig.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Web.RouterConfig
{
    using System.Web.Http;
    using System.Web.Mvc;
    using System.Web.Routing;

    /// <summary>
    /// Route config class.
    /// </summary>
    public static class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes, bool includeAttributeMapping = true)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.IgnoreRoute("content/{*pathInfo}");
            routes.IgnoreRoute("images/{*pathInfo}");
            routes.IgnoreRoute("scripts/{*pathInfo}");
            if (includeAttributeMapping)
            {
                routes.MapMvcAttributeRoutes();
            }

            var apiRoute = routes.MapHttpRoute(
                    name: "apiRoute",
                    routeTemplate: "api/{controller}/{action}");
            apiRoute.RouteHandler = new ReadonlySessionRouteHandler();

            routes.MapRoute(
                "Default",
                "{controller}/{action}",
                new { controller = "Home", action = "Index" });
            routes.MapPageRoute("sso", "sso.aspx", "~/sso.aspx");
        }
    }
}