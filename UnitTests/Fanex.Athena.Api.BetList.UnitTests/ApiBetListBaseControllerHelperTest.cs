namespace Fanex.Athena.Api.BetList.UnitTests
{
    using System;
    using System.Net.Http;
    using System.Threading.Tasks;
    using DataAccess.Core.Configuration.Interfaces;
    using Fanex.Athena.Api.Base.UnitTests;
    using Model.BetList.MetaModel;
    using Model.Core.DTO.Configuration;
    using NSubstitute;
    using NUnit.Framework;
    using Service.Core.Utility.Interfaces;
    using Service.StatelessCore.Interfaces;

    public class ApiBetListBaseControllerHelperTest : BaseApiControllerHelperTest
    {
        protected readonly BalanceUp DefaultBalanceUp = new BalanceUp { BalanceUpDay = new DateTime(2018, 1, 1), MoveBetsDay = new DateTime(2018, 1, 2) };

        protected IAuthorizationService AuthorizationService { get; private set; }

        protected IBetListApiClient BetListApiClient { get; private set; }

        protected IBalanceUpDataAccess BalanceUpDataAccess { get; private set; }

        [OneTimeSetUp]
        public override void FixtureSetUp()
        {
            base.FixtureSetUp();

            AuthorizationService = Substitute.For<IAuthorizationService>();
            BetListApiClient = Substitute.For<IBetListApiClient>();
            BalanceUpDataAccess = Substitute.For<IBalanceUpDataAccess>();

            BalanceUpDataAccess.GetBalanceUp().Returns(DefaultBalanceUp);
        }

        [OneTimeTearDown]
        public override void FixtureTearDown()
        {
            base.FixtureTearDown();

            AuthorizationService = null;
            BetListApiClient = null;
            BalanceUpDataAccess = null;
        }

        protected void StubBetListApiPostAsync(
            object metaModel,
            string apiPath,
            HttpResponseMessage result)
        {
            BetListApiClient.PostAsync(apiPath, metaModel).Returns(Task.FromResult(result));
        }
    }
}