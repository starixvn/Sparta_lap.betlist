namespace Fanex.Athena.Service.Core.UnitTests.Common
{
    using System.Collections.Generic;
    using Core.Common;
    using Core.Common.Interfaces;
    using DataAccess.Core.Common.Interfaces;
    using Model.Core.DataDomain;
    using Model.Core.DTO.Common;
    using Model.Core.DTO.Users;
    using Model.Core.MetaModel.Account;
    using NSubstitute;
    using NUnit.Framework;

    [TestFixture]
    public class CurrencyServiceTest
    {
        private ICurrencyDataAccess _currencyDataAccess;
        private ICurrencyService _currencyService;

        [OneTimeSetUp]
        public void SetUp()
        {
            _currencyDataAccess = Substitute.For<ICurrencyDataAccess>();
            _currencyService = new CurrencyService(_currencyDataAccess);
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            _currencyDataAccess = null;
            _currencyService = null;
        }

        [Test]
        public void GetCurrencies_WhenCall_ReturnDataFromDataAccess()
        {
            // Arrange
            var expectedResult = new List<CurrencyUser>
            {
                new CurrencyUser { CurrencyId = 1 }
            };

            const int LicenseeId = 1;

            _currencyDataAccess.GetCurrencies(LicenseeId).Returns(expectedResult);

            // Action
            var result = _currencyService.GetCurrencies(LicenseeId);

            // Assert
            Assert.That(result[0].CurrencyId, Is.EqualTo(expectedResult[0].CurrencyId));
        }

        [Test]
        public void GetCurrencyMapByUser_WhenCall_ReturnActiveCurrencyUserList()
        {
            // Arrange
            var expectedResult = new List<CurrencyUser>
            {
                new CurrencyUser { CurrencyName = "USD", IsActive = true },
                new CurrencyUser { CurrencyName = "UUS", IsActive = false }
            };

            _currencyDataAccess.GetCurrenciesByUser(Arg.Any<int>(), Arg.Any<int>()).Returns(expectedResult);

            // Action
            var result = _currencyService.GetCurrencyMapByUser(0, 0);

            // Assert
            Assert.That(result, Contains.Key("USD"));
            Assert.That(result, Does.Not.Contains("UUS"));
        }

        [Test]
        public void GetCurrenciesByUser_LicenseeIsKeenOcean_ReturnResultFromDataAccess()
        {
            // Arrange
            var expectedResult = new List<CurrencyUser>
            {
                new CurrencyUser { CurrencyName = "HKD", CurrencyId = CurrencyType.Hkd, IsActive = true },
                new CurrencyUser { CurrencyName = "CNY", CurrencyId = CurrencyType.Cny, IsActive = true }
            };

            const int licenseeId = LicenseeIdentityDefinition.KeenOcean;
            const int userId = 2;
            _currencyDataAccess.GetCurrenciesByUser(licenseeId, userId).Returns(expectedResult);

            // Action
            var result = _currencyService.GetCurrencyMapByUser(licenseeId, userId);

            // Assert
            Assert.That(result.Count, Is.EqualTo(1));
            Assert.That(result["CNY"], Is.EqualTo(CurrencyType.Hkd));
        }

        [Test]
        public void GetCurrenciesByUser_WhenCall_ReturnResultFromDataAccess()
        {
            // Arrange
            var expectedResult = new List<CurrencyUser>
            {
                new CurrencyUser { CurrencyName = "USD" },
                new CurrencyUser { CurrencyName = "UUS" }
            };

            const int licenseeId = 1;
            const int userId = 2;
            _currencyDataAccess.GetCurrenciesByUser(licenseeId, userId).Returns(expectedResult);

            // Action
            var result = _currencyService.GetCurrenciesByUser(licenseeId, userId);

            // Assert
            Assert.That(result[0].CurrencyName, Is.EqualTo(expectedResult[0].CurrencyName));
            Assert.That(result[1].CurrencyName, Is.EqualTo(expectedResult[1].CurrencyName));
        }

        [Test]
        public void AssignCurrenciesByUser_WhenCall_ReturnResultFromDataAccess()
        {
            // Arrange
            var expectedResult = new ResultInfo
            {
                Code = 1,
                Message = "WTF"
            };

            _currencyDataAccess.AssignCurrenciesByUser(Arg.Any<AssignCurrencyMetaModel>()).Returns(expectedResult);

            // Action
            var result = _currencyService.AssignCurrenciesByUser(null);

            // Assert
            Assert.That(result.Code, Is.EqualTo(expectedResult.Code));
            Assert.That(result.Message, Is.EqualTo(expectedResult.Message));
        }
    }
}