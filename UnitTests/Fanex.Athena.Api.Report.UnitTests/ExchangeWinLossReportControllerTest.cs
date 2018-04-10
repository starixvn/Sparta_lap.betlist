namespace Fanex.Athena.Api.Report.UnitTests
{
    using System;
    using System.Collections.Generic;
    using Base.UnitTests;
    using DataAccess.Report.Interfaces;
    using Model.Core.DataDomain;
    using Model.Core.DTO.Configuration;
    using Model.Core.DTO.Users;
    using Model.Report.MetaModel;
    using NSubstitute;
    using NUnit.Framework;
    using Service.Core.Configuration.Interfaces;

    public class ExchangeWinLossReportControllerTest : BaseApiControllerHelperTest
    {
        private readonly DateTime _minDate = new DateTime(2018, 2, 3);
        private readonly DateTime _maxDate = new DateTime(2018, 8, 7);

        private IReportSettingService _reportSettingService;
        private IExchangeWinLossDataAccess _dataAccess;
        private ExchangeWinLossReportController _controller;

        [OneTimeSetUp]
        public override void FixtureSetUp()
        {
            base.FixtureSetUp();
            _reportSettingService = Substitute.For<IReportSettingService>();
            _reportSettingService.GetSetting(PermissionKeys.ReportExchangeWinLoss).Returns(new ReportSetting
            {
                MinDate = _minDate,
                MaxDate = _maxDate
            });

            _dataAccess = Substitute.For<IExchangeWinLossDataAccess>();

            _controller = Substitute.ForPartsOf<ExchangeWinLossReportController>(
                SessionService,
                ProductService,
                LangService,
                _reportSettingService,
                _dataAccess);

            MockSessionData();
        }

        [OneTimeTearDown]
        public override void FixtureTearDown()
        {
            base.FixtureTearDown();

            _controller = null;
            _dataAccess = null;
        }

        [OneTimeSetUp]
        public void SetUpOneTime()
        {
            StubLicenseeSessionData(new LicenseeSession { LicenseeId = SessionLicenseeId });
        }

        [Test]
        public void Index_InvalidModel_ReturnEmptyData()
        {
            // Arrange
            var model = new ExchangeWinLossMetaModel
            {
                FromDate = DateTime.Now,
                ToDate = DateTime.Now.AddDays(-1)
            };

            // Action
            var result = _controller.Index(model);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.IsEmpty(result.DataList);
                Assert.IsEmpty(result.TotalDataList);
                Assert.IsEmpty(result.SummaryTotal);
            });
        }

        [Test]
        public void Index_InvalidRequest_ReturnEmptyData()
        {
            // Arrange
            StubUserSessionData(new UserSession
            {
                CurrencyList = new Dictionary<int, string>
                {
                    { 1, "Currency 1" },
                    { 2, "Currency 2" }
                }
            });

            var model = new ExchangeWinLossMetaModel
            {
                CurrencyId = 3
            };

            // Action
            var result = _controller.Index(model);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.IsEmpty(result.DataList);
                Assert.IsEmpty(result.TotalDataList);
                Assert.IsEmpty(result.SummaryTotal);
            });
        }

        [Test]
        public void Index_Level0_GetMainData()
        {
            // Arrange
            var model = new ExchangeWinLossMetaModel
            {
                CurrencyId = 0,
                Level = 0
            };

            // Action
            var result = _controller.Index(model);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(model.LicenseeId, Is.EqualTo(SessionLicenseeId));
                _dataAccess.Received().GetMainWinloss(model);
            });
        }

        [Test]
        public void Index_Level2_GetCustomerData()
        {
            // Arrange
            var model = new ExchangeWinLossMetaModel
            {
                CurrencyId = 0,
                Level = 2
            };

            // Action
            var result = _controller.Index(model);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(model.LicenseeId, Is.EqualTo(SessionLicenseeId));
                _dataAccess.Received().GetCustomerWinloss(model);
            });
        }

        [Test]
        public void Index_Level1AndIsNotOneTwoBet_GetCustomerData()
        {
            // Arrange
            const int LicenseeId = LicenseeIdentity.OneTwoBet + 1;
            StubLicenseeSessionData(new LicenseeSession
            {
                LicenseeId = LicenseeId
            });

            var model = new ExchangeWinLossMetaModel
            {
                CurrencyId = 0,
                Level = 1
            };

            // Action
            var result = _controller.Index(model);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(model.LicenseeId, Is.EqualTo(LicenseeId));
                _dataAccess.Received().GetCustomerWinloss(model);
            });
        }

        [Test]
        public void Index_Level1AndIsOneTwoBet_GetLicenseeData()
        {
            // Arrange
            StubLicenseeSessionData(new LicenseeSession
            {
                LicenseeId = LicenseeIdentity.OneTwoBet
            });

            var model = new ExchangeWinLossMetaModel
            {
                CurrencyId = 0,
                Level = 1
            };

            // Action
            var result = _controller.Index(model);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(model.LicenseeId, Is.EqualTo(LicenseeIdentity.OneTwoBet));
                _dataAccess.Received().GetLicenseeWinloss(model);
            });
        }
    }
}