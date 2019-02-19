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
    using NSubstitute;
    using NUnit.Framework;
    using Report.Result;
    using Service.Core.Common.Interfaces;
    using Service.Core.Utility.Interfaces;
    using VendorAdapter.Interfaces;

    [TestFixture]
    public partial class ResultReportControllerTest : BaseControllerHelperTest
    {
        private const string HtmlResult = "<abc>hello</abc>";

        private static readonly DateTime BalanceUpDate = new DateTime(2017, 3, 4);

        private IBalanceUpDataAccess _balanceUpDataAccess;

        private IBetListApiClient _betListApiClient;

        [OneTimeSetUp]
        public override void FixtureSetUp()
        {
            base.FixtureSetUp();
            _balanceUpDataAccess = Substitute.For<IBalanceUpDataAccess>();

            _balanceUpDataAccess.GetBalanceUp().Returns(new BalanceUp
            {
                BalanceUpDay = BalanceUpDate
            });

            SetUpDefaultFakeData();
        }

        [SetUp]
        public void SetUp()
        {
            _betListApiClient = Substitute.For<IParserService>();
            _resultService = Substitute.For<IResultService>();
            _rngCasinoSingleWalletService = Substitute.For<IRNGCasinoSingleWalletService>();
        }

        [Test]
        public void MicrogamingResultPopup_ValidModelState_ResultIsCorrect()
        {
            using (var controller = CreateController())
            {
                // Arrange
                const string ExpectedData = "hello cái bô";
                const int GameId = 1;
                const int CustId = 2;
                _betListApiClient.Parser(Arg.Any<MicroGaming>()).ReturnsForAnyArgs(ExpectedData);

                // Action
                var viewResult = controller.MicrogamingResultPopup(GameId, CustId) as ViewResult;

                // Assert
                Assert.Multiple(() =>
                {
                    _betListApiClient.Received(1).Parser(Arg.Is<MicroGaming>(x =>
                        x.GameId == GameId && x.CustId == CustId &&
                        x.SiteName == SessionService.LicenseeSession.LicenseeName));
                    Assert.That(viewResult.ViewName, Is.EqualTo("~/Views/Desktop/Report/Result/Popup/Microgaming.cshtml"));
                    Assert.That(viewResult.ViewBag.Data, Is.EqualTo(ExpectedData));
                });
            }
        }

        [Test]
        public void MicrogamingResultPopup_InvalidModelState_RedirectToErrorPage()
        {
            using (var controller = CreateController())
            {
                // Arrange
                controller.ModelState.AddModelError("error", "hihi");

                // Action
                var result = controller.MicrogamingResultPopup(0, 0) as RedirectResult;

                // Assert
                Assert.That(result.Url, Is.EqualTo("~/Utility/Error?err=400"));
            }
        }

        [Test]
        public void ShowRngCasinoSingleWalletResult_WhenCall_ReturnCorrectResult()
        {
            using (var controller = CreateController())
            {
                // Arrange
                const string RefNo = "hohoho";
                const string ExpectedData = "huhuhu";
                _rngCasinoSingleWalletService.GetResultInfo(RefNo).Returns(ExpectedData);

                // Action
                var result = controller.ShowRngCasinoSingleWalletResult(RefNo) as JsonResult;

                // Assert
                Assert.That(result.Data, Is.EqualTo(ExpectedData));
            }
        }

        [Test]
        public void ResultDetail_DataIsNull_ReturnCorrectResult()
        {
            using (var controller = CreateController())
            {
                // Action
                var result = controller.ResultDetail(0, 0) as RedirectResult;

                // Assert
                Assert.That(result.Url, Is.EqualTo("~/Utility/Error?err=400"));
            }
        }

        [Test]
        public void ResultDetail_WhenCall_ReturnCorrectResult()
        {
            using (var controller = CreateController())
            {
                // Arrange
                const string ExpectedViewBagData = "Abc";
                const int MatchId = 1;
                const int SportId = (int)SportType.Soccer;
                var detailedData = new Sportsbook
                {
                    MatchId = MatchId
                };
                _resultService.GetSportsbookGameDetail(MatchId).Returns(detailedData);
                _resultService.HtmlGameDetailParser(detailedData, Arg.Any<ResultSetting>()).Returns(ExpectedViewBagData);

                // Action
                var result = controller.ResultDetail(MatchId, SportId) as ViewResult;

                // Assert
                Assert.Multiple(() =>
                {
                    _resultService.Received(1).HtmlGameDetailParser(
                        detailedData,
                        Arg.Is<ResultSetting>(x =>
                            x.SportType == (SportType)SportId &&
                            x.LanguageCode == SessionService.UserSession.Language));
                    Assert.That(result.ViewName, Is.EqualTo("~/Views/Desktop/Report/Result/Popup/Main.cshtml"));
                });
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
                SessionService.UserSession.Language,
                Arg.Any<string>()).Returns(new ProductAccess(productList.ToList()));
            var controller = new ResultReportController(
                LangService,
                SessionService,
                AppSettings,
                productService,
                _balanceUpDataAccess,
                _betListApiClient,
                _resultService,
                _rngCasinoSingleWalletService,
                Substitute.For<ICacheRefAdapter>());
            controller.ControllerContext = ControllerContext;
            return controller;
        }
    }
}