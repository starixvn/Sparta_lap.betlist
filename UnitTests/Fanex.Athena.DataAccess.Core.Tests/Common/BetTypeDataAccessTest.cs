namespace Fanex.Athena.DataAccess.Core.Tests.Common
{
    using System.Collections.Generic;
    using System.Data;
    using Core.Common;
    using Data;
    using Model.Core.DTO.Common;
    using Model.Core.MetaModel.Product;
    using NSubstitute;
    using NUnit.Framework;

    [TestFixture]
    public class BetTypeDataAccessTest
    {
        private IObjectDb _objectDb;
        private BetTypeDataAccess _dataAccess;
        private IDataReader _reader;

        [SetUp]
        public void Setup()
        {
            _objectDb = Substitute.For<IObjectDb>();
            _dataAccess = Substitute.ForPartsOf<BetTypeDataAccess>();
            _reader = Substitute.For<IDataReader>();
            _objectDb.ExecuteReader().Returns(_reader);           
        }

        [Test]
        public void GetBettypes_WhenCall_PassCorrectParameters()
        {
            // Arrange
            int licenseeId = 1;
            var sports = "1,2,3";
            var languageCode = "en-US";
            _dataAccess.GetObjectDb(Arg.Is("Lrf_SelBetTypeBySport")).Returns(_objectDb);

            // Action
            _dataAccess.GetBettypes(licenseeId, sports, languageCode);

            // Assert
            _objectDb.Received().AddInParameter("licenseeid", DbType.Int32, licenseeId);
            _objectDb.Received().AddInParameter("sportlist", DbType.String, sports);
            _objectDb.Received().AddInParameter("langkey", DbType.String, languageCode);
        }

        [Test]
        public void GetBettypes_WhenCall_DataReturnedCorrectly()
        {
            // Arrange
            int licenseeId = 1;
            var sports = "1,2,3";
            var languageCode = "en-US";
            var expected = new CheckedItem { Checked = true, ID = 1, Name = "BettypeNameTest" };

            _dataAccess.GetObjectDb(Arg.Is("Lrf_SelBetTypeBySport")).Returns(_objectDb);
            _reader.Read().Returns(true, false);
            _reader["BettypeID"].Returns(expected.ID);
            _reader["BettypeName"].Returns(expected.Name);

            // Action
            var result = _dataAccess.GetBettypes(licenseeId, sports, languageCode);

            // Assert
            Assert.That(result[0].Checked, Is.True);
            Assert.That(result[0].ID, Is.EqualTo(expected.ID));
            Assert.That(result[0].Name, Is.EqualTo(expected.Name));
        }

        [Test]
        public void GetAllBettypes_WhenCall_PassCorrectParameters()
        {
            // Arrange
            _dataAccess.GetObjectDb(Arg.Is("Lrf_BetTypesGetAll")).Returns(_objectDb);
            var productOptions = new List<ProductOption>
            {
                new ProductOption() { Code = "SB", Status = false },
                new ProductOption() { Code = "VB", Status = true }
            };

            // Action
            _dataAccess.GetAllBettypes(productOptions);

            // Assert
            _objectDb.Received().AddInParameter("showSB", DbType.Boolean, false);
            _objectDb.Received().AddInParameter("showVB", DbType.Boolean, true);
        }
    }
}
