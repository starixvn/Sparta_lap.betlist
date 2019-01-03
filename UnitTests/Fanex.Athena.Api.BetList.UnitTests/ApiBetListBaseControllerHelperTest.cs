namespace Fanex.Athena.Api.BetList.UnitTests
{
    using System;
    using System.Collections.Generic;
    using DataAccess.Core.Configuration.Interfaces;
    using Fanex.Athena.Api.Base.UnitTests;
    using Fanex.BetList.Core.Builder;
    using Fanex.BetList.Core.Entities;
    using Model.Core.DTO.Configuration;
    using NSubstitute;
    using NUnit.Framework;
    using Service.BetList.Interfaces;
    using VendorAdapter.Interfaces;

    public class ApiBetListBaseControllerHelperTest : BaseApiControllerHelperTest
    {
        protected readonly BalanceUp DefaultBalanceUp = new BalanceUp { BalanceUpDay = new DateTime(2018, 1, 1), MoveBetsDay = new DateTime(2018, 1, 2) };

        protected IBetListService BetListService { get; private set; }

        protected IBalanceUpDataAccess BalanceUpDataAccess { get; private set; }

        protected ITicketHelper TicketHelper { get; private set; }

        protected ICacheRefAdapter CacheRefAdapter { get; private set; }

        protected IBetListHTMLBuilder BetListHTMLBuilder { get; private set; }

        [OneTimeSetUp]
        public override void FixtureSetUp()
        {
            base.FixtureSetUp();

            BetListService = Substitute.For<IBetListService>();
            BalanceUpDataAccess = Substitute.For<IBalanceUpDataAccess>();
            TicketHelper = Substitute.For<ITicketHelper>();
            CacheRefAdapter = Substitute.For<ICacheRefAdapter>();
            BetListHTMLBuilder = Substitute.For<IBetListHTMLBuilder>();

            BalanceUpDataAccess.GetBalanceUp().Returns(DefaultBalanceUp);
        }
    }
}