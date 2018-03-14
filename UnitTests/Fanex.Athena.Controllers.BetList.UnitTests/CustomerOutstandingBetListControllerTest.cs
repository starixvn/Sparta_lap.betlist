namespace Fanex.Athena.Controllers.BetList.UnitTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using Model.BetList.MetaModel;
    using Model.BetList.ViewModel;
    using Model.Core.DTO.Common;
    using NSubstitute;
    using NUnit.Framework;

    public class CustomerOutstandingBetListControllerTest : BetListBaseControllerHelperTest
    {
        [OneTimeSetUp]
        public override void FixtureSetUp()
        {
            base.FixtureSetUp();

            SetUpDefaultFakeData();
        }

        [Test]
        public void Index_WhenCall_ReturnCorrectView()
        {
            using (var controller = CreateController())
            {
                // Arrange
                var metaModel = new BetListMetaModel { ProductCodeList = string.Empty };

                ProductService.GetProductAccess(Arg.Any<int>(), Arg.Any<string>(), Arg.Any<string>())
                    .Returns(new ProductAccess(new List<Product>()));

                // Action
                var result = controller.Index(metaModel) as ViewResult;

                // Assert
                Assert.That(result.ViewName, Is.EqualTo("~/Views/BetList/CustomerOutstanding/Main.cshtml"));
            }
        }

        [Test]
        public void Index_WhenFirstCall_ReturnCorrectViewModel()
        {
            using (var controller = CreateController())
            {
                // Arrange
                var metaModel = new BetListMetaModel { ProductCodeList = string.Empty };

                ProductService.GetProductAccess(Arg.Any<int>(), Arg.Any<string>(), Arg.Any<string>())
                    .Returns(new ProductAccess(new List<Product> { new Product { ProductCode = "AA" }, new Product { ProductCode = "DD" } }));

                // Action
                var result = controller.Index(metaModel) as ViewResult;
                var model = (BetListBaseViewModel)result.Model;
                metaModel.FromDate = new DateTime(DateTime.Now.Year - 1, 1, 1);

                // Assert
                Assert.Multiple(() =>
                {
                    Assert.That(model.ProductOptions.Count, Is.EqualTo(2));
                    Assert.That(model.ProductOptions.First(x => x.Code == "AA").Status, Is.True);
                    Assert.That(model.ProductOptions.First(x => x.Code == "DD").Status, Is.True);
                });
            }
        }

        [Test]
        public void Index_WhenCall_ReturnCorrectViewModel()
        {
            using (var controller = CreateController())
            {
                // Arrange
                var metaModel = new BetListMetaModel { ProductCodeList = "AA, BB" };

                ProductService.GetProductAccess(Arg.Any<int>(), Arg.Any<string>(), Arg.Any<string>())
                    .Returns(new ProductAccess(new List<Product> { new Product { ProductCode = "AA" }, new Product { ProductCode = "DD" } }));

                // Action
                var result = controller.Index(metaModel) as ViewResult;
                var model = (BetListBaseViewModel)result.Model;
                metaModel.FromDate = new DateTime(DateTime.Now.Year - 1, 1, 1);

                // Assert
                Assert.Multiple(() =>
                {
                    Assert.That(model.ProductOptions.Count, Is.EqualTo(2));
                    Assert.That(model.ProductOptions.First(x => x.Code == "AA").Status, Is.True);
                    Assert.That(model.ProductOptions.First(x => x.Code == "DD").Status, Is.False);
                });
            }
        }

        [Test]
        public void Index_RunningBetlist_ReturnCorrectViewModel()
        {
            using (var controller = CreateController())
            {
                // Arrange
                var metaModel = new BetListMetaModel { ProductCodeList = "AB, AA, BB", BetListName = "betlistrunning" };

                ProductService.GetProductAccess(Arg.Any<int>(), Arg.Any<string>(), Arg.Any<string>())
                    .Returns(new ProductAccess(new List<Product>()));

                // Action
                var result = controller.Index(metaModel) as ViewResult;
                var model = (BetListBaseViewModel)result.Model;

                // Assert
                Assert.That(model.Param.FromDate.Date, Is.Not.EqualTo(BalanceUp.MoveBetsDay.AddDays(1)));
            }
        }

        [Test]
        public void Index_IsNotRunningBetlist_ReturnCorrectViewModel()
        {
            using (var controller = CreateController())
            {
                // Arrange
                var metaModel = new BetListMetaModel { ProductCodeList = "AB, AA, BB" };

                ProductService.GetProductAccess(Arg.Any<int>(), Arg.Any<string>(), Arg.Any<string>())
                    .Returns(new ProductAccess(new List<Product>()));

                // Action
                var result = controller.Index(metaModel) as ViewResult;
                var model = (BetListBaseViewModel)result.Model;

                // Assert
                Assert.That(model.Param.FromDate.Date, Is.EqualTo(BalanceUp.MoveBetsDay.AddDays(1)));
            }
        }

        private CustomerOutstandingBetListController CreateController()
        {
            var controller = Substitute.ForPartsOf<CustomerOutstandingBetListController>(
                LangService,
                SessionService,
                AppSettings,
                ClientViewHelper,
                ProductService,
                BetListService,
                BalanceUpDataAccess,
                CacheRefAdapter);
            return controller;
        }
    }
}