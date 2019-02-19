namespace Fanex.Athena.Result.Controller.UnitTests
{
    using System;
    using System.Globalization;
    using System.Web.Mvc;
    using Common.Utility.Services.Interfaces;
    using Logging;
    using Model.Const;
    using Model.DTO.Keno;
    using Model.MetaModel.Keno;
    using NSubstitute;
    using NUnit.Framework;
    using Service.Interfaces;
    using UnitTest.Common.Helpers;

    [TestFixture]
    public class ResultKenoReportControllerTest
    {
        private const int VendorProductId = 34343;

        private const string UserId = "987343";

        private ILogger _logger;

        private IAppSettings _appSettings;

        private IKenoApi _kenoApi;

        [OneTimeSetUp]
        public void FixtureSetUp()
        {
            _logger = Substitute.For<ILogger>();
            _appSettings = Substitute.For<IAppSettings>();
            _kenoApi = Substitute.For<IKenoApi>();
        }

        [TearDown]
        public void TearDown()
        {
            _kenoApi.ClearReceivedCalls();
        }

        [Test]
        public void GetKenoResultUrl_WhenCall_ReturnCorrectResult()
        {
            using (var controller = CreateController())
            {
                // Arrange
                const string Url = "http://abc";
                _kenoApi.CreateKenoResultUrl(Arg.Any<KenoResultMetaModel>(), UserId, Arg.Any<DateTime>()).ReturnsForAnyArgs(Url);
                _appSettings.GetValue(Arg.Any<string>()).ReturnsForAnyArgs(x => x[0]);

                // Action
                var result = controller.GetKenoResultUrl(VendorProductId, UserId);

                // Assert
                Assert.That(result, Is.EqualTo(Url));
            }
        }

        [TestCase(ResultVendorProductTypes.Keno, "KenoResultUrl")]
        [TestCase(ResultVendorProductTypes.Lottery, "LotteryResultUrl")]
        public void GetKenoResultUrl_WhenCall_PassCorrectParams(int vendorProductId, string expectedResultUrl)
        {
            using (var controller = CreateController())
            {
                // Arrange
                _appSettings.GetValue(Arg.Any<string>()).ReturnsForAnyArgs(x => x[0]);

                // Action
                var result = controller.GetKenoResultUrl(vendorProductId, UserId);

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
                    UserId,
                    Arg.Any<DateTime>());
            }
        }

        [Test]
        public void KeepSession_WhenCall_ReturnResultFromApi()
        {
            using (var controller = CreateController())
            {
                // Arrange
                var expectedResult = new KenoExtendedToken
                {
                    ErrorCode = 12
                };

                _kenoApi.ExtendToken(Arg.Any<KenoResultMetaModel>(), UserId).Returns(expectedResult);

                // Action
                var result = controller.KeepSession(VendorProductId, UserId) as JsonResult;

                // Assert
                Assert.That((result.Data as KenoExtendedToken).ErrorCode, Is.EqualTo(expectedResult.ErrorCode));
            }
        }

        private ResultKenoReportController CreateController()
        {
            var controller = new ResultKenoReportController(
                _logger,
                _appSettings,
                _kenoApi);
            controller.ControllerContext = MvcHelper.CreateControllerContext();
            controller.Url = MvcHelper.CreateUrlHelper(controller.ControllerContext.HttpContext);
            return controller;
        }
    }
}