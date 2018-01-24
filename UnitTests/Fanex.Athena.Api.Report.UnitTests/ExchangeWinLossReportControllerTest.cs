namespace Fanex.Athena.Api.Report.UnitTests
{
    using System;
    using System.Collections.Generic;
    using DataAccess.Report.Interfaces;
    using Model.Core.DataDomain;
    using Model.Core.DTO.Users;
    using Model.Report.MetaModel;
    using NSubstitute;
    using NUnit.Framework;

    public class ExchangeWinLossReportControllerTest : BaseApiReportControllerHelperTest
    {
        private IExchangeWinLossDataAccess _dataAccess;
        private ExchangeWinLossReportController _controller;

        [OneTimeSetUp]
        public override void FixtureSetUp()
        {
            base.FixtureSetUp();

            _dataAccess = Substitute.For<IExchangeWinLossDataAccess>();

            _controller = Substitute.ForPartsOf<ExchangeWinLossReportController>(SessionService, ProductService, _dataAccess);

            MockSessionData();
        }

        [OneTimeTearDown]
        public override void FixtureTearDown()
        {
            base.FixtureTearDown();

            _controller = null;
            _dataAccess = null;
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
            _dataAccess.Received().GetMainWinloss(model);
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
            _dataAccess.Received().GetCustomerWinloss(model);
        }

        [Test]
        public void Index_Level1AndIsNotOneTwoBet_GetCustomerData()
        {
            // Arrange
            StubLicenseeSessionData(new LicenseeSession
            {
                LicenseeId = LicenseeIdentity.OneTwoBet + 1
            });

            var model = new ExchangeWinLossMetaModel
            {
                CurrencyId = 0,
                Level = 1
            };

            // Action
            var result = _controller.Index(model);

            // Assert
            _dataAccess.Received().GetCustomerWinloss(model);
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
            _dataAccess.Received().GetLicenseeWinloss(model);
        }
    }
}