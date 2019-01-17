namespace Fanex.Athena.Api.BetList.UnitTests
{
    using System;
    using System.Collections.Generic;
    using System.Dynamic;
    using System.Threading.Tasks;
    using Model.BetList.MetaModel;
    using Model.Core.DTO.Common;
    using NSubstitute;
    using NSubstitute.Core;
    using NUnit.Framework;
    using Session.Model;

    public class OutStandingCompletedBetListControllerTest : ApiBetListBaseControllerHelperTest
    {
        private const string UspKey = "Lrf_BetList_CustomerList_Mem";

        private const string BetListCustomerOutstandingName = "BetList_CustomerOutstanding";

        private OutStandingCompletedBetListController _controller;

        [OneTimeSetUp]
        public override void FixtureSetUp()
        {
            base.FixtureSetUp();

            _controller = Substitute.ForPartsOf<OutStandingCompletedBetListController>(
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
            var result = _controller.Index(1, 10, metaModel);

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
            var result = _controller.Index(1, 10, metaModel);

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
            var result = _controller.Index(1, 10, metaModel);

            // Assert
            Assert.That(result, Is.Null);
        }

        [TestCase(0)]
        [TestCase(1)]
        [TestCase(2)]
        public void Detail_WhenCall_ReturnCorrectResult(int currencyId)
        {
            // Arrange
            var metaModel = new BetListMetaModel { CurrencyId = currencyId, ProductCodeList = "Product1,Product2" };

            StubLicenseeSessionData(new LicenseeSession { LicenseeId = SessionLicenseeId });
            StubUserSessionData(new UserSession { Language = SessionLanguage, CurrencyList = new Dictionary<int, string> { { currencyId, "Currency" } } });
            SessionService.GenerateAgentListXml(metaModel.CurrencyId).Returns("<Xml/>");

            ProductService.GetProductAccess(
                SessionLicenseeId,
                SessionLanguage,
                BetListCustomerOutstandingName)
            .Returns(new ProductAccess(new List<Product> { new Product { ProductCode = "Product1" }, new Product { ProductCode = "Product3" } }));

            dynamic betList = new ExpandoObject();
            betList.Main = "Main";
            betList.LeagueName = "Name";
            betList.Total = "Total";
            _controller.When(async x => await x.GetPagingBetListResult(metaModel, UspKey, Arg.Any<int>(), Arg.Any<int>())).DoNotCallBase();
            _controller.GetPagingBetListResult(metaModel, UspKey, 1, 10).Returns(Task.FromResult((object)betList));

            // Action
            var result = _controller.Index(1, 10, metaModel).Result;
            

            // Assert
            Assert.Multiple(() =>
            {
                Received.InOrder(async () =>
                {
                    await _controller.Received(1).GetPagingBetListResult(metaModel, UspKey, 1, 10);
                });
                Assert.That(metaModel.ProductCodeList, Is.EqualTo("Product1"));

                Assert.That(result.Main, Is.EqualTo(betList.Main));
                Assert.That(result.LeagueName, Is.EqualTo(betList.LeagueName));
                Assert.That(result.Total, Is.EqualTo(betList.Total));
            });
        }
    }
}