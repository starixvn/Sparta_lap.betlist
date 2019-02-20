namespace Fanex.Athena.Controllers.BetList.UnitTests
{
    using System;
    using System.Collections.Generic;
    using Base.UnitTests;
    using DataAccess.Core.Configuration.Interfaces;
    using Model.BetList.MetaModel;
    using Model.Core.DTO.Configuration;
    using NSubstitute;
    using NUnit.Framework;
    using Service.BetList.Interfaces;
    using Service.Core.Common.Interfaces;
    using Service.Core.Users.Interfaces;
    using VendorAdapter.Interfaces;

    public class BetListBaseControllerHelperTest : BaseControllerHelperTest
    {
        protected static readonly BalanceUp BalanceUp = new BalanceUp
        {
            BalanceUpDay = new DateTime(2018, 1, 1),
            MoveBetsDay = new DateTime(2018, 1, 1)
        };

        protected IBalanceUpDataAccess BalanceUpDataAccess { get; private set; }

        protected IProductService ProductService { get; private set; }

        protected IBetListService BetListService { get; private set; }

        protected ICacheRefAdapter CacheRefAdapter { get; private set; }

        [OneTimeSetUp]
        public override void FixtureSetUp()
        {
            base.FixtureSetUp();

            BalanceUpDataAccess = Substitute.For<IBalanceUpDataAccess>();
            ProductService = Substitute.For<IProductService>();
            BetListService = Substitute.For<IBetListService>();
            CacheRefAdapter = Substitute.For<ICacheRefAdapter>();

            BalanceUpDataAccess.GetBalanceUp().Returns(BalanceUp);
        }

        [OneTimeTearDown]
        public override void FixtureTearDown()
        {
            base.FixtureTearDown();

            BalanceUpDataAccess = null;
            ProductService = null;
            BetListService = null;
        }

        protected void AssertDefaultBetListViewModel(BetListMetaModel viewModel, BetListMetaModel metaModel)
        {
            Assert.That(viewModel.LicenseeId, Is.EqualTo(DefaultLicenseeSession.LicenseeId));
            Assert.That(viewModel.UserId, Is.EqualTo(DefaultUserSession.UserId));
            Assert.That(viewModel.BalanceUpDay.Date, Is.EqualTo(BalanceUp.BalanceUpDay.Date));
            Assert.That(viewModel.MoveBetsDay.Date, Is.EqualTo(BalanceUp.MoveBetsDay.Date));
            Assert.That(viewModel.FromDate.Date, Is.EqualTo(metaModel.FromDate.Date));
            Assert.That(viewModel.ToDate.Date, Is.EqualTo(metaModel.ToDate.Date));
            Assert.That(viewModel.Date.Date, Is.EqualTo(metaModel.Date.Date));
        }
    }
}