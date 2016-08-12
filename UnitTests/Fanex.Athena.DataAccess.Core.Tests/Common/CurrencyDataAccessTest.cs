namespace Fanex.Athena.DataAccess.Core.Tests.Common
{
    using System.Data;
    using Core.Common;
    using Data;
    using Model.Core.DTO.Common;
    using Model.Core.MetaModel.Account;
    using NSubstitute;
    using NUnit.Framework;
    using TestHelpers;
    [TestFixture]
    public class CurrencyDataAccessTest
    {
        private IObjectDb _objectDb;
        private CurrencyDataAccess _dataAccess;

        [SetUp]
        public void Setup()
        {
            _objectDb = Substitute.For<IObjectDb>();
            _dataAccess = Substitute.ForPartsOf<CurrencyDataAccess>();
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
            var expected = ResultInfoHelper.GetFakeResultInfo(_objectDb);

            // Action
            var result = _dataAccess.AssignCurrenciesByUser(new AssignCurrencyMetaModel());

            // Assert
            Assert.That(result.Code, Is.EqualTo(expected.Code));
            Assert.That(result.Message, Is.EqualTo(expected.Message));
        }

        [Test]
        public void GetCurrenciesByUser_WhenCall_ParametersPassedCorrectly()
        {
            // Arrange
            var licenseeId = 1;
            var userId = 2;           
            _dataAccess.GetObjectDb(Arg.Is<string>("Lrf_SelCurrencyByLicenseeUser")).Returns(_objectDb);

            // Action
            _dataAccess.GetCurrenciesByUser(licenseeId, userId);

            // Assert
            _objectDb.Received().AddInParameter("LicenseeID", DbType.Int32, licenseeId);
            _objectDb.Received().AddInParameter("UserID", DbType.Int32, userId);           
        }
    }
}