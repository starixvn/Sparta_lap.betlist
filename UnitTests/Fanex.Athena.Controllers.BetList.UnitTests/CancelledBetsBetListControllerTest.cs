namespace Fanex.Athena.Controllers.BetList.UnitTests
{
    using System;
    using System.Collections.Generic;
    using System.Web.Mvc;
    using Fanex.Athena.DataAccess.BetList.Interfaces;
    using Fanex.Athena.Model.BetList.DTO;
    using Fanex.Athena.Model.BetList.MetaModel;
    using Fanex.Athena.Model.BetList.ViewModel;
    using Fanex.Athena.Model.Core.DTO.Common;
    using NSubstitute;
    using NUnit.Framework;

    public class CancelledBetsBetListControllerTest : BetListBaseControllerHelperTest
    {
        private ICancelledBetListDataAccess _cancelledBetListDataAccess;

        [OneTimeSetUp]
        public override void FixtureSetUp()
        {
            base.FixtureSetUp();

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
                AuthorizationService.HasPermissionOnData(Arg.Any<int>(), Arg.Any<int>(), Arg.Any<long>()).Returns(false);

                // Action
                var result = controller.Index() as ViewResult;

                // Assert
                Assert.That(result.ViewName, Is.EqualTo("~/Views/BetList/CancelledBetsBetList/Main.cshtml"));
            }
        }

        [Test]
        public void Index_WhenGet_ReturnCorrectViewModel()
        {
            using (var controller = CreateController())
            {
                // Arrange
                AuthorizationService.HasPermissionOnData(Arg.Any<int>(), Arg.Any<int>(), Arg.Any<long>()).Returns(true);
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
                AuthorizationService.HasPermissionOnData(Arg.Any<int>(), Arg.Any<int>(), Arg.Any<long>()).Returns(true);

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
                AuthorizationService.HasPermissionOnData(Arg.Any<int>(), Arg.Any<int>(), Arg.Any<long>()).Returns(false);

                // Action
                var result = controller.Index(new BetListMetaModel()) as ViewResult;

                // Assert
                Assert.That(result.ViewName, Is.EqualTo("~/Views/BetList/CancelledBetsBetList/Main.cshtml"));
            }
        }

        [Test]
        public void Index_WhenPost_ReturnCorrectViewModel()
        {
            using (var controller = CreateController())
            {
                // Arrange
                AuthorizationService.HasPermissionOnData(Arg.Any<int>(), Arg.Any<int>(), Arg.Any<long>()).Returns(true);
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

        private CancelledBetsBetListController CreateController()
        {
            var controller = Substitute.ForPartsOf<CancelledBetsBetListController>(
                LangService,
                SessionService,
                AppSettings,
                ClientViewHelper,
                AuthorizationService,
                ProductService,
                BetListService,
                BalanceUpDataAccess,
                CacheRefAdapter,
                _cancelledBetListDataAccess);
            return controller;
        }
    }
}