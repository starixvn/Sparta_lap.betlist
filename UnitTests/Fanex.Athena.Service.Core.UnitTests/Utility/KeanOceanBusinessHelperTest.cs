namespace Fanex.Athena.Service.Core.UnitTests.Utility
{
    using System.Collections.Generic;
    using System.Linq;
    using Fanex.Athena.Model.Core.DataDomain;
    using Fanex.Athena.Model.Core.DTO.Users;
    using Fanex.Athena.Service.Core.Utility;
    using NUnit.Framework;

    [TestFixture]
    public class KeanOceanBusinessHelperTest
    {
        [Test]
        public void ModifyCurrenciesForKeenOcean_ContainCNYCurrency_ReturnCurrencyListWithoutCNY()
        {
            // Arrange
            var currencyList = new List<CurrencyUser>
            {
                new CurrencyUser { CurrencyId = CurrencyType.Cny, CurrencyName = "CNY" },
                new CurrencyUser { CurrencyId = 1, CurrencyName = "test" }
            };

            // Action
            var temp = KeanOceanBusinessHelper.ModifyCurrenciesForKeenOcean(currencyList);
            var expectedResult = temp.Any(i => i.CurrencyId == CurrencyType.Cny);

            // Assert
            Assert.True(expectedResult == false);
        }

        [Test]
        public void ModifyCurrenciesForKeenOcean_ContainHKDCurrency_ChangeCurrencyNameOfHKDToCNY()
        {
            // Arrange
            var currencyList = new List<CurrencyUser>
            {
                new CurrencyUser { CurrencyId = CurrencyType.Cny, CurrencyName = "CNY" },
                new CurrencyUser { CurrencyId = 1, CurrencyName = "test" },
                new CurrencyUser { CurrencyId = CurrencyType.Hkd, CurrencyName = "HKD" }
            };

            var expectedResult = new CurrencyUser { CurrencyId = CurrencyType.Hkd, CurrencyName = "CNY" };

            // Action
            var temp = KeanOceanBusinessHelper.ModifyCurrenciesForKeenOcean(currencyList);
            var currency = temp.SingleOrDefault(i => i.CurrencyId == CurrencyType.Hkd);

            // Assert
            Assert.NotNull(currency);
            Assert.AreEqual(expectedResult.CurrencyName, currency.CurrencyName);
        }

        [Test]
        public void KeepHKDCurrencyName_ContainHKDCurrency_HDKCurrency_CurrenyName_Equal_HKD()
        {
            // Arrange
            var currencyList = new List<CurrencyUser>
            {
                new CurrencyUser { CurrencyId = CurrencyType.Cny, CurrencyName = "CNY" },
                new CurrencyUser { CurrencyId = CurrencyType.Hkd, CurrencyName = "CNY" }
            };
            var expectedResult = "HKD";

            // Action
            KeanOceanBusinessHelper.KeepHKDCurrencyName(currencyList);
            var currency = currencyList.SingleOrDefault(i => i.CurrencyId == CurrencyType.Hkd);

            // Assert
            Assert.NotNull(currency);
            Assert.AreEqual(expectedResult, currency.CurrencyName);
        }

        [TestCase(1, CurrencyType.Cny, false)]
        [TestCase(1, CurrencyType.Hkd, true)]
        [TestCase(1, 0, false)]
        [TestCase(LicenseeIdentityDefinition.KeenOcean, CurrencyType.Cny, true)]
        [TestCase(LicenseeIdentityDefinition.KeenOcean, CurrencyType.Hkd, true)]
        [TestCase(LicenseeIdentityDefinition.KeenOcean, 1000, false)]
        public void VerifyKeanOceanCurrencyRequest_CurrencyByLicensee_ReturnExpectedValue(int licenseeId, int currencyId, bool expectedResult)
        {
            // Arrange
            var currencyList = new Dictionary<int, string>();
            currencyList.Add(CurrencyType.Hkd, "HKD");
            currencyList.Add(1, "USD");

            // Action
            var result = KeanOceanBusinessHelper.VerifyKeanOceanCurrencyRequest(currencyList, licenseeId, currencyId);

            // Assert
            Assert.AreEqual(expectedResult, result);
        }

        [Test]
        public void AddHKDCurrencyForTurnOverReport_CurrencyLisNotContainCNY_ReturnCurrenyListWithHKD()
        {
            // Arrange
            var currencyList = new Dictionary<int, string>();
            currencyList.Add(1, "USD");

            // Action
            KeanOceanBusinessHelper.AddHKDCurrencyForTurnOverReport(currencyList, LicenseeIdentityDefinition.KeenOcean);

            // Assert
            Assert.True(currencyList.Any(i => i.Key == CurrencyType.Hkd));
        }
    }
}