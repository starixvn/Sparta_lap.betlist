namespace Fanex.Athena.Api.BetList.UnitTests
{
    using System;
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Threading.Tasks;
    using Model.BetList.MetaModel;
    using Model.Core.DataDomain;
    using Model.Core.DTO.Configuration;
    using NSubstitute;
    using NUnit.Framework;
    using Service.Core.Configuration.Interfaces;
    using Session.Model;

    public class LicenseeWinLossBetListControllerTest : ApiBetListBaseControllerHelperTest
    {
        private const string UspKey = "Lrf_BetList_SelWinloss_Mem";

        private const string ApiPath = "/BetList/GetBetList";

        private IReportSettingService _reportSettingService;

        [SetUp]
        public void SetUp()
        {
            MockSessionData();

            _reportSettingService = Substitute.For<IReportSettingService>();

            AuthorizationService.HasPermissionOnData(SessionLicenseeId, Arg.Any<int>(), Arg.Any<long>()).Returns(true);
        }

        [TearDown]
        public void TearDown()
        {
            _reportSettingService = null;
        }

        [Test]
        public async Task Index_InvalidDateRange_ReturnNull()
        {
            using (var controller = CreateController())
            {
                // Arrange
                var metaModel = new BetListMetaModel { FromDate = new DateTime(2018, 1, 1) };

                StubUserSessionData(new UserSession { CurrencyList = new Dictionary<int, string>() });

                _reportSettingService.GetSetting(PermissionKeys.ReportLicenseeWinLoss).Returns(new ReportSetting
                {
                    MinDate = new DateTime(2018, 2, 2),
                    MaxDate = new DateTime(2018, 3, 3)
                });

                // Action
                var result = await controller.Index(metaModel);

                // Assert
                Assert.That(result, Is.Null);
            }
        }

        [Test]
        public async Task Index_InvalidCurrency_ReturnNull()
        {
            using (var controller = CreateController())
            {
                // Arrange
                var metaModel = new BetListMetaModel { CurrencyId = 1 };

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
        public async Task Index_WhenCall_CallBetListApiWithCorrectParameters(int currencyId)
        {
            using (var controller = CreateController())
            {
                // Arrange
                var metaModel = new BetListMetaModel { CurrencyId = currencyId };

                StubUserSessionData(new UserSession
                {
                    Language = SessionLanguage,
                    CurrencyList = new Dictionary<int, string> { { currencyId, "Currency" } }
                });

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

        private LicenseeWinLossBetListController CreateController()
        {
            return new LicenseeWinLossBetListController(
                SessionService,
                ProductService,
                LangService,
                AuthorizationService,
                BetListApiClient,
                BalanceUpDataAccess,
                _reportSettingService);
        }
    }
}