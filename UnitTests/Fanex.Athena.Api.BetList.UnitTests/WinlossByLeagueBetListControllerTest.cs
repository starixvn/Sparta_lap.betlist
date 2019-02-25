namespace Fanex.Athena.Api.BetList.UnitTests
{
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Threading.Tasks;
    using Model.BetList.MetaModel;
    using NSubstitute;
    using NUnit.Framework;
    using Session.Model;

    public class WinlossByLeagueBetListControllerTest : ApiBetListBaseControllerHelperTest
    {
        private const string UspKey = "Lrf_BetList_WinlostByLeague_Mem";

        private const string ApiPath = "/BetList/GetWinLossByLeagueData";

        [SetUp]
        public void SetUp()
        {
            MockSessionData();

            AuthorizationService.HasPermissionOnData(SessionLicenseeId, Arg.Any<int>(), Arg.Any<long>()).Returns(true);
        }

        [Test]
        public async Task Detail_InvalidCurrency_ReturnNull()
        {
            using (var controller = CreateController())
            {
                // Arrange
                var metaModel = new WinLossByLeagueBetListMetaModel { CurrencyId = 1 };

                StubUserSessionData(new UserSession { CurrencyList = new Dictionary<int, string>() });

                // Action
                var result = await controller.Index(metaModel);

                // Assert
                Assert.That(result, Is.Null);
            }
        }

        [TestCase(0)]
        [TestCase(1)]
        [TestCase(2)]
        public async Task Detail_WhenCall_CallBetListApiWithCorrectParameters(int currencyId)
        {
            using (var controller = CreateController())
            {
                // Arrange
                var metaModel = new WinLossByLeagueBetListMetaModel { CurrencyId = currencyId };

                StubUserSessionData(new UserSession { CurrencyList = new Dictionary<int, string> { { currencyId, "Currency" } } });

                var expect = new HttpResponseMessage();
                StubBetListApiPostAsync(Arg.Any<WinLossByLeagueBetListMetaModel>(), ApiPath, expect);

                // Action
                var actual = await controller.Index(metaModel);

                // Assert
                // Assert
                Assert.Multiple(() =>
                {
                    Assert.That(expect, Is.EqualTo(actual));
                    Assert.That(metaModel.UspKey, Is.EqualTo(UspKey));
                    BetListApiClient.Received().PostAsync(ApiPath, metaModel);
                });
            }
        }

        private WinlossByLeagueBetListController CreateController()
        {
            return new WinlossByLeagueBetListController(
                SessionService,
                ProductService,
                LangService,
                AuthorizationService,
                BetListApiClient,
                BalanceUpDataAccess);
        }
    }
}