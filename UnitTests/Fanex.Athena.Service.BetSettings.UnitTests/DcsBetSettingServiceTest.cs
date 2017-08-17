namespace Fanex.Athena.Service.BetSettings.UnitTests
{
    using Core.Users.Interfaces;
    using DataAccess.BetSettings.Interfaces;
    using Interfaces;
    using Model.Core.DTO.Common;
    using NSubstitute;
    using NUnit.Framework;
    using Resources;

    [TestFixture]
    public partial class DcsBetSettingServiceTest
    {
        private const int LicenseeId = 1;
        private const int CustId = 2;
        private const int ProductId = 3;
        private const int UserId = 4;

        private IDcsBetSettingDataAccess _betSettingDataAccess;
        private IDcsBetSettingService _dcsBettingService;
        private IAuthorizationService _authorizationService;

        [OneTimeSetUp]
        public void Setup()
        {
            _betSettingDataAccess = Substitute.For<IDcsBetSettingDataAccess>();
            _authorizationService = Substitute.For<IAuthorizationService>();
            _dcsBettingService = new DcsBetSettingService(_betSettingDataAccess, _authorizationService);
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            _betSettingDataAccess = null;
            _dcsBettingService = null;
            _authorizationService = null;
        }

        [Test]
        public void GetStatus_WhenCallWithCorrectPermission_ReturnCorrectData()
        {
            // Arrange
            _authorizationService.HasPermissionOnData(LicenseeId, CustId, Arg.Any<long>()).Returns(true);
            _betSettingDataAccess.GetStatus(CustId, ProductId).Returns(true);

            // Action
            var result = _dcsBettingService.GetStatus(LicenseeId, CustId, ProductId);

            // Assert
            Assert.That(result, Is.True);
        }

        [Test]
        public void GetStatus_HaveNoPermission_ReturnNull()
        {
            // Arrange
            _authorizationService.HasPermissionOnData(LicenseeId, CustId, Arg.Any<long>()).Returns(false);

            // Action
            var result = _dcsBettingService.GetStatus(LicenseeId, CustId, ProductId);

            // Assert
            Assert.That(result, Is.Null);
        }

        [Test]
        public void UpdateStatus_WhenCallWithCorrectPermission_ReturnCorrectResult()
        {
            // Arrange
            _authorizationService.HasPermissionOnData(LicenseeId, CustId, Arg.Any<long>()).Returns(true);
            var userId = 4;
            var ip = string.Empty;
            var status = true;
            var expectedResult = new ResultInfo { Code = 100, Message = "Success" };
            _betSettingDataAccess.UpdateStatus(userId, CustId, ip, status, ProductId).Returns(expectedResult);

            // Action
            var result = _dcsBettingService.UpdateStatus(LicenseeId, userId, CustId, ip, status, ProductId);

            // Assert
            Assert.That(result, Is.EqualTo(expectedResult));
        }

        [Test]
        public void UpdateStatus_HaveNoPermission_ReturnCorrectResult()
        {
            // Arrange
            _authorizationService.HasPermissionOnData(LicenseeId, CustId, Arg.Any<long>()).Returns(false);
            var expectedResult = new ResultInfo { Code = 1, Message = ViewResources.AccessDeniedMsg };
            var userId = 4;
            var ip = string.Empty;
            var status = true;

            // Action
            var result = _dcsBettingService.UpdateStatus(LicenseeId, userId, CustId, ip, status, ProductId);

            // Assert
            Assert.That(result.Code, Is.EqualTo(expectedResult.Code));
            Assert.That(result.Message, Is.EqualTo(expectedResult.Message));
        }
    }
}