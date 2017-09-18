namespace Fanex.Athena.Service.Core.UnitTests.Users
{
    using System.Collections.Generic;
    using Core.Users;
    using Core.Users.Interfaces;
    using DataAccess.Core.Users.Interfaces;
    using Model.Core.DTO.Common;
    using Model.Core.DTO.Users;
    using NSubstitute;
    using NUnit.Framework;

    [TestFixture]
    public class LicenseeServiceTest
    {
        private ILicenseeDataAccess _licenseeDataAccess;
        private ILicenseeService _licenseeService;

        [OneTimeSetUp]
        public void SetUp()
        {
            _licenseeDataAccess = Substitute.For<ILicenseeDataAccess>();
            _licenseeService = new LicenseeService(_licenseeDataAccess);
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            _licenseeDataAccess = null;
            _licenseeService = null;
        }

        [Test]
        public void IsAllowUpdateMemberBAStatus_WhenCall_ReturnResultFromDataAccess()
        {
            // Arrange
            var expectedResult = new ResultInfo
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
            var licensees = new List<Licensee>
            {
                new Licensee { Id = 1 }
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
            var expectedResult = new List<Licensee>
            {
                new Licensee { Id = 1 }
            };
            var userId = 1;
            _licenseeDataAccess.GetByUser(userId).Returns(expectedResult);

            // Action
            var result = _licenseeService.GetByUser(userId);

            // Assert
            Assert.That(result[0].Id, Is.EqualTo(expectedResult[0].Id));
        }

        [Test]
        public void GetLicenseeByName_WhenCall_ReturnCorrectValue()
        {
            // Arrange
            var licenseeName = "LicenseeTest";
            var expectedResult = new Licensee
            {
                Id = 1,
                Name = licenseeName
            };
            _licenseeDataAccess.GetLicenseeByName(licenseeName).Returns(expectedResult);

            // Action
            var result = _licenseeService.GetLicenseeByName(licenseeName);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.Id, Is.EqualTo(expectedResult.Id));
                Assert.That(result.Name, Is.EqualTo(expectedResult.Name));
            });
        }
    }
}