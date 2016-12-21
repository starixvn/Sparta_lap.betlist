// --------------------------------------------------------------------------
// <copyright file="Global.asax.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace Fanex.Athena
{
    using System;
    using System.Configuration;
    using System.Web;
    using System.Web.Helpers;
    using System.Web.Http;
    using System.Web.Http.ExceptionHandling;
    using System.Web.Mvc;
    using System.Web.Optimization;
    using System.Web.Routing;
    using Api.Base.Attributes;
    using Api.Base.Logger;
    using CasinoSetting;
    using Data.Configuration;
    using Fanex.Athena.Extension.Utility;
    using Fanex.Athena.Service.Core.Helpers;
    using Fanex.Athena.Web.ActionFilter;
    using Fanex.Athena.Web.RouterConfig;

    public class MvcApplication : HttpApplication
    {
        public void Application_Start(object sender, EventArgs e)
        {
            ViewEngines.Engines.Clear();
            ViewEngines.Engines.Add(new RazorViewEngine());

            AreaRegistration.RegisterAllAreas();

            GlobalFilters.Filters.Add(new CustomAuthorizeAttribute());
            RouteTable.Routes.IgnoreRoute("favicon.ico");
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            BundleConfig.RegisterBundles(BundleTable.Bundles);
            /*BundleConfig.RegisterBundles(BundleTable.Bundles);*/
            /*Web API*/
            using (var compressHandler = new CompressHandler())
            {
                GlobalConfiguration.Configuration.MessageHandlers.Add(compressHandler);
            }
            GlobalConfiguration.Configuration.Filters.Add(new CustomApiAuthorizeAttribute());
            GlobalConfiguration.Configuration.Formatters.Remove(GlobalConfiguration.Configuration.Formatters.XmlFormatter);
            GlobalConfiguration.Configuration.Services.Add(typeof(IExceptionLogger), new ApiExceptionLogger());
            /*End Web API*/

            AntiForgeryConfig.SuppressXFrameOptionsHeader = true;

            DbSettingProviderManager.Current.Start();
            log4net.Config.XmlConfigurator.Configure();
            log4net.LogManager.GetLogger("GeneralLog").Info("Start");

            if (ConfigurationManager.AppSettings["EnableCasinoService"] == "1")
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
            var exception = Server.GetLastError().GetBaseException();
            if (exception is HttpAntiForgeryException)
            {
                return;
            }

            var httpException = exception as HttpException;
            if (httpException != null && httpException.GetHttpCode() == 404)
            {
                return;
            }

            // Exception when passing wrong data type to action method
            if (exception.TargetSite.DeclaringType == typeof(ActionDescriptor) &&
                exception.TargetSite.Name == "ExtractParameterFromDictionary")
            {
                return;
            }

            LogHelper.WriteError(exception);
        }

        /// <summary>
        /// End request.
        /// </summary>
        protected void Application_EndRequest()
        {
            Response.Headers.Remove("X-AspNet-Version");
            Response.Headers.Remove("X-AspNetMvc-Version");
            Response.Headers.Remove("Server");
            Response.Headers.Remove("X-Powered-By");
        }
    }
}