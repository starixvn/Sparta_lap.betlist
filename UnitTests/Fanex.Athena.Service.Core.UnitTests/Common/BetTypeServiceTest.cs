namespace Fanex.Athena.Service.Core.UnitTests.Common
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Core.Common;
    using Core.Common.Interfaces;
    using DataAccess.Core.Common.Interfaces;
    using Model.Core.DTO.Common;
    using NSubstitute;
    using NUnit.Framework;
    using UnitTest.Common.Helpers;

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

        [TearDown]
        public void TearDown()
        {
            _betTypeDataAccess.ClearReceivedCalls();
        }

        [Test]
        public void GetBettypes_WhenCall_PassCorrectParamsToDataAccess()
        {
            // Arrange
            var parentSport = new SportProductItem { ParentId = -1, ID = 2 };
            var childSport1 = new SportProductItem { ParentId = parentSport.ID, ID = FakeHelper.Create<int>() };
            var childSport2 = new SportProductItem { ParentId = parentSport.ID, ID = FakeHelper.Create<int>() };
            var sports = new List<SportProductItem> { parentSport, childSport1, childSport2 };

            var expectedSportXmlString = $"<products><product ID=\"{parentSport.ID}\" Sport=\"{childSport1.ID},{childSport2.ID}\"> </product></products>";

            var licenseeId = Math.Abs(FakeHelper.Create<int>()) + 1;
            var languageCode = FakeHelper.Create<string>();

            // Action
            _betTypeService.GetBettypes(licenseeId, sports, languageCode);

            // Assert
            _betTypeDataAccess.Received().GetBettypes(licenseeId, expectedSportXmlString, languageCode);
        }

        [Test]
        public void GetBettypes_NoChildSports_PassCorrectParamsToDataAccess()
        {
            // Arrange
            var parentSport = new SportProductItem { ParentId = -1, ID = FakeHelper.Create<int>() };
            var sports = new List<SportProductItem> { parentSport };

            var expectedSportXmlString = $"<products><product ID=\"{parentSport.ID}\" Sport=\"{parentSport.ID}\"> </product></products>";

            var licenseeId = Math.Abs(FakeHelper.Create<int>()) + 1;
            var languageCode = FakeHelper.Create<string>();

            // Action
            _betTypeService.GetBettypes(licenseeId, sports, languageCode);

            // Assert
            _betTypeDataAccess.Received().GetBettypes(licenseeId, expectedSportXmlString, languageCode);
        }

        [Test]
        public void GetBettypes_WhenCall_ReturnResultFromDataAccess()
        {
            // Arrange
            var expectedResult = FakeHelper.CreateMany<CheckedItem>(10).ToList();
            var sports = new List<SportProductItem>
            {
                new SportProductItem()
            };
            var licenseeId = Math.Abs(FakeHelper.Create<int>()) + 1;
            var languageCode = FakeHelper.Create<string>();
            _betTypeDataAccess.GetBettypes(licenseeId, Arg.Any<string>(), languageCode).Returns(expectedResult);

            // Action
            var result = _betTypeService.GetBettypes(licenseeId, sports, languageCode);

            // Assert
            Assert.That(result, Is.EqualTo(expectedResult));
        }

        [Test]
        public void GetBettypes_SportsIsNull_ReturnEmptyDataList()
        {
            // Arrange
            var licenseeId = Math.Abs(FakeHelper.Create<int>()) + 1;
            var languageCode = FakeHelper.Create<string>();

            IList<SportProductItem> sports = null;

            // Action
            var result = _betTypeService.GetBettypes(licenseeId, sports, languageCode);

            // Assert
            Assert.That(result.Count, Is.EqualTo(0));
        }

        [Test]
        public void GetBettypes_SportsIsEmpty_ReturnEmptyDataList()
        {
            // Arrange
            var licenseeId = Math.Abs(FakeHelper.Create<int>()) + 1;
            var languageCode = FakeHelper.Create<string>();

            IList<SportProductItem> sports = new List<SportProductItem>();

            // Action
            var result = _betTypeService.GetBettypes(licenseeId, sports, languageCode);

            // Assert
            Assert.That(result.Count, Is.EqualTo(0));
        }

        [Test]
        public void GetBettypes_InvalidLicensee_ReturnEmptyDataList()
        {
            // Arrange
            var licenseeId = Math.Abs(FakeHelper.Create<int>()) * (-1);
            var languageCode = FakeHelper.Create<string>();
            var sports = new List<SportProductItem>
            {
                new SportProductItem()
            };

            // Action
            var result = _betTypeService.GetBettypes(licenseeId, sports, languageCode);

            // Assert
            Assert.That(result.Count, Is.EqualTo(0));
        }

        [Test]
        public void GetBettypes_EmptySportAndInvalidLicensee_ReturnEmptyDataList()
        {
            // Arrange
            var licenseeId = Math.Abs(FakeHelper.Create<int>()) * (-1);
            var languageCode = FakeHelper.Create<string>();
            var sports = new List<SportProductItem>();

            // Action
            var result = _betTypeService.GetBettypes(licenseeId, sports, languageCode);

            // Assert
            Assert.That(result.Count, Is.EqualTo(0));
        }

        [Test]
        public void GetBettypes_SportNullAndInvalidLicensee_ReturnEmptyDataList()
        {
            // Arrange
            var licenseeId = Math.Abs(FakeHelper.Create<int>()) * (-1);
            var languageCode = FakeHelper.Create<string>();
            IList<SportProductItem> sports = null;

            // Action
            var result = _betTypeService.GetBettypes(licenseeId, sports, languageCode);

            // Assert
            Assert.That(result.Count, Is.EqualTo(0));
        }
    }
}