// --------------------------------------------------------------------------
// <copyright file="Global.asax.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace Fanex.Athena
{
    using System;
    using System.Collections.Generic;
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
    using Logging;
    using Logging.Extensions.Database;
    using Logging.Extensions.Web;
    using Logging.RabbitMQ;

    public class MvcApplication : HttpApplication
    {
        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Justification = "Reviewed.")]
        public void Application_Error(object sender, EventArgs e)
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

        [SuppressMessage("Sonar Code Smell", "S2325:Methods and properties that don't access instance data should be static", Justification = "Reviewed.")]
        public void Application_Start(object sender, EventArgs e)
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
            RefDataTypeContainer
           .ConnectTo(new Uri(ConfigurationManager.AppSettings["Fanex.RemotingRef.Service.Url"]))
           .FromApplication("Fanex.LAP.BetListApi");
            BetListManager.RegisterRemotingRef(new CacheRef());
            ResultCoreManager.RegisterRefDataHelper(new RefDataHelper());

            LogManager
                .SetDefaultLogCategory(ConfigurationManager.AppSettings["Fanex.Logging-LogCategory"])
                .Use(new List<ILogEngine>
                 {
                     new RabbitMQLogging(ConfigurationManager.AppSettings["RabbitMQ-ConnectionString"])
                 });

            Logging.Extensions.Web.ExceptionHandler.UseConfig(new LoggingConfiguration())
                .IgnoreException<HttpAntiForgeryException>()
                .GetHttpHeaderInfo()
                .UseGetDbExceptionInfo(DatabaseInfoGetters.GetDbExceptionInfo);
            DIConfig.Register();
        }
    }
}