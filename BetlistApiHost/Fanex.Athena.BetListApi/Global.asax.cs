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
    using System.Web.Http;
    using System.Web.Mvc;
    using System.Web.Routing;
    using App_Start;
    using Data;
    using Fanex.BetList.Core;
    using Fanex.BetList.Core.RemotingRef;
    using Fanex.RemotingRef;
    using Fanex.Result.Core.Base;
    using Fanex.Result.Core.RemotingRef;
    using Logging.Extensions.Database;
    using Logging.Extensions.Web;

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

                // Exception when passing wrong data type to action method
                if (exception.TargetSite.DeclaringType == typeof(ActionDescriptor) &&
                     exception.TargetSite.Name == "ExtractParameterFromDictionary")
                {
                    return;
                }

                ExceptionHandler.Handle(exception);
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

                RouteConfig.RegisterRoutes(RouteTable.Routes);

                GlobalConfiguration.Configuration.Formatters.Remove(GlobalConfiguration.Configuration.Formatters.XmlFormatter);

                DbSettingProviderManager
                    .StartNewSession()
                    .UseDefaultConnectionStringProvider()
                    .UseDefaultDbSettingProvider(resourcePath: "~/SpNameConfig/FanexSpName.xml")
                    .Run();
                log4net.Config.XmlConfigurator.Configure();
                log4net.LogManager.GetLogger("GeneralLog").Info("Start");
                RefDataTypeContainer
               .ConnectTo(new Uri(ConfigurationManager.AppSettings["Fanex.RemotingRef.Service.Url"]))
               .FromApplication("Fanex.LAP");
                BetListManager.RegisterRemotingRef(new CacheRef());
                ResultCoreManager.RegisterRefDataHelper(new RefDataHelper());

                Logging.Extensions.Web.ExceptionHandler.UseConfig(new LoggingConfiguration())
                    .IgnoreException<HttpAntiForgeryException>()
                    .GetHttpHeaderInfo()
                    .UseGetDbExceptionInfo(DatabaseInfoGetters.GetDbExceptionInfo);
                DIConfig.Register();
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