namespace Fanex.Athena.Controllers.TotalBets.UnitTests
{
    using System;
    using System.Globalization;
    using System.Web.Mvc;
    using System.Web.Script.Serialization;
    using Base.UnitTests;
    using Logging;
    using Model.Core.DataDomain;
    using Model.Report.DTO.Keno;
    using Model.Report.MetaModel.Keno;
    using NSubstitute;
    using NUnit.Framework;
    using Report.Result;
    using Service.Core.Common.Interfaces;
    using Service.Report.Result.Interfaces;

    [TestFixture]
    public class ResultKenoReportControllerTest : BaseControllerHelperTest
    {
        private ILogger _logger;

        private IKenoApi _kenoApi;

        [OneTimeSetUp]
        public override void FixtureSetUp()
        {
            _logger = Substitute.For<ILogger>();
            _kenoApi = Substitute.For<IKenoApi>();
            base.FixtureSetUp();

            SetUpDefaultFakeData();
        }

        [TearDown]
        public void TearDown()
        {
            _kenoApi.ClearReceivedCalls();
        }

        [Test]
        public void Index_WhenCall_ReturnCorrectResult()
        {
            using (var controller = CreateController())
            {
                // Arrange
                const string Url = "http://abc";
                _kenoApi.CreateKenoResultUrl(null, null, DateTime.MinValue).ReturnsForAnyArgs(Url);
                AppSettings.GetValue(Arg.Any<string>()).ReturnsForAnyArgs(x => x[0]);

                // Action
                var result = controller.Index(0) as JsonResult;

                // Assert
                Assert.That(new JavaScriptSerializer().Serialize(result.Data), Is.EqualTo(@"{""Url"":""http://abc"",""UserId"":1}"));
            }
        }

        [TestCase(ResultVendorProductTypes.Keno, "KenoResultUrl")]
        [TestCase(ResultVendorProductTypes.Lottery, "LotteryResultUrl")]
        public void Index_WhenCall_PassCorrectParams(int vendorProductId, string expectedResultUrl)
        {
            using (var controller = CreateController())
            {
                // Arrange
                AppSettings.GetValue(Arg.Any<string>()).ReturnsForAnyArgs(x => x[0]);

                // Action
                var result = controller.Index(vendorProductId) as JsonResult;

                // Assert
                _kenoApi.CreateKenoResultUrl(
                    Arg.Is<KenoResultMetaModel>(x =>
                        x.Language == CultureInfo.CurrentUICulture.TwoLetterISOLanguageName &&
                        x.SiteID == "KenoSiteID" &&
                        x.AuthorizedUrl == "KenoAuthorizedUrl" &&
                        x.ExpireTokenUrl == "KenoExpireTokenUrl" &&
                        x.ExtendTokenUrl == "KenoExtendTokenUrl" &&
                        x.VenderKey == "KenoVenderKey" &&
                        x.Vendor == "KenoVendor" &&
                        x.ResultUrl == expectedResultUrl &&
                        x.KickOutUrl == "abc" &&
                        x.VendorProduct == vendorProductId),
                    SessionService.UserSession.UserId.ToString(),
                    Arg.Any<DateTime>());
            }
        }

        [TestCase(null)]
        [TestCase("")]
        [TestCase("999")]
        public void KeepSession_InvalidUserId_ReturnAccessDenied(string userId)
        {
            using (var controller = CreateController())
            {
                // Arrange
                const string AccessDeniedUrl = "AccessDeniedUrl";
                controller.Url.Action("AccessDenied", "Utility", null, controller.Request.Url.Scheme)
                    .Returns(AccessDeniedUrl);

                // Action
                var result = controller.KeepSession(userId, 0);

                // Assert
                Assert.That(result, Is.EqualTo(AccessDeniedUrl));
            }
        }

        [Test]
        public void KeepSession_ApiReturnError_ReturnAccessDenied()
        {
            using (var controller = CreateController())
            {
                // Arrange
                var userId = SessionService.UserSession.UserId.ToString();
                const string AccessDeniedUrl = "AccessDeniedUrl";
                controller.Url.Action("AccessDenied", "Utility", null, controller.Request.Url.Scheme)
                    .Returns(AccessDeniedUrl);
                _kenoApi.ExtendToken(Arg.Any<KenoResultMetaModel>(), userId)
                    .Returns(new KenoExtendedToken
                    {
                        ErrorCode = 1
                    });

                // Action
                var result = controller.KeepSession(userId, 0);

                // Assert
                Assert.Multiple(() =>
                {
                    _kenoApi.Received(1).ExtendToken(Arg.Any<KenoResultMetaModel>(), userId);
                    Assert.That(result, Is.EqualTo(AccessDeniedUrl));
                });
            }
        }

        [Test]
        public void KeepSession_ApiSuccess_ReturnEmptyString()
        {
            using (var controller = CreateController())
            {
                // Arrange
                var userId = SessionService.UserSession.UserId.ToString();
                _kenoApi.ExtendToken(Arg.Any<KenoResultMetaModel>(), userId)
                    .Returns(new KenoExtendedToken());

                // Action
                var result = controller.KeepSession(userId, 0);

                // Assert
                Assert.Multiple(() =>
                {
                    _kenoApi.Received(1).ExtendToken(Arg.Any<KenoResultMetaModel>(), userId);
                    Assert.That(result, Is.EqualTo(string.Empty));
                });
            }
        }

        private ResultKenoReportController CreateController()
        {
            var controller = new ResultKenoReportController(
                LangService,
                SessionService,
                AppSettings,
                ClientViewHelper,
                Substitute.For<IProductService>(),
                _logger,
                _kenoApi);
            controller.ControllerContext = ControllerContext;
            controller.Url = Substitute.ForPartsOf<UrlHelper>(ControllerContext.RequestContext);
            return controller;
        }
    }
}