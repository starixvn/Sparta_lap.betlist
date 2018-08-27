namespace Fanex.Athena.DataAccess.Core.UnitTests.Common
{
    using System;
    using System.Data;
    using System.Linq;
    using Core.Common;
    using NSubstitute;
    using NUnit.Framework;
    using TestHelpers;
    using VendorAdapter.Interfaces;

    public class SportTypeDataAccessTest : BaseDataAccessHelperTest<SportTypeDataAccess>
    {
        protected override void SetUpDataAccess()
        {
            DataAccess = Substitute.ForPartsOf<SportTypeDataAccess>(Substitute.For<ICacheRefAdapter>());
        }

        public override void FixtureSetUp()
        {
            base.FixtureSetUp();

            DataAccess.GetObjectDb(Arg.Any<string>()).Returns(ObjectDb);
        }

        [Test]
        public void GetSportsAndProducts_WhenCall_ParametersPassedCorrectly()
        {
            // Arrange
            const int LicenseeId = 1;
            const string LanguageCode = "en-US";

            // Action
            DataAccess.GetSportsAndProducts(LicenseeId, LanguageCode);

            // Assert
            Assert.Multiple(() =>
            {
                ObjectDb.Received().AddInParameter("licenseeId", DbType.Int32, LicenseeId);
                ObjectDb.Received().AddInParameter("langkey", DbType.String, LanguageCode);
            });
        }

        [TestCase(null, null, -1)]
        [TestCase(1, null, 1)]
        [TestCase(2, 2, 2)]
        public void GetSportsAndProducts_ProductIdIsValid_DataReturnCorrectly(int? parentId, int? productId, int expectedParentId)
        {
            // Arrange
            MapReaderData(new
            {
                ID = 0,
                Name = string.Empty
            });
            if (parentId.HasValue)
            {
                Reader["ParentId"].Returns(parentId);
            }
            else
            {
                Reader["ParentId"].Returns(DBNull.Value);
            }

            Reader.Read().Returns(true, false);

            // Action
            var result = DataAccess.GetSportsAndProducts(0, string.Empty, productId);

            // Assert
            Assert.That(result.ElementAt(0).ParentId, Is.EqualTo(expectedParentId));
        }

        [TestCase(1, 3)]
        public void GetSportsAndProducts_ProductIdIsInValid_ReturnEmptyData(int parentId, int? productId)
        {
            // Arrange
            MapReaderData(new
            {
                ParentId = parentId
            });
            Reader.Read().Returns(true, false);

            // Action
            var result = DataAccess.GetSportsAndProducts(0, string.Empty, productId);

            // Assert
            Assert.That(result, Is.Empty);
        }

        [Test]
        public void GetAllSports_WhenCall_ReturnCorrectData()
        {
            // Arrange
            const int SportType = 100;

            MapReaderData(new { Sporttype = SportType });
            Reader.Read().Returns(true, false);

            // Action
            var result = DataAccess.GetAllSports();

            // Assert
            Assert.That(result.ContainsKey(SportType), Is.True);
        }

        [Test]
        public void GetAllWHSports_WhenCall_ReturnCorrectData()
        {
            // Arrange
            const int SportType = 100;

            MapReaderData(new { Sporttype = SportType });
            Reader.Read().Returns(true, false);

            // Action
            var result = DataAccess.GetAllWHSports();

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.First().SportId, Is.EqualTo(0));
                Assert.That(result.ElementAt(1).SportId, Is.EqualTo(SportType));
            });
        }
    }
}