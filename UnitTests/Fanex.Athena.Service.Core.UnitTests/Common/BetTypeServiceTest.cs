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

        [SetUp]
        public void SetUp()
        {
            _betTypeDataAccess = Substitute.For<IBetTypeDataAccess>();
            _betTypeService = new BetTypeService(_betTypeDataAccess);
        }

        [TearDown]
        public void TearDown()
        {
            _betTypeDataAccess = null;
            _betTypeService = null;
        }

        [Test]
        public void GetBettypes_WhenCall_PassCorrectParamsToDataAccess()
        {
            // Arrange
            const string ExpectedSportXmlString = "<products><product ID=\"1\" Sport=\"2,3\"> </product></products>";
            var sports = new List<SportProductItem>
            {
                new SportProductItem { ParentId = -1, ID = 1 },
                new SportProductItem { ParentId = 1, ID = 2 },
                new SportProductItem { ParentId = 1, ID = 3 }
            };

            const int LicenseeId = 1;
            const string LanguageCode = "en-US";

            var actualSportXmlString = string.Empty;
            _betTypeDataAccess.When(x => x.GetBettypes(LicenseeId, Arg.Any<string>(), LanguageCode))
                .Do(x => actualSportXmlString = (string)x[1]);

            // Action
            _betTypeService.GetBettypes(LicenseeId, sports, LanguageCode);

            // Assert
            Assert.That(actualSportXmlString, Is.EqualTo(ExpectedSportXmlString));
        }

        [Test]
        public void GetBettypes_NoChildSports_PassCorrectParamsToDataAccess()
        {
            // Arrange
            var expectedSportXmlString = "<products><product ID=\"1\" Sport=\"1\"> </product></products>";
            var sports = new List<SportProductItem>
            {
                new SportProductItem { ParentId = -1, ID = 1 }
            };

            const int LicenseeId = 1;
            const string LanguageCode = "en-US";

            var actualSportXmlString = string.Empty;
            _betTypeDataAccess.When(x => x.GetBettypes(LicenseeId, Arg.Any<string>(), LanguageCode))
                .Do(x => actualSportXmlString = (string)x[1]);

            // Action
            _betTypeService.GetBettypes(LicenseeId, sports, LanguageCode);

            // Assert
            _betTypeDataAccess.Received().GetBettypes(LicenseeId, expectedSportXmlString, LanguageCode);
        }

        [Test]
        public void GetBettypes_WhenCall_ReturnResultFromDataAccess()
        {
            // Arrange
            var expectedResult = new List<CheckedItem>
            {
                new CheckedItem { ID = 1 },
                new CheckedItem { ID = 2 }
            };
            var sports = new List<SportProductItem>
            {
                new SportProductItem()
            };
            const int LicenseeId = 1;
            _betTypeDataAccess.GetBettypes(LicenseeId, Arg.Any<string>(), Arg.Any<string>()).Returns(expectedResult);

            // Action
            var result = _betTypeService.GetBettypes(LicenseeId, sports, Arg.Any<string>());

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result[0].ID, Is.EqualTo(expectedResult[0].ID));
                Assert.That(result[1].ID, Is.EqualTo(expectedResult[1].ID));
            });
        }

        [Test]
        public void GetBettypes_NoSportItem_ReturnEmptyDataList()
        {
            // Arrange
            const int LicenseeId = 1;

            // Action
            var result = _betTypeService.GetBettypes(LicenseeId, null, Arg.Any<string>());

            // Assert
            Assert.That(result.Count, Is.EqualTo(0));
        }

        [Test]
        public void GetBettypes_InvalidLicensee_ReturnEmptyDataList()
        {
            // Arrange
            const int LicenseeId = 0;
            var sports = new List<SportProductItem>
            {
                new SportProductItem()
            };

            // Action
            var result = _betTypeService.GetBettypes(LicenseeId, sports, Arg.Any<string>());

            // Assert
            Assert.That(result.Count, Is.EqualTo(0));
        }
    }
}