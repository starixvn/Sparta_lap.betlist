namespace Fanex.Athena.Api.BetList.UnitTests
{
    using System;
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Threading.Tasks;
    using Model.BetList.MetaModel;
    using Model.Core.DTO.Common;
    using NSubstitute;
    using NUnit.Framework;
    using Session.Model;

    public class OutStandingCompletedBetListControllerTest : ApiBetListBaseControllerHelperTest
    {
        private const string UspKey = "Lrf_BetList_CustomerList_Mem";

        private const string BetListCustomerOutstandingName = "BetList_CustomerOutstanding";

        private const string ApiPath = "/BetList/GetBetListPaging";

        [SetUp]
        public void SetUp()
        {
            MockSessionData();

            AuthorizationService.HasPermissionOnData(SessionLicenseeId, Arg.Any<int>(), Arg.Any<long>()).Returns(true);
        }

        [Test]
        public async Task Detail_InvalidCurrency_ReturnNull()
        {
            using (var controller = CreateController())
            {
                // Arrange
                var metaModel = new BetListMetaModel { CurrencyId = 1, ProductCodeList = "1,2,3" };

                StubUserSessionData(new UserSession { CurrencyList = new Dictionary<int, string>() });

                // Action
                var result = await controller.Index(1, 10, metaModel);

                // Assert
                Assert.That(result, Is.Null);
            }
        }

        [Test]
        public async Task Detail_InvalidDate_ReturnNull()
        {
            using (var controller = CreateController())
            {
                // Arrange
                var metaModel = new BetListMetaModel { CurrencyId = 1, FromDate = new DateTime(2018, 1, 3), ToDate = new DateTime(2018, 1, 1) };

                StubUserSessionData(new UserSession { CurrencyList = new Dictionary<int, string>() });

                // Action
                var result = await controller.Index(1, 10, metaModel);

                // Assert
                Assert.That(result, Is.Null);
            }
        }

        [Test]
        public async Task Detail_InvalidProductCodeList_ReturnNull()
        {
            using (var controller = CreateController())
            {
                // Arrange
                var metaModel = new BetListMetaModel { CurrencyId = 1, ProductCodeList = string.Empty };

                StubUserSessionData(new UserSession { CurrencyList = new Dictionary<int, string> { { metaModel.CurrencyId, "Currency" } } });

                // Action
                var result = await controller.Index(1, 10, metaModel);

                // Assert
                Assert.That(result, Is.Null);
            }
        }

        [TestCase(0)]
        [TestCase(1)]
        [TestCase(2)]
        public async Task Detail_WhenCall_ReturnCorrectResult(int currencyId)
        {
            using (var controller = CreateController())
            {
                // Arrange
                var metaModel = new BetListMetaModel { CurrencyId = currencyId, ProductCodeList = "Product1,Product2" };

                StubUserSessionData(new UserSession { Language = SessionLanguage, CurrencyList = new Dictionary<int, string> { { currencyId, "Currency" } } });

                ProductService.GetProductAccess(
                    SessionLicenseeId,
                    SessionLanguage,
                    BetListCustomerOutstandingName)
                .Returns(new ProductAccess(new List<Product> { new Product { ProductCode = "Product1" }, new Product { ProductCode = "Product3" } }));

                var expect = new HttpResponseMessage();
                StubBetListApiPostAsync(metaModel, ApiPath, expect);

                // Action
                var actual = await controller.Index(1, 10, metaModel);

                // Assert
                Assert.Multiple(() =>
                {
                    Assert.That(actual, Is.EqualTo(expect));
                    Assert.That(metaModel.UspKey, Is.EqualTo(UspKey));
                    Assert.That(metaModel.ProductCodeList, Is.EqualTo("Product1"));
                    BetListApiClient.Received().PostAsync(ApiPath, metaModel);
                });
            }
        }

        private OutStandingCompletedBetListController CreateController()
        {
            return new OutStandingCompletedBetListController(
                SessionService,
                ProductService,
                LangService,
                AuthorizationService,
                BetListApiClient,
                BalanceUpDataAccess);
        }
    }
}