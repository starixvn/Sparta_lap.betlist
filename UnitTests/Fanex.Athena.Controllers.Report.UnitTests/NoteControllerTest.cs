namespace Fanex.Athena.Controllers.TotalBets.UnitTests
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Base.UnitTests;
    using DataAccess.Core.Configuration.Interfaces;
    using Model.Core.DTO.Common;
    using Model.Core.DTO.Configuration;
    using Model.Report.ViewModel.Note;
    using NSubstitute;
    using NUnit.Framework;
    using Report;
    using Service.Core.Common.Interfaces;

    [TestFixture]
    public class NoteControllerTest : CultureBaseControllerHelperTest
    {
        private IBalanceUpDataAccess _balanceUpDataAccess;
        private IProductService _productService;

        [OneTimeSetUp]
        public override void FixtureSetUp()
        {
            base.FixtureSetUp();
            _balanceUpDataAccess = Substitute.For<IBalanceUpDataAccess>();
            _productService = Substitute.For<IProductService>();
            SetUpDefaultFakeData();
        }

        [Test]
        public async Task Index_WhenCall_ReturnCorrectViewResult()
        {
            using (var controller = CreateController())
            {
                // Arrange
                var moveBetsDate = new DateTime(2017, 1, 1);
                const string ViewName = "ABC";
                _balanceUpDataAccess.GetBalanceUp().Returns(new BalanceUp
                {
                    MoveBetsDay = moveBetsDate
                });
                var productList = new List<Product>
                {
                    new Product()
                };
                _productService.GetProductList(
                    SessionService.LicenseeSession.LicenseeId,
                    SessionService.UserSession.Language).Returns(productList);

                // Action
                var viewResult = await controller.Index(ViewName) as ViewResult;
                var viewModel = viewResult.Model as NoteViewModel;

                // Assert
                Assert.Multiple(() =>
                {
                    Assert.That(viewResult.ViewName, Is.EqualTo($"~/Views/Note/{ViewName}.cshtml"));
                    Assert.That(viewModel.MoveBetsDate, Is.EqualTo(moveBetsDate));
                    Assert.That(viewModel.ProductList, Is.EqualTo(productList));
                });
            }
        }

        private NoteController CreateController()
        {
            return new NoteController(
                LangService,
                SessionService,
                AppSettings,
                _productService,
                _balanceUpDataAccess);
        }
    }
}