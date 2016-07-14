// --------------------------------------------------------------------------
// <copyright file="ReadonlySessionRouteHandler.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Dave.Bui" date="2012.4.11">Created</change>
// </history>
// ----------------------------------------------------------------------

namespace Fanex.Athena.Web.RouterConfig
{
    using System.Web;
    using System.Web.Http.WebHost;
    using System.Web.Routing;
    using System.Web.SessionState;

    /// <summary>
    /// ReadonlySessionRouteHandler class.
    /// </summary>
    public class ReadonlySessionRouteHandler : HttpControllerRouteHandler
    {
        protected override IHttpHandler GetHttpHandler(RequestContext requestContext)
        {
            requestContext.HttpContext.SetSessionStateBehavior(SessionStateBehavior.ReadOnly);
            return new HttpControllerHandler(requestContext.RouteData);
        }
    }
}