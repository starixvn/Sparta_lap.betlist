namespace Fanex.Athena.Api.BetList.UnitTests
{
    using System.Net.Http;
    using System.Threading.Tasks;
    using Model.BetList.MetaModel;
    using NSubstitute;
    using NUnit.Framework;
    using UnitTest.Common.Helpers;

    public class DailyBetListControllerTest : ApiBetListBaseControllerHelperTest
    {
        private const string UspKey = "Lrf_BetList_SelWinlossDaily";

        private const string ApiPath = "/BetList/GetDailyBetListData";

        [SetUp]
        public void SetUp()
        {
            MockSessionData();

            AuthorizationService.HasPermissionOnData(SessionLicenseeId, Arg.Any<int>(), Arg.Any<long>()).Returns(true);
        }

        [TestCase(null, "")]
        [TestCase("test", "test")]
        public async Task Index_WhenCall_CallBetListApiWithCorrectParameters(string baseCurrency, string expectedBaseCurrency)
        {
            using (var controller = new DailyBetListController(
                SessionService,
                ProductService,
                LangService,
                AuthorizationService,
                BetListApiClient,
                BalanceUpDataAccess))
            {
                // Arrange
                var metaModel = FakeHelper.Create<DailyBetListMetaModel>();

                var expect = new HttpResponseMessage();
                StubBetListApiPostAsync(Arg.Any<DailyBetListMetaModel>(), ApiPath, expect);

                // Action
                var actual = await controller.Index(
                    metaModel.PageIndex,
                    metaModel.PageSize,
                    metaModel.CurrencyId,
                    metaModel.CurrencyName,
                    baseCurrency,
                    metaModel.Date,
                    metaModel.ProductCode,
                    metaModel.DailyBetListStatus,
                    metaModel.SortBy,
                    metaModel.SortOrder);

                // Assert
                Assert.Multiple(() =>
                {
                    Assert.That(expect, Is.EqualTo(actual));
                    BetListApiClient.Received().PostAsync(
                        ApiPath,
                        Arg.Is<DailyBetListMetaModel>(x => x.BaseCurrency == expectedBaseCurrency && x.UspKey == UspKey));
                });
            }
        }
    }
}