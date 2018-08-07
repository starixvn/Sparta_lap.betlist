// --------------------------------------------------------------------------
// <copyright file="Global.asax.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace Fanex.Athena
{
    using System;
    using System.Configuration;
    using System.Diagnostics.CodeAnalysis;
    using System.Web;
    using System.Web.Helpers;
    using System.Web.Http;
    using System.Web.Http.ExceptionHandling;
    using System.Web.Mvc;
    using System.Web.Optimization;
    using System.Web.Routing;
    using Api.Base.Attributes;
    using Api.Base.Logger;
    using Data.Configuration;
    using Fanex.Athena.Web.ActionFilter;
    using Fanex.Athena.Web.RouterConfig;
    using Fanex.RemotingRef;
    using Service.Core.Utility;

    public class MvcApplication : HttpApplication
    {
        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Justification = "Reviewed.")]
        public void Application_Error(object sender, EventArgs e)
        {
            var deploymentMode = ConfigurationManager.AppSettings["DeploymentMode"];
            if (string.IsNullOrEmpty(deploymentMode) || !deploymentMode.Equals("Static"))
            {
                // Code that runs when an unhandled error occurs
                var exception = Server.GetLastError();

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

                var logger = new CoreLogger();
                logger.Error(exception);
            }
        }

        [SuppressMessage("Sonar Code Smell", "S2325:Methods and properties that don't access instance data should be static", Justification = "Reviewed.")]
        public void Application_Start(object sender, EventArgs e)
        {
            var deploymentMode = ConfigurationManager.AppSettings["DeploymentMode"];
            if (string.IsNullOrEmpty(deploymentMode) || !deploymentMode.Equals("Static"))
            {
                ViewEngines.Engines.Clear();
                ViewEngines.Engines.Add(new RazorViewEngine());

                AreaRegistration.RegisterAllAreas();

                GlobalFilters.Filters.Add(new CustomAuthorizeAttribute());
                RouteTable.Routes.IgnoreRoute("favicon.ico");
                RouteConfig.RegisterRoutes(RouteTable.Routes);

                BundleConfig.RegisterBundles(BundleTable.Bundles);
                /*Web API*/
                GlobalConfiguration.Configuration.EnableCors();
                GlobalConfiguration.Configuration.Filters.Add(new CustomApiAuthorizeAttribute());
                GlobalConfiguration.Configuration.Formatters.Remove(GlobalConfiguration.Configuration.Formatters.XmlFormatter);
                GlobalConfiguration.Configuration.Services.Add(typeof(IExceptionLogger), new ApiExceptionLogger());
                /*End Web API*/

                AntiForgeryConfig.SuppressXFrameOptionsHeader = true;

                DbSettingProviderManager.Current.Start();
                log4net.Config.XmlConfigurator.Configure();
                log4net.LogManager.GetLogger("GeneralLog").Info("Start");
                RefDataTypeContainer
               .ConnectTo(new Uri(ConfigurationManager.AppSettings["Fanex.RemotingRef.Service.Url"]))
               .FromApplication("Fanex.LAP");

            }
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