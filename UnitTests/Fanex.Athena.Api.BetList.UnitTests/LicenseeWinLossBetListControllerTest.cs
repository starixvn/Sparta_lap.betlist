namespace Fanex.Athena.Api.BetList.UnitTests
{
    using System;
    using System.Collections.Generic;
    using Model.BetList.MetaModel;
    using Model.Core.DataDomain;
    using Model.Core.DTO.Configuration;
    using Model.Core.DTO.Users;
    using NSubstitute;
    using NUnit.Framework;
    using Service.Core.Configuration.Interfaces;

    public class LicenseeWinLossBetListControllerTest : ApiBetListBaseControllerHelperTest
    {
        private const string UspKey = "Lrf_BetList_SelWinloss_Mem";
        private IReportSettingService _reportSettingService;

        private LicenseeWinLossBetListController _controller;

        [SetUp]
        public void SetUp()
        {
            _reportSettingService = Substitute.For<IReportSettingService>();
            _controller = Substitute.ForPartsOf<LicenseeWinLossBetListController>(
                SessionService,
                ProductService,
                LangService,
                BetListService,
                BalanceUpDataAccess,
                TicketHelper,
                CacheRefAdapter,
                BetListHTMLBuilder,
                _reportSettingService);

            StubLicenseeSessionData(new LicenseeSession { LicenseeId = SessionLicenseeId });
        }

        [Test]
        public void Index_InvalidDateRange_ReturnNull()
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
            var result = _controller.Index(metaModel);

            // Assert
            Assert.That(result, Is.Null);
        }

        [Test]
        public void Index_InvalidCurrency_ReturnNull()
        {
            // Arrange
            var metaModel = new BetListMetaModel { CurrencyId = 1 };

            StubUserSessionData(new UserSession { CurrencyList = new Dictionary<int, string>() });

            // Action
            var result = _controller.Index(metaModel);

            // Assert
            Assert.That(result, Is.Null);
        }

        [TestCase(0)]
        [TestCase(1)]
        [TestCase(2)]
        public void Index_WhenCall_GetBetListResultWithCorrectParameter(int currencyId)
        {
            // Arrange
            var metaModel = new BetListMetaModel { CurrencyId = currencyId };

            StubUserSessionData(new UserSession
            {
                CurrencyList = new Dictionary<int, string> { { currencyId, "Currency" } }
            });
            SessionService.GenerateAgentListXml(metaModel.CurrencyId).Returns("<Xml/>");

            _controller.WhenForAnyArgs(x => x.GetBetListResult(metaModel, UspKey)).DoNotCallBase();

            // Action
            _controller.Index(metaModel);

            // Assert
            _controller.Received().GetBetListResult(metaModel, UspKey);
        }
    }
}