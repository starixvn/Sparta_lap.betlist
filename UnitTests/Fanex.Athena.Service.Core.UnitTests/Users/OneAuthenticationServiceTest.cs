namespace Fanex.Athena.Service.Core.UnitTests.Users
{
    using System.Collections.Generic;
    using System.IO;
    using Fanex.Athena.Service.Core.Users;
    using Model.Core.DTO.Common;
    using Newtonsoft.Json;
    using NSubstitute;
    using NUnit.Framework;

    [TestFixture]
    public class OneAuthenticationServiceTest
    {
        private OneAuthenticationService _oneAuthenticationService;

        [OneTimeSetUp]
        public void SetUp()
        {
            _oneAuthenticationService = Substitute.ForPartsOf<OneAuthenticationService>();
            _oneAuthenticationService.When(x => x.GetStreamReader(null)).DoNotCallBase();
            _oneAuthenticationService.GetStreamReader(null).Returns(new FakeReader());
        }

        #region SingleLicenseeSiteName
        [TestCase("", 0)]
        [TestCase(null, 0)]
        [TestCase("12 bet", 0)]
        public void SingleLicenseeSiteName_InvalidSiteName_ReturnDefatulValue(string siteName, int expectedLicenseeId)
        {
            ///Action
            var result = _oneAuthenticationService.SingleLicenseeSiteName(siteName);

            ///Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(result.LicenseeID, expectedLicenseeId);
        }

        [TestCase("hello", 0)]
        public void SingleLicenseeSiteName_NotExistedSiteName_ReturnDefatulValue(string siteName, int expectedLicenseeId)
        {
            ///Action
            var result = _oneAuthenticationService.SingleLicenseeSiteName(siteName);

            ///Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(result.LicenseeID, expectedLicenseeId);
        }

        [TestCase(1, "12bet")]
        public void SingleLicenseeSiteName_ExitedSiteName_ReturnValidLicenseeSiteName(int licenseeId, string siteName)
        {
            ///Action
            var result = _oneAuthenticationService.SingleLicenseeSiteName(siteName);

            ///Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(result.SiteName, siteName);
            Assert.AreEqual(result.LicenseeID, licenseeId);
        }
        #endregion

        #region GetTokenInfo

        #endregion
        private class FakeReader : TextReader
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