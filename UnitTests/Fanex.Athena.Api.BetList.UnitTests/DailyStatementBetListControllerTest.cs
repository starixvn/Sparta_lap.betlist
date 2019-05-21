namespace Fanex.Athena.Api.BetList.UnitTests
{
    using System;
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Text;
    using System.Threading.Tasks;
    using Model.BetList.MetaModel;
    using Model.Core.DTO.Configuration;
    using Newtonsoft.Json;
    using NSubstitute;
    using NUnit.Framework;
    using Session.Model;

    public class DailyStatementBetListControllerTest : ApiBetListBaseControllerHelperTest
    {
        private const string UspKey = "Lrf_BetList_SelByDayAll_Mem";

        private const string UspKeyOld = "Lrf_BetList_SelByDayAll_Mem_Old";

        private const int RunningTicketTransType = 3;

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

        [TestCase(0, "01/01/2018", "01/02/2018", UspKeyOld)]
        [TestCase(1, "01/03/2018", "01/02/2018", UspKey)]
        [TestCase(2, "01/03/2018", "01/02/2018", UspKey)]
        public async Task Index_WhenCall_CallBetListApiWithCorrectParameters(int currencyId, string fromDate, string balanceUpDay, string uspKey)
        {
            using (var controller = CreateController())
            {
                // Arrange
                var metaModel = new BetListMetaModel
                {
                    CurrencyId = currencyId,
                    FromDate = DateTime.Parse(fromDate),
                    ToDate = new DateTime(2018, 1, 1)
                };

                BalanceUpDataAccess.GetBalanceUp().Returns(new BalanceUp { BalanceUpDay = DateTime.Parse(balanceUpDay) });
                StubUserSessionData(new UserSession { CurrencyList = new Dictionary<int, string> { { currencyId, "Test" } } });

                dynamic betListResult = new
                {
                    Main = "Main",
                    Total = "Total",
                    LeagueName = "Name"
                };
                var content = new StringContent(JsonConvert.SerializeObject(betListResult), Encoding.UTF8, "application/json");

                var expect = new HttpResponseMessage { Content = content };
                StubBetListApiPostAsync(metaModel, ApiPath, expect);

                // Action
                var actual = await controller.Index(metaModel);

                // Assert
                Assert.Multiple(() =>
                {
                    Assert.That(metaModel.UspKey, Is.EqualTo(uspKey));
                    BetListApiClient.Received().PostAsync(ApiPath, metaModel, SessionLanguage);
                });
            }
        }

        [TestCase(1, true)]
        [TestCase(RunningTicketTransType, false)]
        public async Task Index_WhenCall_ReturnCorrectResult(int transType, bool isShowTotal)
        {
            using (var controller = CreateController())
            {
                // Arrange
                var metaModel = new BetListMetaModel
                {
                    CurrencyId = 0,
                    FromDate = new DateTime(2018, 1, 1),
                    ToDate = new DateTime(2018, 1, 1),
                    TransType = transType
                };

                dynamic betListResult = new
                {
                    Main = "Main",
                    Total = "Total",
                    LeagueName = "Name"
                };
                var content = new StringContent(JsonConvert.SerializeObject(betListResult), Encoding.UTF8, "application/json");

                var expect = new HttpResponseMessage { Content = content };
                StubBetListApiPostAsync(metaModel, ApiPath, expect);

                // Action
                var actual = await controller.Index(metaModel);

                // Assert
                Assert.Multiple(() =>
                {
                    Assert.That(actual.Main.ToString(), Is.EqualTo(betListResult.Main));
                    Assert.That(actual.Total.ToString(), Is.EqualTo(betListResult.Total));
                    Assert.That(actual.LeagueName.ToString(), Is.EqualTo(betListResult.LeagueName));
                    Assert.That(actual.IsShowTotal, Is.EqualTo(isShowTotal));
                });
            }
        }

        private DailyStatementBetListController CreateController()
        {
            return new DailyStatementBetListController(
                SessionService,
                ProductService,
                LangService,
                AuthorizationService,
                BetListApiClient,
                BalanceUpDataAccess);
        }
    }
}