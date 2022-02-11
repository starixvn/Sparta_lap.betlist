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
    using Fanex.Athena.Common.Utility.Services.Internal;
    using Fanex.Athena.Result.Service;
    using Fanex.Athena.Result.Service.Interfaces;
    using Fanex.BetList.Core.Builder;
    using Fanex.BetList.Core.Entities;
    using Fanex.BetList.Core.TicketHelper;
    using Fanex.Logging;
    using Fanex.RemotingRef;
    using Fanex.Result.Core.AllBetCasino;
    using Fanex.Result.Core.Casino;
    using Fanex.Result.Core.LinkedGames;
    using Fanex.Result.Core.SabaClub;
    using Fanex.Result.Core.SAGaming;
    using Fanex.Result.Core.SexyGaming;
    using Fanex.Result.Core.VirtualGames;
    using Fanex.Result.Core.SportsLottery;
    using Fanex.Result.Core.WM;
    using Fanex.Result.Core.VGaming;
    using Fanex.Athena.BetList.Service;
    using Fanex.Athena.BetList.Service.Interfaces;

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
            builder.RegisterType<IPService>().As<IIPService>().SingleInstance();
            builder.RegisterType<TicketHelper>().As<ITicketHelper>().InstancePerRequest();
            builder.RegisterType<BetListHTMLBuilder>().As<IBetListHTMLBuilder>().InstancePerRequest();
            builder.RegisterType<BetListExcelBuilder>().As<IBetListExcelBuilder>().InstancePerRequest();
            builder.RegisterType<AllbetResultBuilder>().As<IAllbetResultBuilder>();
            builder.RegisterType<SabaClubResult>().As<ISabaClubResult>();
            builder.RegisterType<RemotingRefService>().As<IRemotingRefService>().SingleInstance();
            builder.RegisterInstance(Logger.Log).As<Logging.ILogger>();
            builder.Register(x => new CasinoResult("SpadegamingResultConfigs")).Keyed<ICasinoResult>("SG");
            builder.RegisterType<VirtualGamesResult>().As<IVirtualGamesResult>();
            builder.RegisterType<Indo4DResult>().As<IIndo4DResult>();
            builder.RegisterType<SexyGamingResultBuilder>().As<ISexyGamingResultBuilder>();
            builder.RegisterType<SAGamingResultBuilder>().As<ISAGamingResultBuilder>();
            builder.RegisterType<LinkedGamesResult>().As<ILinkedGamesResult>();
            builder.RegisterType<WMResultBuilder>().As<IWMResultBuilder>();
            builder.RegisterType<VGamingResult>().As<IVGamingResult>();
            builder.RegisterType<SportsLotteryResult>().As<ISportsLotteryResult>();
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