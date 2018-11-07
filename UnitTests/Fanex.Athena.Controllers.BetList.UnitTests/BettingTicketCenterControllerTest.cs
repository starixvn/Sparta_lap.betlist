namespace Fanex.Athena.Controllers.BetList.UnitTests
{
    using System;
    using System.Linq;
    using System.Web.Mvc;
    using Base.UnitTests;
    using Model.BetList.ViewModel;
    using Model.Core.DataDomain;
    using Model.Core.DTO.Common;
    using NSubstitute;
    using NUnit.Framework;
    using Service.Core.Common.Interfaces;
    using UnitTest.Common.Helpers;

    public class BettingTicketCenterControllerTest : BaseControllerHelperTest
    {
        private IProductService _productService;

        [OneTimeSetUp]
        public override void FixtureSetUp()
        {
            base.FixtureSetUp();

            _productService = Substitute.For<IProductService>();

            SetUpDefaultFakeData();
        }

        [OneTimeTearDown]
        public override void FixtureTearDown()
        {
            base.FixtureTearDown();

            _productService = null;
        }

        [Test]
        public void RefNo_WhenCall_ReturnCorrectView()
        {
            using (var controller = CreateController())
            {
                // Action
                var result = controller.RefNo() as ViewResult;

                // Assert
                Assert.That(result.ViewName, Is.EqualTo("~/Views/BetList/BettingTicketCenter/RefNo.cshtml"));
            }
        }

        [Test]
        public void RefNo_WhenCall_ReturnCorrectViewModel()
        {
            using (var controller = CreateController())
            {
                // Action
                var result = controller.RefNo() as ViewResult;

                // Assert
                Assert.That(result.Model, Is.EqualTo(DefaultLicenseeSession.IsAllowedCustomerComm));
            }
        }

        [Test]
        public void Running_WhenCall_ReturnCorrectView()
        {
            using (var controller = CreateController())
            {
                // Arrange
                var historyDate = FakeHelper.Create<DateTime>();
                AppSettings.HistoryDate.Returns(historyDate);

                var productCodes = FakeHelper.CreateMany<Product>(3).ToList();
                _productService.GetSingleWalletProductAccess(
                    DefaultLicenseeSession.LicenseeId,
                    DefaultUserSession.Language,
                    FunctionNames.BettingTicketCenter).Returns(new ProductAccess(productCodes));

                // Action
                var result = controller.Running() as ViewResult;

                // Assert
                Assert.That(result.ViewName, Is.EqualTo("~/Views/BetList/BettingTicketCenter/Running.cshtml"));
            }
        }

        [Test]
        public void Running_WhenCall_ReturnCorrectViewModel()
        {
            using (var controller = CreateController())
            {
                // Arrange
                var historyDate = FakeHelper.Create<DateTime>();
                AppSettings.HistoryDate.Returns(historyDate);

                var productCodes = FakeHelper.CreateMany<Product>(3).ToList();
                _productService.GetSingleWalletProductAccess(
                    DefaultLicenseeSession.LicenseeId,
                    DefaultUserSession.Language,
                    FunctionNames.BettingTicketCenter).Returns(new ProductAccess(productCodes));

                // Action
                var result = controller.Running() as ViewResult;
                var viewModel = result.Model as BettingTicketCenterViewModel;

                // Assert
                Assert.Multiple(() =>
                {
                    Assert.That(viewModel.CurrencyList, Is.EqualTo(DefaultUserSession.CurrencyList));
                    Assert.That(viewModel.ProductOptionList.Count(), Is.EqualTo(productCodes.Count));
                    Assert.That(viewModel.MinDate, Is.EqualTo(historyDate));
                    Assert.That(viewModel.IsAllowedCustomerComm, Is.EqualTo(DefaultLicenseeSession.IsAllowedCustomerComm));
                });
            }
        }

        [Test]
        public void SingleCustomer_WhenCall_ReturnCorrectView()
        {
            using (var controller = CreateController())
            {
                // Arrange
                var historyDate = FakeHelper.Create<DateTime>();
                AppSettings.HistoryDate.Returns(historyDate);

                var productCodes = FakeHelper.CreateMany<Product>(3).ToList();
                _productService.GetSingleWalletProductAccess(
                    DefaultLicenseeSession.LicenseeId,
                    DefaultUserSession.Language,
                    FunctionNames.BettingTicketCenter).Returns(new ProductAccess(productCodes));

                // Action
                var result = controller.SingleCustomer() as ViewResult;

                // Assert
                Assert.That(result.ViewName, Is.EqualTo("~/Views/BetList/BettingTicketCenter/SingleCustomer.cshtml"));
            }
        }

        [Test]
        public void SingleCustomer_WhenCall_ReturnCorrectViewModel()
        {
            using (var controller = CreateController())
            {
                // Arrange
                var historyDate = FakeHelper.Create<DateTime>();
                AppSettings.HistoryDate.Returns(historyDate);

                var productCodes = FakeHelper.CreateMany<Product>(3).ToList();
                _productService.GetSingleWalletProductAccess(
                    DefaultLicenseeSession.LicenseeId,
                    DefaultUserSession.Language,
                    FunctionNames.BettingTicketCenter).Returns(new ProductAccess(productCodes));

                // Action
                var result = controller.SingleCustomer() as ViewResult;
                var viewModel = result.Model as BettingTicketCenterViewModel;

                // Assert
                Assert.Multiple(() =>
                {
                    Assert.That(viewModel.LiveStatusList, Is.Not.Empty);
                    Assert.That(viewModel.StatusList, Is.Not.Empty);
                    Assert.That(viewModel.PlatformList, Is.Not.Empty);
                    Assert.That(viewModel.ProductOptionList.Count(), Is.EqualTo(productCodes.Count));
                    Assert.That(viewModel.MinDate, Is.EqualTo(historyDate));
                    Assert.That(viewModel.IsAllowedCustomerComm, Is.EqualTo(DefaultLicenseeSession.IsAllowedCustomerComm));
                });
            }
        }

        [Test]
        public void MultipleCustomers_WhenCall_ReturnCorrectView()
        {
            using (var controller = CreateController())
            {
                // Arrange
                var historyDate = FakeHelper.Create<DateTime>();
                AppSettings.HistoryDate.Returns(historyDate);

                var productCodes = FakeHelper.CreateMany<Product>(3).ToList();
                _productService.GetSingleWalletProductAccess(
                    DefaultLicenseeSession.LicenseeId,
                    DefaultUserSession.Language,
                    FunctionNames.BettingTicketCenter).Returns(new ProductAccess(productCodes));

                // Action
                var result = controller.MultipleCustomers() as ViewResult;

                // Assert
                Assert.That(result.ViewName, Is.EqualTo("~/Views/BetList/BettingTicketCenter/MultiCustomer.cshtml"));
            }
        }

        [Test]
        public void MultipleCustomers_WhenCall_ReturnCorrectViewModel()
        {
            using (var controller = CreateController())
            {
                // Arrange
                var historyDate = FakeHelper.Create<DateTime>();
                AppSettings.HistoryDate.Returns(historyDate);

                var productCodes = FakeHelper.CreateMany<Product>(3).ToList();
                _productService.GetSingleWalletProductAccess(
                    DefaultLicenseeSession.LicenseeId,
                    DefaultUserSession.Language,
                    FunctionNames.BettingTicketCenter).Returns(new ProductAccess(productCodes));

                // Action
                var result = controller.MultipleCustomers() as ViewResult;
                var viewModel = result.Model as BettingTicketCenterViewModel;

                // Assert
                Assert.Multiple(() =>
                {
                    Assert.That(viewModel.CurrencyList, Is.EqualTo(DefaultUserSession.CurrencyList));
                    Assert.That(viewModel.LiveStatusList, Is.Not.Empty);
                    Assert.That(viewModel.StatusList, Is.Not.Empty);
                    Assert.That(viewModel.PlatformList, Is.Not.Empty);
                    Assert.That(viewModel.ProductOptionList.Count(), Is.EqualTo(productCodes.Count));
                    Assert.That(viewModel.MinDate, Is.EqualTo(historyDate));
                    Assert.That(viewModel.IsAllowedCustomerComm, Is.EqualTo(DefaultLicenseeSession.IsAllowedCustomerComm));
                });
            }
        }

        private BettingTicketCenterController CreateController()
        {
            var controller = Substitute.ForPartsOf<BettingTicketCenterController>(
                LangService,
                SessionService,
                AppSettings,
                _productService);
            return controller;
        }
    }
}