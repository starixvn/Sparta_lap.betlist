namespace Fanex.Athena.Service.BetSettings.UnitTests
{
    using System.Collections.Generic;
    using DataAccess.BetSettings.Interfaces;
    using Model.BetSettings.DTO;
    using NSubstitute;
    using NUnit.Framework;

    [TestFixture]
    public class CoreBetSettingServiceTest
    {
        private const int LicenseeId = 1;
        private ICoreBetSettingDataAccess _coreBetSettingDataAccess;
        private CoreBetSettingService _coreBetSettingService;

        [SetUp]
        public void Setup()
        {
            _coreBetSettingDataAccess = Substitute.For<ICoreBetSettingDataAccess>();
            _coreBetSettingService = new CoreBetSettingService(_coreBetSettingDataAccess);
        }

        [TearDown]
        public void TearDown()
        {
            _coreBetSettingDataAccess = null;
            _coreBetSettingService = null;
        }

        [Test]
        public void GetBetSettingProducts_WhenCall_ReturnCorrectData()
        {
            // Arrange
            var expectedResult = new List<BetSettingProduct>();
            _coreBetSettingDataAccess.GetBetSettingProducts(LicenseeId).Returns(expectedResult);

            // Action
            var result = _coreBetSettingService.GetBetSettingProducts(LicenseeId);

            // Assert
            Assert.That(result, Is.EqualTo(expectedResult));
        }

        [Test]
        public void IsBetSettingEditable_WhenCall_ReturnCorrectData()
        {
            // Arrange
            var userId = 2;
            _coreBetSettingDataAccess.IsBetSettingEditable(LicenseeId, userId).Returns(true);

            // Action
            var result = _coreBetSettingService.IsBetSettingEditable(LicenseeId, userId);

            // Assert
            Assert.That(result, Is.True);
        }
    }
}