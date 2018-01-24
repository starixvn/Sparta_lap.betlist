namespace Fanex.Athena.Controllers.TotalBets.UnitTests
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Text;
    using System.Web.Mvc;
    using Base.UnitTests;
    using DataAccess.Core.Configuration.Interfaces;
    using Model.Core.DTO.Common;
    using Model.Core.DTO.Configuration;
    using Model.Report.MetaModel;
    using NSubstitute;
    using NUnit.Framework;
    using Report.Result;
    using Resources;
    using Result.Core.Base;
    using Result.Core.MicroGaming;
    using Result.Core.MicroGaming.DTO;
    using Result.Core.Sportsbook.DTO;
    using Service.Casino.Interfaces;
    using Service.Core.Common.Interfaces;
    using Service.Result.Interfaces;

    [TestFixture]
    public partial class ResultReportControllerTest : BaseControllerHelperTest
    {
        private const string HtmlResult = "<abc>hello</abc>";

        private static readonly DateTime BalanceUpDate = new DateTime(2017, 3, 4);

        private IBalanceUpDataAccess _balanceUpDataAccess;

        private IParserService _microGamingParserService;

        private IResultService _resultService;

        private IRNGCasinoSingleWalletService _rngCasinoSingleWalletService;

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
            _microGamingParserService = Substitute.For<IParserService>();
            _resultService = Substitute.For<IResultService>();
            _rngCasinoSingleWalletService = Substitute.For<IRNGCasinoSingleWalletService>();
        }

        [Test]
        public void ResultExcel_InvalidModelState_ReturnBadRequestResponse()
        {
            using (var controller = CreateController())
            {
                // Arrange
                controller.ModelState.AddModelError("error", "hihi");
                var response = controller.HttpContext.Response;

                // Action
                controller.ResultExcel(new ResultMetalModel());

                // Assert
                Assert.Multiple(() =>
                {
                    Assert.That(response.StatusCode, Is.EqualTo((int)HttpStatusCode.BadRequest));
                    Assert.That(response.StatusDescription, Is.EqualTo(ViewResources.InvalidParams));
                });
            }
        }

        [Test]
        public void ResultExcel_ValidModelState_ReturnCorrectResponse()
        {
            // Arrange
            const string ExpectedStreamData = "Hello everyone";
            using (var controller = CreateController())
            using (var excelStream = new MemoryStream(Encoding.UTF8.GetBytes(ExpectedStreamData)))
            using (var outputStream = new MemoryStream())
            {
                var response = controller.HttpContext.Response;
                response.OutputStream.Returns(outputStream);
                _resultService.ExcelParser(new List<Sportsbook>(), null).ReturnsForAnyArgs(excelStream);

                // Action
                controller.ResultExcel(new ResultMetalModel());

                // Assert
                Assert.Multiple(() =>
                {
                    response.Received(1).AddHeader("Content-disposition", "attachment; filename=LRF-Result.xls");
                    Assert.That(response.ContentType, Is.EqualTo("application/vnd.ms-excel"));
                    Assert.That(Encoding.UTF8.GetString(((MemoryStream)response.OutputStream).ToArray()), Is.EqualTo(ExpectedStreamData));
                });
            }
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
                _microGamingParserService.Parser(Arg.Any<MicroGaming>()).ReturnsForAnyArgs(ExpectedData);

                // Action
                var viewResult = controller.MicrogamingResultPopup(GameId, CustId) as ViewResult;

                // Assert
                Assert.Multiple(() =>
                {
                    _microGamingParserService.Received(1).Parser(Arg.Is<MicroGaming>(x =>
                        x.GameId == GameId && x.CustId == CustId &&
                        x.SiteName == SessionService.LicenseeSession.LicenseeName));
                    Assert.That(viewResult.ViewName, Is.EqualTo("~/Views/Report/Result/Popup/Microgaming.cshtml"));
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
                    Assert.That(result.ViewName, Is.EqualTo("~/Views/Report/Result/Popup/Main.cshtml"));
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
                ClientViewHelper,
                productService,
                _balanceUpDataAccess,
                _microGamingParserService,
                _resultService,
                _rngCasinoSingleWalletService);
            controller.ControllerContext = ControllerContext;
            return controller;
        }
    }
}