namespace Fanex.Athena.Service.Core.Tests.Common
{
    using System.Collections.Generic;
    using DataAccess.Core.Users.Interfaces;
    using Model.Core.DTO.Common;
    using Model.Core.DTO.Users;
    using NSubstitute;
    using NUnit.Framework;
    using Users;
    using Users.Interfaces;

    [TestFixture]
    public class LicenseeServiceTest
    {
        private ILicenseeDataAccess _licenseeDataAccess;
        private ILicenseeService _licenseeService;

        [SetUp]
        public void SetUp()
        {
            _licenseeDataAccess = Substitute.For<ILicenseeDataAccess>();
            _licenseeService = new LicenseeService(_licenseeDataAccess);
        }

        [Test]
        public void IsAllowUpdateMemberBAStatus_WhenCall_ReturnResultFromDataAccess()
        {
            // Arrange
            var expectedResult = new ResultInfo()
            {
                Code = 1
            };
            var licenseeId = 1;
            var isAllow = false;
            _licenseeDataAccess.IsAllowUpdateMemberBAStatus(Arg.Any<Request>(), licenseeId, out isAllow).Returns(expectedResult);

            // Action
            var result = _licenseeService.IsAllowUpdateMemberBAStatus(new Request(), licenseeId, out isAllow);

            // Assert
            Assert.That(result.Code, Is.EqualTo(expectedResult.Code));
        }

        [TestCase(1, true)]
        [TestCase(2, false)]
        public void IsLicenseeBelongToUser_WhenCall_ReturnCorrectResult(int licenseeId, bool expectedResult)
        {
            // Arrange
            var licensees = new List<Licensee>()
            {
                new Licensee() { Id = 1 }
            };
            var userId = 1;
            _licenseeDataAccess.GetByUser(userId).Returns(licensees);

            // Action
            var result = _licenseeService.IsLicenseeBelongToUser(licenseeId, userId);

            // Assert
            Assert.That(result, Is.EqualTo(expectedResult));
        }

        [Test]
        public void GetByUser_WhenCall_ReturnDataFromDataAccess()
        {
            // Arrange
            var expectedResult = new List<Licensee>()
            {
                new Licensee() { Id = 1 }
            };
            var userId = 1;
            _licenseeDataAccess.GetByUser(userId).Returns(expectedResult);

            // Action
            var result = _licenseeService.GetByUser(userId);

            // Assert
            Assert.That(result[0].Id, Is.EqualTo(expectedResult[0].Id));
        }

        [Test]
        public void GetCurrencies_WhenCall_ReturnDataFromDataAccess()
        {
            // Arrange
            var expectedResult = new List<Currency>()
            {
                new Currency() { CurrencyId = 1 }
            };
            var licenseeId = 1;
            var userId = 1;
            _licenseeDataAccess.GetCurrencies(licenseeId, userId).Returns(expectedResult);

            // Action
            var result = _licenseeService.GetCurrencies(licenseeId, userId);

            // Assert
            Assert.That(result[0].CurrencyId, Is.EqualTo(expectedResult[0].CurrencyId));
        }

        [Test]
        public void GetCustByLicensee_WhenCall_ReturnDataFromDataAccess()
        {
            // Arrange
            var expectedResult = new List<LicenseeCustomer>()
            {
                new LicenseeCustomer() { Id = 1 }
            };
            var licenseeId = 1;
            _licenseeDataAccess.GetCustByLicensee(licenseeId).Returns(expectedResult);

            // Action
            var result = _licenseeService.GetCustByLicensee(licenseeId);

            // Assert
            Assert.That(result[0].Id, Is.EqualTo(expectedResult[0].Id));
        }

        [Test]
        public void GetNumberOfOnlineUser_WhenCall_ReturnDataFromDataAccess()
        {
            // Arrange
            var expectedResult = "999";
            _licenseeDataAccess.GetNumberOfOnlineUser().Returns(expectedResult);

            // Action
            var result = _licenseeService.GetNumberOfOnlineUser();

            // Assert
            Assert.That(result, Is.EqualTo(expectedResult));
        }
    }
}