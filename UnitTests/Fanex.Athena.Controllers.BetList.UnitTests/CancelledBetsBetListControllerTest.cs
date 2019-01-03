namespace Fanex.Athena.Controllers.BetList.UnitTests
{
    using System;
    using System.Collections.Generic;
    using System.Web.Mvc;
    using Excel.Base.Entities;
    using Fanex.Athena.DataAccess.BetList.Interfaces;
    using Fanex.Athena.Model.BetList.DTO;
    using Fanex.Athena.Model.BetList.MetaModel;
    using Fanex.Athena.Model.BetList.ViewModel;
    using Fanex.Athena.Model.Core.DTO.Common;
    using NSubstitute;
    using NUnit.Framework;
    using Resources;
    using Service.StatelessCore.Interfaces;

    public class CancelledBetsBetListControllerTest : BetListBaseControllerHelperTest
    {
        private IAuthorizationService _authorizationService;

        private ICancelledBetListDataAccess _cancelledBetListDataAccess;

        [OneTimeSetUp]
        public override void FixtureSetUp()
        {
            base.FixtureSetUp();
            _authorizationService = Substitute.For<IAuthorizationService>();
            _cancelledBetListDataAccess = Substitute.For<ICancelledBetListDataAccess>();

            ProductService.GetProductAccess(Arg.Any<int>(), Arg.Any<string>())
                .Returns(new ProductAccess(new List<Product> { new Product { ProductCode = Product.AB } }));

            SetUpDefaultFakeData();

            StubGetValueAppSettings("BaseUsdLicensees", "usd");
        }

        [Test]
        public void Index_WhenGet_ReturnCorrectView()
        {
            using (var controller = CreateController())
            {
                // Arrange
                _authorizationService.HasPermissionOnData(Arg.Any<int>(), Arg.Any<int>(), Arg.Any<long>()).Returns(false);

                // Action
                var result = controller.Index() as ViewResult;

                // Assert
                Assert.That(result.ViewName, Is.EqualTo("~/Views/Desktop/BetList/CancelledBetsBetList/Main.cshtml"));
            }
        }

        [Test]
        public void Index_WhenGet_ReturnCorrectViewModel()
        {
            using (var controller = CreateController())
            {
                // Arrange
                _authorizationService.HasPermissionOnData(Arg.Any<int>(), Arg.Any<int>(), Arg.Any<long>()).Returns(true);
                _cancelledBetListDataAccess.GetCancelBetCustList(Arg.Any<BetListMetaModel>())
                    .Returns(new List<CustomerCancelBetItem>());

                // Action
                var result = controller.Index() as ViewResult;
                var model = (CancelledBetsBetListViewModel)result.Model;

                // Assert
                Assert.Multiple(() =>
                {
                    Assert.That(model.ShowEmptyContent, Is.True);
                    Assert.That(model.CustomerList, Is.Empty);
                    Assert.That(model.Param.CustId, Is.Zero);
                    AssertDefaultBetListViewModel(model.Param, new BetListMetaModel());
                });
            }
        }

        [Test]
        public void Index_WhenGet_ReturnCorrectCustId()
        {
            using (var controller = CreateController())
            {
                // Arrange
                _authorizationService.HasPermissionOnData(Arg.Any<int>(), Arg.Any<int>(), Arg.Any<long>()).Returns(true);

                var cancelBetCust = new CustomerCancelBetItem { CustId = 2 };
                var cancelBetCustList = new List<CustomerCancelBetItem> { cancelBetCust };

                _cancelledBetListDataAccess.GetCancelBetCustList(Arg.Any<BetListMetaModel>()).Returns(cancelBetCustList);

                // Action
                var result = controller.Index() as ViewResult;
                var model = (CancelledBetsBetListViewModel)result.Model;

                // Assert
                Assert.Multiple(() =>
                {
                    Assert.That(model.CustomerList, Is.EqualTo(cancelBetCustList));
                    Assert.That(model.Param.CustId, Is.EqualTo(cancelBetCust.CustId));
                });
            }
        }

        [Test]
        public void Index_WhenPost_ReturnCorrectView()
        {
            using (var controller = CreateController())
            {
                // Arrange
                _authorizationService.HasPermissionOnData(Arg.Any<int>(), Arg.Any<int>(), Arg.Any<long>()).Returns(false);

                // Action
                var result = controller.Index(new BetListMetaModel()) as ViewResult;

                // Assert
                Assert.That(result.ViewName, Is.EqualTo("~/Views/Desktop/BetList/CancelledBetsBetList/Main.cshtml"));
            }
        }

        [Test]
        public void Index_WhenPost_ReturnCorrectViewModel()
        {
            using (var controller = CreateController())
            {
                // Arrange
                _authorizationService.HasPermissionOnData(Arg.Any<int>(), Arg.Any<int>(), Arg.Any<long>()).Returns(true);
                _cancelledBetListDataAccess.GetCancelBetCustList(Arg.Any<BetListMetaModel>())
                    .Returns(new List<CustomerCancelBetItem>());

                var metaModel = new BetListMetaModel
                {
                    FromDate = new DateTime(2018, 2, 1),
                    ToDate = new DateTime(2018, 2, 10),
                    Date = new DateTime(2018, 2, 5)
                };

                // Action
                var result = controller.Index(metaModel) as ViewResult;
                var model = (CancelledBetsBetListViewModel)result.Model;

                // Assert
                Assert.Multiple(() =>
                {
                    Assert.That(model.ShowEmptyContent, Is.True);
                    Assert.That(model.CustomerList, Is.Empty);
                    Assert.That(model.Param.CustId, Is.Zero);
                    AssertDefaultBetListViewModel(model.Param, metaModel);
                });
            }
        }

        [Test]
        public void Index_ExportExcel_InvalidCurrency_ReturnEmptyResult()
        {
            using (var controller = CreateController())
            {
                // Arrange
                var metaModel = new BetListMetaModel
                {
                    IsExportExcel = true,
                    CurrencyId = -1000
                };

                controller.When(x => x.EmptyExcel()).DoNotCallBase();

                // Action
                controller.Index(metaModel);

                // Assert
                controller.Received().EmptyExcel();
            }
        }

        [TestCase("USD", null, "USD")]
        [TestCase("USD", "eur", "EUR")]
        public void Index_ExportExcel_PassCorrectParam(string currency, string baseCurrency, string expectedCurrency)
        {
            using (var controller = CreateController())
            {
                // Arrange
                const string AgentListXml = "<xml/>";

                var metaModel = new BetListMetaModel
                {
                    IsExportExcel = true,
                    CurrencyId = 0,
                    CurrencyName = currency,
                    BaseCurrency = baseCurrency
                };

                var currencyTitle = string.Format(ViewResources.Viewin, expectedCurrency);
                var expectedTitle = $"{ViewResources.CancelledBets} - {currencyTitle}";

                var ticketResult = new TicketResult();
                BetListService.GetBetListData(Arg.Any<BetListMetaModel>(), UserCurrencyList, "Lrf_BetList_SelDeleted").Returns(ticketResult);

                SessionService.GenerateAgentListXml(metaModel.CurrencyId, false).Returns(AgentListXml);

                controller.When(x => x.ProcessExportExcel(
                    Arg.Any<ShowingOption>(),
                    Arg.Any<BetListMetaModel>(),
                    Arg.Any<TicketResult>(),
                    Arg.Any<IEnumerable<int>>(),
                    Arg.Any<string>(),
                    Arg.Any<string>(),
                    Arg.Any<int>())).DoNotCallBase();

                // Action
                controller.Index(metaModel);

                // Assert
                controller.Received().ProcessExportExcel(
                    Arg.Is<ShowingOption>(x => x.CustomerCommVisible == DefaultLicenseeSession.IsAllowedCustomerComm),
                    Arg.Is<BetListMetaModel>(x => x.LicenseeId == DefaultLicenseeSession.LicenseeId && !x.IsShowSumStake && x.AgentListXml == AgentListXml),
                    ticketResult,
                    Arg.Is<IEnumerable<int>>(x => string.Join(",", x) == "7,8,9"),
                    expectedTitle,
                    "CancelledBetList",
                    1);
            }
        }

        private CancelledBetsBetListController CreateController()
        {
            var controller = Substitute.ForPartsOf<CancelledBetsBetListController>(
                LangService,
                SessionService,
                AppSettings,
                ProductService,
                BetListService,
                BalanceUpDataAccess,
                CacheRefAdapter,
                _authorizationService,
                _cancelledBetListDataAccess);
            return controller;
        }
    }
}