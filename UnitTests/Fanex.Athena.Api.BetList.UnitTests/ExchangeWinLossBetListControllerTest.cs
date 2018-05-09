namespace Fanex.Athena.Api.BetList.UnitTests
{
    using System;
    using System.Collections.Generic;
    using Base.UnitTests;
    using Model.BetList.MetaModel;
    using Model.Core.DTO.Users;
    using NSubstitute;
    using NUnit.Framework;
    using Service.BetList.Interfaces;
    using Service.Core.Users.Interfaces;
    using Session.Model;

    public class ExchangeWinLossBetListControllerTest : BaseApiControllerHelperTest
    {
        private IExchangeBetListService _service;

        private IAuthorizationService _authorizationService;

        private ExchangeWinLossBetListController _controller;

        [OneTimeSetUp]
        public override void FixtureSetUp()
        {
            base.FixtureSetUp();

            MockSessionData();
        }

        [SetUp]
        public void SetUp()
        {
            _service = Substitute.For<IExchangeBetListService>();
            _authorizationService = Substitute.For<IAuthorizationService>();

            _controller = Substitute.ForPartsOf<ExchangeWinLossBetListController>(
                SessionService,
                ProductService,
                LangService,
                _service,
                _authorizationService);
        }

        [TearDown]
        public void TearDown()
        {
            _service = null;
            _authorizationService = null;
            _controller = null;
        }

        [Test]
        public void Index_InValidDateRange_ReturnNullData()
        {
            // Arrange
            var metaModel = new ExchangeBetListMetaModel
            {
                FromDate = new DateTime(2018, 1, 2),
                ToDate = new DateTime(2018, 1, 1)
            };

            // Action
            var result = _controller.Index(metaModel);

            // Assert
            Assert.That(result.MainData, Is.Null);
        }

        [Test]
        public void Index_InValidCurrency_ReturnNullData()
        {
            // Arrange
            var metaModel = new ExchangeBetListMetaModel
            {
                FromDate = new DateTime(2018, 1, 1),
                ToDate = new DateTime(2018, 1, 2),
                CurrencyId = 1
            };

            StubUserSessionData(new UserSession { CurrencyList = new Dictionary<int, string>() });

            // Action
            var result = _controller.Index(metaModel);

            // Assert
            Assert.That(result.MainData, Is.Null);
        }

        [Test]
        public void Index_NoPermission_ReturnNullData()
        {
            // Arrange
            var metaModel = new ExchangeBetListMetaModel
            {
                FromDate = new DateTime(2018, 1, 1),
                ToDate = new DateTime(2018, 1, 2),
                CurrencyId = 0,
                CustId = 1
            };

            _authorizationService.HasPermissionOnData(SessionLicenseeId, metaModel.CurrencyId, 0).Returns(false);

            // Action
            var result = _controller.Index(metaModel);

            // Assert
            Assert.That(result.MainData, Is.Null);
        }

        [Test]
        public void Index_NoCustId_ReturnNullData()
        {
            // Arrange
            var metaModel = new ExchangeBetListMetaModel
            {
                FromDate = new DateTime(2018, 1, 1),
                ToDate = new DateTime(2018, 1, 2),
                CurrencyId = 0,
                CustId = 0
            };

            // Action
            var result = _controller.Index(metaModel);

            // Assert
            Assert.That(result.MainData, Is.Null);
        }

        [TestCase(0, null, "")]
        [TestCase(1, "usd", "USD")]
        [TestCase(2, "usD", "USD")]
        public void Index_ValidRequest_CallServiceWithCorrectParam(int currencyId, string inputBaseCurrency, string expectedBaseCurrency)
        {
            // Arrange
            var metaModel = new ExchangeBetListMetaModel
            {
                FromDate = new DateTime(2018, 1, 1),
                ToDate = new DateTime(2018, 1, 2),
                CurrencyId = currencyId,
                CustId = 100,
                BaseCurrency = inputBaseCurrency
            };

            StubUserSessionData(new UserSession
            {
                CurrencyList = new Dictionary<int, string> { { currencyId, "Currency" } }
            });

            _authorizationService.HasPermissionOnData(SessionLicenseeId, metaModel.CustId, 0).Returns(true);

            // Action
            _controller.Index(metaModel);

            // Assert
            _service.Received().GetBetListData(Arg.Is<ExchangeBetListMetaModel>(x => x == metaModel && x.BaseCurrency == expectedBaseCurrency));
        }

        [Test]
        public void Detail_InValidCurrency_ReturnEmptyData()
        {
            // Arrange
            var metaModel = new ExchangeBetListDetailMetaModel
            {
                CurrencyId = 1,
                BetId = 1,
                CustId = 1,
            };

            StubUserSessionData(new UserSession { CurrencyList = new Dictionary<int, string>() });

            // Action
            var result = _controller.Detail(metaModel);

            // Assert
            Assert.That(result, Is.Empty);
        }

        [Test]
        public void Detail_NoPermission_ReturnEmptyData()
        {
            // Arrange
            var metaModel = new ExchangeBetListDetailMetaModel
            {
                CurrencyId = 0,
                CustId = 1,
                BetId = 1,
            };

            _authorizationService.HasPermissionOnData(SessionLicenseeId, metaModel.CurrencyId, 0).Returns(false);

            // Action
            var result = _controller.Detail(metaModel);

            // Assert
            Assert.That(result, Is.Empty);
        }

        [Test]
        public void Detail_NoBetId_ReturnEmptyData()
        {
            // Arrange
            var metaModel = new ExchangeBetListDetailMetaModel
            {
                CurrencyId = 0,
                CustId = 1,
                BetId = 0
            };

            // Action
            var result = _controller.Detail(metaModel);

            // Assert
            Assert.That(result, Is.Empty);
        }

        [Test]
        public void Detail_NoCustId_ReturnEmptyData()
        {
            // Arrange
            var metaModel = new ExchangeBetListDetailMetaModel
            {
                CurrencyId = 0,
                CustId = 0,
                BetId = 1
            };

            // Action
            var result = _controller.Detail(metaModel);

            // Assert
            Assert.That(result, Is.Empty);
        }

        [TestCase(0)]
        [TestCase(1)]
        [TestCase(2)]
        public void Detail_ValidRequest_ReturnDataFromService(int currencyId)
        {
            // Arrange
            var metaModel = new ExchangeBetListDetailMetaModel
            {
                BetId = 1,
                CurrencyId = currencyId,
                CustId = 200
            };

            StubUserSessionData(new UserSession
            {
                CurrencyList = new Dictionary<int, string> { { currencyId, "Currency" } }
            });

            _authorizationService.HasPermissionOnData(SessionLicenseeId, metaModel.CustId, 0).Returns(true);

            // Action
            _controller.Detail(metaModel);

            // Assert
            _service.Received().GetTicketDetailList(metaModel);
        }
    }
}