namespace Fanex.Athena.DataAccess.Core.UnitTests.Common
{
    using System.Data;
    using Core.Common;
    using Data;
    using Model.Core.DTO.Common;
    using Model.Core.DTO.Users;
    using Model.Core.MetaModel.Account;
    using NSubstitute;
    using NUnit.Framework;
    using TestHelpers;

    [TestFixture]
    public class CurrencyDataAccessTest
    {
        private IObjectDb _objectDb;
        private CurrencyDataAccess _dataAccess;
        private IDataReader _reader;

        [OneTimeSetUp]
        public void Setup()
        {
            _objectDb = Substitute.For<IObjectDb>();
            _dataAccess = Substitute.ForPartsOf<CurrencyDataAccess>();
            _reader = Substitute.For<IDataReader>();
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            _objectDb = null;
            _dataAccess = null;
            _reader = null;
        }

        [Test]
        public void GetCurrencies_WhenCall_ExecuteReaderMethodIsCalled()
        {
            // Arrange
            _dataAccess.GetObjectDb(Arg.Is<string>("Lrf_SelCurrencyByLicensee")).Returns(_objectDb);
            const int LicenseeId = 1;

            // Action
            _dataAccess.GetCurrencies(LicenseeId);

            // Assert
            _objectDb.ReceivedWithAnyArgs().ExecuteReader(null);
        }

        [Test]
        public void GetCurrencies_WhenCall_DataReturnCorrectly()
        {
            // Arrange
            _dataAccess.GetObjectDb(Arg.Is<string>("Lrf_SelCurrencyByLicensee")).Returns(_objectDb);
            _objectDb.ExecuteReader(Arg.Any<object>()).Returns(_reader);
            _reader.Read().Returns(true, false);

            var expected = new CurrencyUser
            {
                CurrencyId = 1,
                CurrencyName = "Currency Test"
            };
            _reader.GetInt32(Arg.Any<int>()).Returns(expected.CurrencyId);
            _reader.GetString(Arg.Any<int>()).Returns(expected.CurrencyName);

            const int LicenseeId = 1;

            // Action
            var result = _dataAccess.GetCurrencies(LicenseeId);

            // Assert
            Assert.That(result[0].CurrencyId, Is.EqualTo(expected.CurrencyId));
            Assert.That(result[0].CurrencyName, Is.EqualTo(expected.CurrencyName));
        }

        [Test]
        public void AssignCurrenciesByUser_WhenCall_ParametersPassedCorrectly()
        {
            // Arrange
            _dataAccess.GetObjectDb(Arg.Is<string>("Lrf_CurrencyCustIns")).Returns(_objectDb);
            var dataModel = new AssignCurrencyMetaModel
            {
                UserId = 1,
                LicenseeId = 1,
                CurrencyList = "USD, UUS, EUR",
                ModifyBy = 2,
                LangKey = "en-US",
                IP = "::1"
            };

            // Action
            _dataAccess.AssignCurrenciesByUser(dataModel);

            // Assert
            _objectDb.Received().AddInParameter("UserID", DbType.Int32, dataModel.UserId);
            _objectDb.Received().AddInParameter("LicenseeID", DbType.Int32, dataModel.LicenseeId);
            _objectDb.Received().AddInParameter("CurrencyXML", DbType.String, dataModel.CurrencyList);
            _objectDb.Received().AddInParameter("ModifyBy", DbType.Int32, dataModel.ModifyBy);
            _objectDb.Received().AddInParameter("LangKey", DbType.String, dataModel.LangKey);
            _objectDb.Received().AddInParameter("IP", DbType.String, dataModel.IP);
            _objectDb.Received().AddOutParameter("ErrCode", DbType.Int32, 4);
            _objectDb.Received().AddOutParameter("ErrMsg", DbType.String, 4000);
        }

        [Test]
        public void AssignCurrenciesByUser_WhenCall_ResultInfoReturnCorrectly()
        {
            // Arrange
            _dataAccess.GetObjectDb(Arg.Is<string>("Lrf_CurrencyCustIns")).Returns(_objectDb);
            ResultInfoHelper.MockResultInfo(_objectDb);

            // Action
            var result = _dataAccess.AssignCurrenciesByUser(new AssignCurrencyMetaModel());

            // Assert
            Assert.That(result.Code, Is.EqualTo(ResultInfoHelper.ResultCode));
            Assert.That(result.Message, Is.EqualTo(ResultInfoHelper.ResultMessage));
        }

        [Test]
        public void GetCurrenciesByUser_WhenCall_ParametersPassedCorrectly()
        {
            // Arrange
            const int licenseeId = 1;
            const int userId = 2;
            _dataAccess.GetObjectDb(Arg.Is<string>("Lrf_SelCurrencyByLicenseeUser")).Returns(_objectDb);

            // Action
            _dataAccess.GetCurrenciesByUser(licenseeId, userId);

            // Assert
            _objectDb.Received().AddInParameter("LicenseeID", DbType.Int32, licenseeId);
            _objectDb.Received().AddInParameter("UserID", DbType.Int32, userId);
        }
    }
}