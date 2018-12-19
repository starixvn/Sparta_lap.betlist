using System;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Web.Compilation;
using System.Web.Http;
using System.Web.Http.ExceptionHandling;
using System.Web.Mvc;
using Autofac;
using Autofac.Integration.Mvc;
using Autofac.Integration.WebApi;
using CitiXchangeLibrary;
using Fanex.Athena.Api.Base.Logger;
using Fanex.Athena.DataAccess.Report;
using Fanex.Athena.DataAccess.Report.Interfaces;
using Fanex.Athena.Extension.Utility.Internal;
using Fanex.Athena.Model.Report.DTO;
using Fanex.BetList.Core.Builder;
using Fanex.BetList.Core.Entities;
using Fanex.BetList.Core.TicketHelper;
using Fanex.Logging;
using Fanex.Result.Core.MicroGaming;
using Fanex.Result.Core.PP;
using ONEbook.UM.Services;
using ONEbook.UM.Services.Impl;

namespace Fanex.Athena
{
    public static class DIConfig
    {
        public static void Register()
        {
            var builder = new ContainerBuilder();
            var config = GlobalConfiguration.Configuration;
            var assemblies = GetAssemblies();
            var serviceAssemblies = GetServiceAssemblies(assemblies);
            var controllerAssemblies = GetControllerAssemblies(assemblies);
            var apiControllerAssemblies = GetApiControllerAssemblies(assemblies);
            builder.RegisterAssemblyTypes(serviceAssemblies).AsImplementedInterfaces();
            builder.RegisterControllers(controllerAssemblies);
            builder.RegisterApiControllers(apiControllerAssemblies);

            builder.RegisterFilterProvider();
            builder.RegisterWebApiFilterProvider(config);

            RegisterThirdpartyServices(builder);

            // generic types
            builder.RegisterGeneric(typeof(ThirdPartyWinLossDataAccess<>))
                .As(typeof(IThirdPartyWinLossDataAccess<>));

            // static services
            builder.RegisterInstance(new CustomMemoryCache()).As<ICustomMemoryCache>();

            builder.RegisterType<ApiExceptionLogger>().As<IExceptionLogger>();

            // Set the dependency resolver to be Autofac.
            var container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
            config.DependencyResolver = new AutofacWebApiDependencyResolver(container);
        }

        private static void RegisterThirdpartyServices(ContainerBuilder builder)
        {
            builder.RegisterType<MaintenanceService>().As<IMaintenanceService>();
            builder.RegisterType<TicketHelper>().As<ITicketHelper>().InstancePerRequest();
            builder.RegisterType<BetListHTMLBuilder>().As<IBetListHTMLBuilder>().InstancePerRequest();
            builder.RegisterType<BetListExcelBuilder>().As<IBetListExcelBuilder>().InstancePerRequest();
            builder.RegisterType<PPResult>().As<IPPResult>();

            builder.RegisterInstance(ParserService.Instance).As<IParserService>();
            builder.RegisterInstance(Logger.Log).As<Logging.ILogger>();

            builder.Register(x => (ICitiXchangeClient)Activator.GetObject(typeof(ICitiXchangeClient), ConfigurationManager.AppSettings["ExchangeProductApiHost"]))
                .As<ICitiXchangeClient>()
                .ExternallyOwned();
        }

        private static Assembly[] GetAssemblies()
        {
            Assembly[] assemblies = null;
            try
            {
                assemblies = BuildManager.GetReferencedAssemblies().Cast<Assembly>().ToArray();
            }
            catch
            {
                assemblies = AppDomain.CurrentDomain.GetAssemblies().Where(i => !i.IsDynamic).ToArray();
            }

            return assemblies.Where(x => x.FullName.StartsWith("Fanex.Athena", StringComparison.InvariantCulture))
                .ToArray();
        }

        private static Assembly[] GetServiceAssemblies(Assembly[] assemblies)
        {
            string[] controllerFileNames = new[] { "Fanex.Athena.Controllers", "Fanex.Athena.Api" };
            return assemblies.Where(ass => !controllerFileNames.Any(x => ass.FullName.Contains(x))).ToArray();
        }

        private static Assembly[] GetControllerAssemblies(Assembly[] assemblies)
        {
            return assemblies.Where(ass => ass.FullName.Contains("Fanex.Athena.Controllers")).ToArray();
        }

        private static Assembly[] GetApiControllerAssemblies(Assembly[] assemblies)
        {
            return assemblies.Where(ass => ass.FullName.Contains("Fanex.Athena.Api")).ToArray();
        }
    }
}