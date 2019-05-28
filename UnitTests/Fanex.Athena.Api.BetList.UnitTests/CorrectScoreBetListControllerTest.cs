namespace Fanex.Athena.Api.BetList.UnitTests
{
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Threading.Tasks;
    using Model.BetList.MetaModel;
    using NSubstitute;
    using NUnit.Framework;
    using Session.Model;

    public class CorrectScoreBetListControllerTest : ApiBetListBaseControllerHelperTest
    {
        private const string UspKey = "Lrf_TotalBets_Sel430";

        private const string ApiPath = "/BetList/GetBetList";

        [SetUp]
        public void SetUp()
        {
            MockSessionData();

            AuthorizationService.HasPermissionOnData(SessionLicenseeId, Arg.Any<int>(), Arg.Any<long>()).Returns(true);
        }

        [TestCase(-100)]
        [TestCase(100)]
        public async Task Index_InvalidCurrencyId_ReturnNull(int currencyId)
        {
            using (var controller = CreateController())
            {
                // Arrange
                var expect = new HttpResponseMessage();
                var metaModel = new BetListMetaModel
                {
                    CurrencyId = currencyId
                };
                StubUserSessionData(new UserSession { CurrencyList = new Dictionary<int, string>() });

                // Action
                var actual = await controller.Index(metaModel);

                // Assert
                Assert.That(actual, Is.Null);
            }
        }

        [TestCase(0)]
        [TestCase(-1)]
        public async Task Index_ValidMetaModel_CallBetListApiWithCorrectParameters(int currencyId)
        {
            using (var controller = CreateController())
            {
                // Arrange
                var metaModel = new BetListMetaModel
                {
                    CurrencyId = currencyId
                };
                StubUserSessionData(new UserSession { Language = SessionLanguage, CurrencyList = new Dictionary<int, string> { { currencyId, "Test" } } });

                var expect = new HttpResponseMessage();
                StubBetListApiPostAsync(metaModel, ApiPath, expect);

                // Action
                var actual = await controller.Index(metaModel);

                // Assert
                Assert.Multiple(() =>
                {
                    Assert.That(expect, Is.EqualTo(actual));
                    Assert.That(metaModel.UspKey, Is.EqualTo(UspKey));
                    BetListApiClient.Received().PostAsync(ApiPath, metaModel, SessionLanguage);
                });
            }
        }

        private CorrectScoreBetListController CreateController()
        {
            return new CorrectScoreBetListController(
                SessionService,
                ProductService,
                LangService,
                AuthorizationService,
                BetListApiClient,
                BalanceUpDataAccess);
        }
    }
}