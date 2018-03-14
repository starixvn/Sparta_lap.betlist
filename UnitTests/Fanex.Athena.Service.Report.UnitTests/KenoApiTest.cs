namespace Fanex.Athena.Service.Report.UnitTests
{
    using System;
    using Core.Exception;
    using Core.Utility.Interfaces;
    using Model.Core.DataDomain;
    using Model.Report.DTO.Keno;
    using Model.Report.MetaModel.Keno;
    using NSubstitute;
    using NUnit.Framework;
    using Result;

    public class KenoApiTest
    {
        private IWebClientService _webClient;
        private KenoApi _service;

        [SetUp]
        public void SetUp()
        {
            _webClient = Substitute.For<IWebClientService>();
            _service = Substitute.ForPartsOf<KenoApi>(_webClient);
        }

        [TearDown]
        public void TearDown()
        {
            _webClient = null;
            _service = null;
        }

        [Test]
        public void ExtendToken_ExtendTokenUrlNull_ThrowException()
        {
            // Arrange
            var setting = new KenoResultMetaModel();

            // Action
            TestDelegate testDelegate = () => _service.ExtendToken(setting, string.Empty);

            // Assert
            var exception = Assert.Throws<ServiceException>(testDelegate);
            Assert.That(exception.Message.Contains("Value cannot be null"), Is.True);
        }

        [Test]
        public void ExtendToken_ApiReturnEmpty_ThrowException()
        {
            // Arrange
            const string ExtendTokenUrl = "hehe";
            var setting = new KenoResultMetaModel
            {
                ExtendTokenUrl = ExtendTokenUrl
            };

            // Action
            TestDelegate testDelegate = () => _service.ExtendToken(setting, string.Empty);

            // Assert
            var exception = Assert.Throws<ServiceException>(testDelegate);
            Assert.That(exception.Message.Contains("Couldn't get authorized data from: " + ExtendTokenUrl), Is.True);
        }

        [Test]
        public void ExtendToken_WhenCall_ReturnDataFromApi()
        {
            // Arrange
            const string UserId = "1234";
            const int ExpectedErrorCode = 888;
            const string ExpectedErrorMsg = "Noway";
            var setting = new KenoResultMetaModel
            {
                Vendor = "abc",
                VenderKey = "def",
                VendorProduct = ResultVendorProductTypes.Keno,
                ExtendTokenUrl = "AAAA{0}{1}{2}{3}"
            };
            var extendedUrl = string.Format(setting.ExtendTokenUrl, setting.Vendor, setting.VenderKey, UserId, setting.VendorProduct);
            _webClient.DownloadString(extendedUrl).Returns($"{{ErrorCode: {ExpectedErrorCode}, ErrorMsg: \"{ExpectedErrorMsg}\"}}");

            // Action
            var result = _service.ExtendToken(setting, UserId);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.ErrorCode, Is.EqualTo(ExpectedErrorCode));
                Assert.That(result.ErrorMsg, Is.EqualTo(ExpectedErrorMsg));
            });
        }

        [Test]
        public void CreateKenoResultUrl_AuthorizeFail_ThrowException()
        {
            // Arrange
            const string UserId = "heneken";
            const string ExpectedErrorMsg = "Whatthehell";
            var setting = new KenoResultMetaModel();
            _service.WhenForAnyArgs(x => x.Authorize(null, null)).DoNotCallBase();
            _service.Authorize(setting, UserId).Returns(new KenoAuthorization
            {
                ErrorCode = 11,
                ErrorMsg = ExpectedErrorMsg
            });

            // Action
            TestDelegate testDelegate = () => _service.CreateKenoResultUrl(setting, UserId, DateTime.MinValue);

            // Assert
            var exception = Assert.Throws<ServiceException>(testDelegate);
            Assert.That(exception.Message.Contains(ExpectedErrorMsg), Is.True);
        }

        [Test]
        public void CreateKenoResultUrl_WhenCall_ReturnResultUrl()
        {
            // Arrange
            const string UserId = "heneken";
            var setting = new KenoResultMetaModel
            {
                SiteID = "grabbike",
                VendorProduct = ResultVendorProductTypes.Keno,
                KickOutUrl = "xxx.com"
            };
            _service.WhenForAnyArgs(x => x.Authorize(null, null)).DoNotCallBase();
            _service.Authorize(setting, UserId).Returns(new KenoAuthorization
            {
                Data = new KenoAuthorizedData {  Authorization = "uber" }
            });

            // Action
            var resultUrl = _service.CreateKenoResultUrl(setting, UserId, DateTime.MinValue);

            // Assert
            Assert.That(resultUrl, Is.EqualTo("?Authorization=uber&cust=Keno&siteID=grabbike&action=R&lang=&date=00010101&redirectURL=xxx.com"));
        }

        [Test]
        public void Authorize_WhenCall_ReturnResultUrl()
        {
            // Arrange
            const string UserId = "heneken";
            var setting = new KenoResultMetaModel
            {
                SiteID = "grabbike",
                VendorProduct = ResultVendorProductTypes.Keno,
                KickOutUrl = "xxx.com"
            };
            _service.WhenForAnyArgs(x => x.Authorize(null, null)).DoNotCallBase();
            _service.Authorize(setting, UserId).Returns(new KenoAuthorization
            {
                Data = new KenoAuthorizedData { Authorization = "uber" }
            });

            // Action
            var resultUrl = _service.CreateKenoResultUrl(setting, UserId, DateTime.MinValue);

            // Assert
            Assert.That(resultUrl, Is.EqualTo("?Authorization=uber&cust=Keno&siteID=grabbike&action=R&lang=&date=00010101&redirectURL=xxx.com"));
        }

        [Test]
        public void Authorize_ExtendTokenUrlNull_ThrowException()
        {
            // Arrange
            var setting = new KenoResultMetaModel();

            // Action
            TestDelegate testDelegate = () => _service.Authorize(setting, string.Empty);

            // Assert
            var exception = Assert.Throws<ServiceException>(testDelegate);
            Assert.That(exception.Message.Contains("Value cannot be null"), Is.True);
        }

        [Test]
        public void Authorize_ApiReturnEmpty_ThrowException()
        {
            // Arrange
            var setting = new KenoResultMetaModel
            {
                AuthorizedUrl = "hehe"
            };

            // Action
            TestDelegate testDelegate = () => _service.Authorize(setting, string.Empty);

            // Assert
            var exception = Assert.Throws<ServiceException>(testDelegate);
            Assert.That(exception.Message.Contains("Couldn't get authorized data from: " + setting.AuthorizedUrl), Is.True);
        }

        [Test]
        public void Authorize_WhenCall_ReturnDataFromApi()
        {
            // Arrange
            const string UserId = "1234";
            const int ExpectedErrorCode = 888;
            const string ExpectedErrorMsg = "Noway";
            var setting = new KenoResultMetaModel
            {
                Vendor = "abc",
                VenderKey = "def",
                VendorProduct = ResultVendorProductTypes.Keno,
                AuthorizedUrl = "AAAA{0}{1}{2}{3}"
            };
            var extendedUrl = string.Format(setting.AuthorizedUrl, setting.Vendor, setting.VenderKey, UserId, setting.VendorProduct);
            _webClient.DownloadString(extendedUrl).Returns($"{{ErrorCode: {ExpectedErrorCode}, ErrorMsg: \"{ExpectedErrorMsg}\"}}");

            // Action
            var result = _service.Authorize(setting, UserId);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.ErrorCode, Is.EqualTo(ExpectedErrorCode));
                Assert.That(result.ErrorMsg, Is.EqualTo(ExpectedErrorMsg));
            });
        }
    }
}