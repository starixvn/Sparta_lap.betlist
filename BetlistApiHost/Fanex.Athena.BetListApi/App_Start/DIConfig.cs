using System;
using System.Linq;
using System.Reflection;
using System.Web.Compilation;
using System.Web.Http;
using System.Web.Mvc;
using Autofac;
using Autofac.Integration.Mvc;
using Autofac.Integration.WebApi;
using Fanex.Athena.Common.Utility.Services.Internal;
using Fanex.BetList.Core.Builder;
using Fanex.BetList.Core.Entities;
using Fanex.BetList.Core.TicketHelper;
using Fanex.Logging;
using Fanex.Result.Core.AllBetCasino;
using Fanex.Result.Core.MicroGaming;
using Fanex.Result.Core.PP;
using Fanex.Result.Core.Voidbridge;

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
            var apiControllerAssemblies = GetControllerAssemblies(assemblies);
            builder.RegisterAssemblyTypes(serviceAssemblies).AsImplementedInterfaces();
            builder.RegisterControllers(apiControllerAssemblies);

            builder.RegisterFilterProvider();

            RegisterThirdpartyServices(builder);

            // static services
            builder.RegisterInstance(new CustomMemoryCache()).As<ICustomMemoryCache>();

            var container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
            config.DependencyResolver = new AutofacWebApiDependencyResolver(container);
        }

        private static void RegisterThirdpartyServices(ContainerBuilder builder)
        {
            builder.RegisterType<TicketHelper>().As<ITicketHelper>().InstancePerRequest();
            builder.RegisterType<BetListHTMLBuilder>().As<IBetListHTMLBuilder>().InstancePerRequest();
            builder.RegisterType<BetListExcelBuilder>().As<IBetListExcelBuilder>().InstancePerDependency();
            builder.RegisterType<PPResult>().As<IPPResult>();
            builder.RegisterType<VoidbridgeApiImp>().As<VoidbridgeApi>();
            builder.RegisterType<AllbetResultBuilder>().As<IAllbetResultBuilder>();

            builder.RegisterInstance(ParserService.Instance).As<IParserService>();
            builder.RegisterInstance(Logger.Log).As<Logging.ILogger>();
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

            return assemblies.Where(x => x.FullName.StartsWith("Fanex.Athena", StringComparison.InvariantCulture)).ToArray();
        }

        private static Assembly[] GetServiceAssemblies(Assembly[] assemblies)
        {
            string[] controllerFileNames = new[] { "Fanex.Athena.BetList.Controller", "Fanex.Athena.Result.Controller" };
            return assemblies.Where(ass => !controllerFileNames.Any(x => ass.FullName.Contains(x))).ToArray();
        }

        private static Assembly[] GetControllerAssemblies(Assembly[] assemblies)
        {
            return assemblies.Where(ass =>
                ass.FullName.Contains("Fanex.Athena.BetList.Controller") ||
                ass.FullName.Contains("Fanex.Athena.Result.Controller")).ToArray();
        }
    }
}