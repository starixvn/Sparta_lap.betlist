namespace Fanex.Athena.Service.Core.UnitTests.Users
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using Caching;
    using Fanex.Athena.Service.Core.Users;
    using Model.Core.DTO.Common;
    using Newtonsoft.Json;
    using NSubstitute;
    using NUnit.Framework;

    [TestFixture]
    public class OOneAuthenticationService_SingleLicenseeSiteNameTest
    {
        private OneAuthenticationService _oneAuthenticationService;
        private ICache _cacheService;
        private TextReader _fakeTextReader;
        private int _cacheCounter;

        [SetUp]
        public void SetUp()
        {
            _fakeTextReader = new FakeSiteNameReader();
            _cacheService = Substitute.For<ICache>();
            _cacheService.Get<IList<LicenseeSiteName>>(Arg.Is<string>("OneAuthenticationService_LicenseeSiteNameCacheKey")).Returns(x => null);
            _cacheService.When(x => x.Set<IList<LicenseeSiteName>>(Arg.Any<string>(), Arg.Any<IList<LicenseeSiteName>>(), Arg.Any<DateTime>()))
                .DoNotCallBase();

            _cacheService.When(x => x.Set<IList<LicenseeSiteName>>(Arg.Any<string>(), Arg.Any<IList<LicenseeSiteName>>(), Arg.Any<DateTime>()))
                .Do(x => _cacheCounter++);

            _oneAuthenticationService = Substitute.ForPartsOf<OneAuthenticationService>();
            _oneAuthenticationService.GetStringFromFile(Arg.Any<string>()).Returns(_fakeTextReader.ReadToEnd());
            _oneAuthenticationService.When(x => x.GetMemCacheService()).DoNotCallBase();
            _oneAuthenticationService.GetMemCacheService().Returns(_cacheService);
        }

        [TearDown]
        public void TearDown()
        {
            _cacheService = null;
            _fakeTextReader = null;
            _oneAuthenticationService = null;
            _cacheCounter = 0;
        }

        #region SingleLicenseeSiteName

        [TestCase("", 0)]
        [TestCase(null, 0)]
        [TestCase("12 bet", 0)]
        public void SingleLicenseeSiteName_InvalidSiteName_ReturnDefatulValue(string siteName, int expectedLicenseeId)
        {
            // Action
            var result = _oneAuthenticationService.SingleLicenseeSiteName(siteName);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(result.LicenseeID, expectedLicenseeId);
        }

        [TestCase("hello", 0)]
        public void SingleLicenseeSiteName_NotExistedSiteName_ReturnDefatulValue(string siteName, int expectedLicenseeId)
        {
            // Action
            var result = _oneAuthenticationService.SingleLicenseeSiteName(siteName);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(result.LicenseeID, expectedLicenseeId);
        }

        [TestCase(1, "12bet")]
        public void SingleLicenseeSiteName_ExitedSiteName_ReturnValidLicenseeSiteName(int licenseeId, string siteName)
        {
            // Action
            var result = _oneAuthenticationService.SingleLicenseeSiteName(siteName);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(result.SiteName, siteName);
            Assert.AreEqual(result.LicenseeID, licenseeId);
        }

        [TestCase("spondemo")]
        [TestCase("12bet")]
        public void SingleLicenseeSiteName_AnySiteName_SetDataToCache(string siteName)
        {
            // Action
            var result = _oneAuthenticationService.SingleLicenseeSiteName(siteName);

            // Assert
            Assert.AreEqual(1, _cacheCounter);
        }

        [TestCase("12bet44")]
        public void SingleLicenseeSiteName_AnySiteName_GetDataFromCache(string siteName)
        {
            // Arrange
            var data = new List<LicenseeSiteName>
                {
                    new LicenseeSiteName { LicenseeID = 1, SiteName = "12bet44" },
                    new LicenseeSiteName { LicenseeID = 1, SiteName = "spondemo222" }
                };
            _cacheService.Get<IList<LicenseeSiteName>>(Arg.Is<string>("OneAuthenticationService_LicenseeSiteNameCacheKey")).Returns(data);

            // Action
            var result = _oneAuthenticationService.SingleLicenseeSiteName(siteName);

            // Assert
            _cacheService.DidNotReceive().Set(Arg.Any<string>(), Arg.Any<IList<LicenseeSiteName>>(), Arg.Any<DateTime>());
        }

        #endregion SingleLicenseeSiteName

        private class FakeSiteNameReader : TextReader
        {
            public override string ReadToEnd()
            {
                var items = new List<LicenseeSiteName>
                {
                    new LicenseeSiteName { LicenseeID = 1, SiteName = "12bet" },
                    new LicenseeSiteName { LicenseeID = 1, SiteName = "spondemo" }
                };
                return JsonConvert.SerializeObject(items);
            }
        }
    }
}