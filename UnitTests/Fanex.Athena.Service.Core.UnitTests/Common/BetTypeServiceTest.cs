namespace Fanex.Athena.Service.Core.UnitTests.Common
{
    using System.Collections.Generic;
    using Core.Common;
    using Core.Common.Interfaces;
    using DataAccess.Core.Common.Interfaces;
    using Model.Core.DTO.Common;
    using NSubstitute;
    using NUnit.Framework;

    [TestFixture]
    public class BetTypeServiceTest
    {
        private IBetTypeDataAccess _betTypeDataAccess;

        private IBetTypeService _betTypeService;

        [OneTimeSetUp]
        public void SetUp()
        {
            _betTypeDataAccess = Substitute.For<IBetTypeDataAccess>();
            _betTypeService = new BetTypeService(_betTypeDataAccess);
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            _betTypeDataAccess = null;
            _betTypeService = null;
        }

        [Test]
        public void GetBettypes_WhenCall_PassCorrectParamsToDataAccess()
        {
            // Arrange
            var expectedSportXmlString = "<products><product ID=\"1\" Sport=\"2,3\"> </product></products>";
            var sports = new List<SportProductItem>
            {
                new SportProductItem() { ParentId = -1, ID = 1 },
                new SportProductItem() { ParentId = 1, ID = 2 },
                new SportProductItem() { ParentId = 1, ID = 3 }
            };

            var licenseeId = 1;
            var languageCode = "en-US";

            // Action
            _betTypeService.GetBettypes(licenseeId, sports, languageCode);

            // Assert
            _betTypeDataAccess.Received().GetBettypes(licenseeId, expectedSportXmlString, languageCode);
        }

        [Test]
        public void GetBettypes_WhenCall_ReturnResultFromDataAccess()
        {
            // Arrange
            var expectedResult = new List<CheckedItem>
            {
                new CheckedItem() { ID = 1 },
                new CheckedItem() { ID = 2 }
            };
            var sports = new List<SportProductItem>
            {
                new SportProductItem()
            };
            var licenseeId = 1;
            _betTypeDataAccess.GetBettypes(licenseeId, Arg.Any<string>(), Arg.Any<string>()).Returns(expectedResult);

            // Action
            var result = _betTypeService.GetBettypes(licenseeId, sports, Arg.Any<string>());

            // Assert
            Assert.That(result[0].ID, Is.EqualTo(expectedResult[0].ID));
            Assert.That(result[1].ID, Is.EqualTo(expectedResult[1].ID));
        }

        [Test]
        public void GetBettypes_WhenCall_ReturnEmptyDataList()
        {
            // Arrange
            var licenseeId = 1;

            // Action
            var result = _betTypeService.GetBettypes(licenseeId, null, Arg.Any<string>());

            // Assert
            Assert.That(result.Count, Is.EqualTo(0));
        }
    }
}