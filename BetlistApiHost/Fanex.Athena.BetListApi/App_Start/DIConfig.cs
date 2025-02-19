namespace Fanex.Athena
{
    using System;
    using System.Linq;
    using System.Reflection;
    using System.Web.Compilation;
    using System.Web.Mvc;
    using Autofac;
    using Autofac.Features.AttributeFilters;
    using Autofac.Integration.Mvc;
    using BetList.Service;
    using BetList.Service.Helpers;
    using BetList.Service.Interfaces;
    using Common.Utility.Services;
    using Common.Utility.Services.Internal;
    using Fanex.BetList.Core.Builder;
    using Fanex.BetList.Core.Entities;
    using Fanex.BetList.Core.TicketHelper;
    using Fanex.Result.Core.AllBetCasino;
    using Fanex.Result.Core.Base.Utilities;
    using Fanex.Result.Core.Casino;
    using Fanex.Result.Core.ConfigurationHelper;
    using Fanex.Result.Core.LinkedGames;
    using Fanex.Result.Core.SabaClub;
    using Fanex.Result.Core.SAGaming;
    using Fanex.Result.Core.SportsLottery;
    using Fanex.Result.Core.WM;
    using Logging;
    using RemotingRef;
    using Result.Service;
    using Result.Service.Interfaces;

    public static class DIConfig
    {
        public static void Register()
        {
            var builder = new ContainerBuilder();
            var assemblies = GetAssemblies();
            var controllerAss = GetControllerAssemblies(assemblies);
            builder.RegisterAssemblyTypes(assemblies).AsImplementedInterfaces();
            builder.RegisterFilterProvider();

            RegisterThirdpartyServices(builder);

            // static services
            builder.RegisterInstance(new CustomMemoryCache()).As<ICustomMemoryCache>();

            builder.RegisterControllers(controllerAss).WithAttributeFiltering();

            var container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        }

        private static void RegisterThirdpartyServices(ContainerBuilder builder)
        {
            builder.RegisterType<JsonSerializer>().As<ISerializer>();
            builder.RegisterType<ConfigurationHelper>().As<IConfigurationHelper>();
            builder.RegisterType<ShardInfoAccessor>().As<IShardInfoAccessor>().SingleInstance();
            builder.RegisterType<IPService>().As<IIPService>().SingleInstance();
            builder.RegisterType<LogAggregateService>().As<ILogAggregateService>().InstancePerLifetimeScope();
            builder.RegisterType<TicketHelper>().As<ITicketHelper>().InstancePerRequest();
            builder.RegisterType<BetListHTMLBuilder>().As<IBetListHTMLBuilder>().InstancePerRequest();
            builder.RegisterType<BetListExcelBuilder>().As<IBetListExcelBuilder>().InstancePerRequest();
            builder.RegisterType<AllbetResultBuilder>().As<IAllbetResultBuilder>();
            builder.RegisterType<SabaClubResult>().As<ISabaClubResult>();
            builder.RegisterType<RemotingRefService>().As<IRemotingRefService>().SingleInstance();
            builder.RegisterInstance(Logger.Log).As<ILogger>();
            builder.Register(x => new CasinoResult("SpadegamingResultConfigs")).Keyed<ICasinoResult>("SG");
            builder.RegisterType<Indo4DResult>().As<IIndo4DResult>();
            builder.RegisterType<SAGamingResultBuilder>().As<ISAGamingResultBuilder>();
            builder.RegisterType<LinkedGamesResult>().As<ILinkedGamesResult>();
            builder.RegisterType<WMResultBuilder>().As<IWMResultBuilder>();
            builder.RegisterType<SportsLotteryResult>().As<ISportsLotteryResult>();
            builder.RegisterType<IpInfoService>().As<IIpInfoService>();
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

        private static Assembly[] GetControllerAssemblies(Assembly[] assemblies)
        {
            return assemblies.Where(ass =>
                ass.FullName.Contains("Fanex.Athena.BetList.Controller")
                || ass.FullName.Contains("Fanex.Athena.Result.Controller")).ToArray();
        }
    }
}