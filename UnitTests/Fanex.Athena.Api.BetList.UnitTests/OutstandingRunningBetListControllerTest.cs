namespace Fanex.Athena.Api.BetList.UnitTests
{
    using System;
    using System.Collections.Generic;
    using Model.BetList.MetaModel;
    using Model.Core.DTO.Common;
    using Model.Core.DTO.Users;
    using NSubstitute;
    using NUnit.Framework;

    public class OutstandingRunningBetListControllerTest : ApiBetListBaseControllerHelperTest
    {
        private const string UspKey = "Lrf_BetList_SelRunningByDate_Mem";

        private const string BetListCustomerOutstandingRunningName = "BetList_CustomerOutstandingRunning";

        private OutstandingRunningBetListController _controller;

        [OneTimeSetUp]
        public override void FixtureSetUp()
        {
            base.FixtureSetUp();

            _controller = Substitute.ForPartsOf<OutstandingRunningBetListController>(
                SessionService,
                ProductService,
                LangService,
                BetListService,
                BalanceUpDataAccess,
                TicketHelper,
                CacheRefAdapter,
                BetListHTMLBuilder);
        }

        [OneTimeTearDown]
        public override void FixtureTearDown()
        {
            base.FixtureTearDown();

            _controller = null;
        }

        [Test]
        public void Detail_InvalidCurrency_ReturnNull()
        {
            // Arrange
            var metaModel = new BetListMetaModel { CurrencyId = 1, ProductCodeList = "1,2,3" };

            StubUserSessionData(new UserSession { CurrencyList = new Dictionary<int, string>() });

            // Action
            var result = _controller.Index(metaModel);

            // Assert
            Assert.That(result, Is.Null);
        }

        [Test]
        public void Detail_InvalidDate_ReturnNull()
        {
            // Arrange
            var metaModel = new BetListMetaModel { CurrencyId = 1, FromDate = new DateTime(2018, 1, 3), ToDate = new DateTime(2018, 1, 1) };

            StubUserSessionData(new UserSession { CurrencyList = new Dictionary<int, string>() });

            // Action
            var result = _controller.Index(metaModel);

            // Assert
            Assert.That(result, Is.Null);
        }

        [Test]
        public void Detail_InvalidProductCodeList_ReturnNull()
        {
            // Arrange
            var metaModel = new BetListMetaModel { CurrencyId = 1, ProductCodeList = string.Empty };

            StubUserSessionData(new UserSession { CurrencyList = new Dictionary<int, string> { { metaModel.CurrencyId, "Currency" } } });

            // Action
            var result = _controller.Index(metaModel);

            // Assert
            Assert.That(result, Is.Null);
        }

        [TestCase(0)]
        [TestCase(1)]
        [TestCase(2)]
        public void Detail_WhenCall_GetBetListResultWithCorrectParameter(int currencyId)
        {
            // Arrange
            var metaModel = new BetListMetaModel { CurrencyId = currencyId, ProductCodeList = "Product1,Product2" };

            StubLicenseeSessionData(new LicenseeSession { LicenseeId = SessionLicenseeId });
            StubUserSessionData(new UserSession { Language = SessionLanguage, CurrencyList = new Dictionary<int, string> { { currencyId, "Currency" } } });
            SessionService.GenerateAgentListXml(metaModel.CurrencyId).Returns("<Xml/>");

            ProductService.GetProductAccess(
                SessionLicenseeId,
                SessionLanguage,
                BetListCustomerOutstandingRunningName)
            .Returns(new ProductAccess(new List<Product> { new Product { ProductCode = "Product1" }, new Product { ProductCode = "Product3" } }));

            _controller.When(x => x.GetBetListResult(metaModel, UspKey)).DoNotCallBase();

            // Action
            _controller.Index(metaModel);

            // Assert
            Assert.Multiple(() =>
            {
                _controller.Received().GetBetListResult(metaModel, UspKey);
                Assert.That(metaModel.ToDate.Hour, Is.EqualTo(23));
                Assert.That(metaModel.ProductCodeList, Is.EqualTo("Product1"));
            });
        }
    }
}