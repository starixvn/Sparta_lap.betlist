namespace Fanex.Athena.Service.Casino.UnitTests
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    using Core.Utility.Interfaces;
    using Extension.Helper;
    using Extension.Utility;
    using NSubstitute;
    using NUnit.Framework;

    public class RNGCasinoSingleWalletServiceTest
    {
        private IAppSettings _appSettings;

        private IWebClientService _webClientService;

        private RngCasinoSingleWalletService _service;

        [SetUp]
        public void SetUp()
        {
            _appSettings = Substitute.For<IAppSettings>();
            _webClientService = Substitute.For<IWebClientService>();

            _service = new RngCasinoSingleWalletService(
                Substitute.For<ICoreLogger>(),
                _appSettings,
                _webClientService);
        }

        [TearDown]
        public void TearDown()
        {
            _service = null;
        }

        [Test]
        public void GetResultInfo_GetTokenFail_ReturnErrorResult()
        {
            // Arrange
            const string RefNo = "111";
            var token = new
            {
                Result = new
                {
                    ErrorCode = 1,
                    ErrorDescription = "Error"
                }
            };
            GetRngCasinoServerTokenSetUp(token);

            // Action
            object result = _service.GetResultInfo(RefNo);
            var resultData = result.GetType().GetProperty("Result").GetValue(result, null);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.GetType().GetProperty("Success").GetValue(result, null), Is.False);
                Assert.That(resultData.GetType().GetProperty("ErrorCode").GetValue(resultData, null), Is.EqualTo(token.Result.ErrorCode.ToString()));
                Assert.That(resultData.GetType().GetProperty("ErrorDescription").GetValue(resultData, null), Is.EqualTo(token.Result.ErrorDescription.ToString()));
            });
        }

        [Test]
        public void GetResultInfo_GetTokenSuccessfully_ReturnCorrectResult()
        {
            // Arrange
            const string RefNo = "111";
            var token = new
            {
                Result = new
                {
                    ErrorCode = 0,
                    Token = "Token"
                }
            };
            GetRngCasinoServerTokenSetUp(token);
            GetRngCasinoPopupUrlSetUp();

            // Action
            var result = _service.GetResultInfo(RefNo);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.GetType().GetProperty("Success").GetValue(result, null), Is.True);
                StringAssert.Contains("RNGCasinoResultPopupUrl", result.GetType().GetProperty("Result").GetValue(result, null));
            });
        }

        private void GetRngCasinoServerTokenSetUp(object token)
        {
            var tokenString = token.ToJson();

            _appSettings.GetValue("RNGCasinoGetServerTokenAPIUrl").Returns("http://RngCasinoServerToken");
            _webClientService.UploadString(Arg.Any<Uri>(), Arg.Any<string>(), Arg.Any<IDictionary<HttpRequestHeader, string>>())
                .Returns(tokenString);

            // CreateServerTokenParameters
            _appSettings.GetValue("RNGCasinoLogin").Returns("RNGCasinoLogin");
            _appSettings.GetValue("RNGCasinoPassword").Returns("RNGCasinoPassword");
            _appSettings.GetValue("RNGCasinoSiteId").Returns("1");
            _appSettings.GetValue("RNGCasinoSeq").Returns("RNGCasinoSeq");
            _appSettings.GetValue("RNGCasinoVendorId").Returns("2");
        }

        private void GetRngCasinoPopupUrlSetUp()
        {
            // GetQueryString
            _appSettings.GetValue("RNGCasinoTemplate").Returns("RNGCasinoTemplate");
            _appSettings.GetValue("RNGCasinoRole").Returns("RNGCasinoRole");

            _appSettings.GetValue("RNGCasinoEncryptedKey").Returns("Key");
            _appSettings.GetValue("RNGCasinoResultPopupUrl").Returns("RNGCasinoResultPopupUrl");
        }
    }
}