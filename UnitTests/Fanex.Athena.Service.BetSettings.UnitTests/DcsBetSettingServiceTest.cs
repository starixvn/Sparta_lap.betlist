namespace Fanex.Athena.Service.BetSettings.UnitTests
{
    using Core.Users.Interfaces;
    using DataAccess.BetSettings.Interfaces;
    using Interfaces;
    using Model.Core.DTO.Common;
    using NSubstitute;
    using NUnit.Framework;
    using Resources;
    using UnitTest.Common.Helpers;

    [TestFixture]
    public partial class DcsBetSettingServiceTest
    {
        private readonly int _licenseeId = FakeHelper.Create<int>();
        private readonly int _custId = FakeHelper.Create<int>();
        private readonly int _productId = FakeHelper.Create<int>();
        private readonly int _userId = FakeHelper.Create<int>();

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
            _authorizationService.HasPermissionOnData(_licenseeId, _custId, 0).Returns(true);
            _betSettingDataAccess.GetStatus(_custId, _productId).Returns(true);

            // Action
            var result = _dcsBettingService.GetStatus(_licenseeId, _custId, _productId);

            // Assert
            Assert.That(result, Is.True);
        }

        [Test]
        public void GetStatus_HaveNoPermission_ReturnNull()
        {
            // Arrange
            _authorizationService.HasPermissionOnData(_licenseeId, _custId, 0).Returns(false);

            // Action
            var result = _dcsBettingService.GetStatus(_licenseeId, _custId, _productId);

            // Assert
            Assert.That(result, Is.Null);
        }

        [Test]
        public void UpdateStatus_WhenCallWithCorrectPermission_ReturnCorrectResult()
        {
            // Arrange
            var ip = FakeHelper.Create<string>();
            var status = FakeHelper.Create<bool>();
            var expectedResult = FakeHelper.Create<ResultInfo>();

            _betSettingDataAccess.UpdateStatus(_userId, _custId, ip, status, _productId).Returns(expectedResult);
            _authorizationService.HasPermissionOnData(_licenseeId, _custId, 0).Returns(true);

            // Action
            var result = _dcsBettingService.UpdateStatus(_licenseeId, _userId, _custId, ip, status, _productId);

            // Assert
            Assert.That(result, Is.EqualTo(expectedResult));
        }

        [Test]
        public void UpdateStatus_HaveNoPermission_ReturnCorrectResult()
        {
            // Arrange
            var ip = FakeHelper.Create<string>();
            var status = FakeHelper.Create<bool>();
            var expectedResult = new ResultInfo { Code = 1, Message = ViewResources.AccessDeniedMsg };

            _authorizationService.HasPermissionOnData(_licenseeId, _custId, Arg.Any<long>()).Returns(false);

            // Action
            var result = _dcsBettingService.UpdateStatus(_licenseeId, _userId, _custId, ip, status, _productId);

            // Assert
            Assert.That(result.Code, Is.EqualTo(expectedResult.Code));
            Assert.That(result.Message, Is.EqualTo(expectedResult.Message));
        }
    }
}