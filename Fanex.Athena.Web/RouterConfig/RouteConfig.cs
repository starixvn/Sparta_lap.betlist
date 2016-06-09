// -----------------------------------------------------------------------
// <copyright file="RouteConfig.cs" company="Nexcel">
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
        /// <summary>
        /// Register routes.
        /// </summary>
        /// <param name="routes">The route collection.</param>
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.IgnoreRoute("content/{*pathInfo}");
            routes.IgnoreRoute("images/{*pathInfo}");
            routes.IgnoreRoute("scripts/{*pathInfo}");
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