// --------------------------------------------------------------------------
// <copyright file="Global.asax.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace Fanex.Athena
{
    using System;
    using System.Web;
    using System.Web.Http;
    using System.Web.Mvc;
    using System.Web.Routing;
    using CasinoSetting;
    using Data.Configuration;
    using Fanex.Athena.Extension.Utility;
    using Fanex.Athena.Service.Implement.Users;
    using Fanex.Athena.Service.Interface.Users;
    using Fanex.Athena.Web.ActionFilter;
    using Fanex.Athena.Web.RouterConfig;

    // Note: For instructions on enabling IIS6 or IIS7 classic mode,
    // visit http://go.microsoft.com/?LinkId=9394801

    /// <summary>
    /// Global class.
    /// </summary>
    public class MvcApplication : HttpApplication
    {
        /// <summary>
        /// Application start.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The event ARGS.</param>
        public void Application_Start(object sender, EventArgs e)
        {
            AreaRegistration.RegisterAllAreas();

            GlobalFilters.Filters.Add(new CustomAuthorizeAttribute());
            RouteTable.Routes.IgnoreRoute("favicon.ico");
            RouteConfig.RegisterRoutes(RouteTable.Routes);            

            /*BundleConfig.RegisterBundles(BundleTable.Bundles);*/
            /*Web API*/
            GlobalConfiguration.Configuration.MessageHandlers.Add(new CompressHandler());
            GlobalConfiguration.Configuration.Filters.Add(new CustomApiAuthorizeAttribute());
            GlobalConfiguration.Configuration.Formatters.Remove(GlobalConfiguration.Configuration.Formatters.XmlFormatter);
            GlobalConfiguration.Configuration.Filters.Add(new CustomAPIExceptionHandlingAttribute());
            /*End Web API*/
            DbSettingProviderManager.Current.Start();
            log4net.Config.XmlConfigurator.Configure();
            log4net.LogManager.GetLogger("GeneralLog").Info("Start");

            if (!AppSettings.LocalTest)
            {
                SettingMgr.GetInstance().Initialize(SiteEnum.AgentSite);
            }
        }

        /// <summary>
        /// Code that runs on application shutdown.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The event ARGS.</param>
        public void Application_End(object sender, EventArgs e)
        {
        }

        /// <summary>
        /// The application errors.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The event ARGS.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Justification = "Reviewed.")]
        public void Application_Error(object sender, EventArgs e)
        {
            // Code that runs when an unhandled error occurs
            Exception ex = Server.GetLastError().GetBaseException();
            var httpException = ex as HttpException;
            if (httpException != null && httpException.GetHttpCode() == 404)
            {
                return;
            }

            string message = ex.Message +
                             "\r\nSOURCE: " + ex.Source +
                             "\r\nDOMAIN: " + HttpContext.Current.Request.Headers["Host"] +
                             "\r\nFORM: " + string.Join("~", Request.Form) +
                             "\r\nUrl: " + Request.RawUrl +
                             "\r\nQUERYSTRING: " + string.Join("~", Request.QueryString) +
                             "\r\nTARGETSITE: " + ex.TargetSite +
                             "\r\nSTACKTRACE: " + ex.StackTrace +
                             "\r\nBrowser:" + Request.Browser.Type.ToString() +
                             "\r\nBrowserVer:" + Request.Browser.Version.ToString() +
                             "\r\nAgent:" + Request.UserAgent.ToString();

            ISessionService sessionService = new SessionService();
            var licenseeSession = sessionService.LicenseeSession;
            if (licenseeSession != null)
            {
                message += "\r\nSystemInfo:Licensee=" + licenseeSession.LicenseeId + "-" + licenseeSession.LicenseeName + ";userId=" + licenseeSession.UserId;
            }

            Logging.LogService.Error(message, ex);
        }

        protected void Application_BeginRequest()
        {
        }

        /// <summary>
        /// End request.
        /// </summary>
        protected void Application_EndRequest()
        {
            /*stop as early as you can, even earlier with MvcMiniProfiler.MiniProfiler.Stop(discardResults: true);*/
            if (!AppSettings.Enviroment.Equals("DEV", StringComparison.OrdinalIgnoreCase))
            {
                Response.Headers.Remove("X-AspNet-Version");
                Response.Headers.Remove("X-AspNetMvc-Version");
                Response.Headers.Remove("Server");
                Response.Headers.Remove("X-Powered-By");
            }
        }
    }
}