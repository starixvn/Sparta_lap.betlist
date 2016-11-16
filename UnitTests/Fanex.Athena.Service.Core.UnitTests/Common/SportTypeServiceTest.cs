namespace Fanex.Athena.Service.Core.UnitTests.Common
{
    using System.Collections.Generic;
    using System.Linq;
    using Core.Common;
    using Core.Common.Interfaces;
    using DataAccess.Core.Common.Interfaces;
    using Model.Core.DTO.Common;
    using NSubstitute;
    using NUnit.Framework;

    [TestFixture]
    public class SportTypeServiceTest
    {
        private ISportTypeDataAccess _sportTypeDataAccess;
        private ISportTypeService _sportTypeService;

        [OneTimeSetUp]
        public void SetUp()
        {
            _sportTypeDataAccess = Substitute.For<ISportTypeDataAccess>();
            _sportTypeService = new SportTypeService(_sportTypeDataAccess);
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            _sportTypeDataAccess = null;
            _sportTypeService = null;
        }

        [Test]
        public void GetAllSports_WhenCall_ReturnResultFromDataAccess()
        {
            // Arrange
            var expectedResult = new Dictionary<int, string>
            {
                { 1, "A" }
            };
            _sportTypeDataAccess.GetAllSports().Returns(expectedResult);

            // Action
            var result = _sportTypeService.GetAllSports();

            // Assert
            Assert.That(result, Contains.Key(1));
        }

        [Test]
        public void GetAllWHSports_WhenCall_ReturnResultFromDataAccess()
        {
            // Arrange
            var expectedResult = new List<Sport>
            {
                new Sport("1")
            };
            _sportTypeDataAccess.GetAllWHSports().Returns(expectedResult);

            // Action
            var result = _sportTypeService.GetAllWHSports();

            // Assert
            Assert.That(result[0].SportId, Is.EqualTo(expectedResult[0].SportId));
        }

        [Test]
        public void GetSportsAndProducts_WhenCall_ReturnResultFromDataAccess()
        {
            // Arrange
            var expectedResult = new List<SportProductItem>
            {
                new SportProductItem { ID = 1 }
            };
            var licenseeId = 1;
            var languageCode = "en-US";

            _sportTypeDataAccess.GetSportsAndProducts(licenseeId, languageCode).Returns(expectedResult);

            // Action
            var result = _sportTypeService.GetSportsAndProducts(licenseeId, languageCode);

            // Assert
            Assert.That(result.First().ID, Is.EqualTo(expectedResult[0].ID));
        }
    }
}