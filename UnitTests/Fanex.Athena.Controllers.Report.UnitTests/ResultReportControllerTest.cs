namespace Fanex.Athena.Controllers.TotalBets.UnitTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using Base.UnitTests;
    using DataAccess.Core.Configuration.Interfaces;
    using Model.Core.DTO.Common;
    using Model.Core.DTO.Configuration;
    using Model.Report.MetaModel;
    using Model.Report.ViewModel;
    using NSubstitute;
    using NUnit.Framework;
    using Report.Result;
    using Service.Casino.Interfaces;
    using Service.Core.Common.Interfaces;
    using Service.Result.Interfaces;

    [TestFixture]
    public class ResultReportControllerTest : BaseControllerHelperTest
    {
        private static readonly DateTime BalanceUpDate = new DateTime(2017, 3, 4);

        private IBalanceUpDataAccess _balanceUpDataAccess;

        private IResultService _resultService;

        private IRNGCasinoSingleWalletService _rngCasinoSingleWalletService;

        [OneTimeSetUp]
        public override void FixtureSetUp()
        {
            base.FixtureSetUp();
            _balanceUpDataAccess = Substitute.For<IBalanceUpDataAccess>();
            _resultService = Substitute.For<IResultService>();
            _rngCasinoSingleWalletService = Substitute.For<IRNGCasinoSingleWalletService>();
            _balanceUpDataAccess.GetBalanceUp().Returns(new BalanceUp
            {
                BalanceUpDay = BalanceUpDate
            });

            SetUpDefaultFakeData();
        }

        [TestCase("BG", true, false, false)]
        [TestCase("CS", true, false, false)]
        [TestCase("KN", false, true, false)]
        [TestCase("LT", false, false, true)]
        public void Index_WhenGet_ReturnCorrectViewResult(
            string productCodeList,
            bool isAllowRng,
            bool isAllowKeno,
            bool isAllowLottery)
        {
            using (var controller = CreateController(productCodeList))
            {
                // Action
                var viewResult = controller.Index() as ViewResult;
                var viewModel = viewResult.Model as ResultReportViewModel;

                // Assert
                Assert.Multiple(() =>
                {
                    Assert.That(viewResult.ViewName, Is.EqualTo("~/Views/Report/Result/Main.cshtml"));
                    Assert.That(viewResult.ViewBag.IsAllowedRNG, Is.EqualTo(isAllowRng));
                    Assert.That(viewResult.ViewBag.IsAllowedKeno, Is.EqualTo(isAllowKeno));
                    Assert.That(viewResult.ViewBag.IsAllowedLottery, Is.EqualTo(isAllowLottery));
                    Assert.That(viewModel.Param.BalanceUpDay, Is.EqualTo(BalanceUpDate));
                });
            }
        }

        [Test]
        public void IndexPost_ValidModelState_ResultContentIsNotEmpty()
        {
            using (var controller = CreateController())
            {
                // Action
                var viewResult = controller.Index(new ResultMetalModel()) as ContentResult;

                // Assert
                Assert.That(viewResult.Content, Is.Not.Empty);
            }
        }

        [Test]
        public void IndexPost_InvalidModelState_ResultContentIsEmpty()
        {
            using (var controller = CreateController())
            {
                // Arrange
                controller.ModelState.AddModelError("error", "hihi");

                // Action
                var viewResult = controller.Index(new ResultMetalModel()) as ContentResult;

                // Assert
                Assert.That(viewResult.Content, Is.Empty);
            }
        }

        private ResultReportController CreateController(string productCodeList = null)
        {
            var productList = string.IsNullOrEmpty(productCodeList) ?
                new List<Product>() :
                productCodeList.Split(',').Select(x => new Product
                {
                    ProductCode = x
                });
            var productService = Substitute.For<IProductService>();
            productService.GetProductAccess(
                SessionService.LicenseeSession.LicenseeId,
                SessionService.UserSession.Language).Returns(new ProductAccess(productList.ToList()));
            return new ResultReportController(
                LangService,
                SessionService,
                AppSettings,
                productService,
                _balanceUpDataAccess,
                _resultService,
                _rngCasinoSingleWalletService);
        }
    }
}