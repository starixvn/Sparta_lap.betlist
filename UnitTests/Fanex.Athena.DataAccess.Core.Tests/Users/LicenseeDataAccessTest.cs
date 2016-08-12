namespace Fanex.Athena.DataAccess.Core.Tests.Users
{
    using System.Data;
    using Core.Configuration;
    using Core.Users;
    using Data;
    using Model.Core.DTO.Common;
    using NSubstitute;
    using NUnit.Framework;

    [TestFixture]
    public class LicenseeDataAccessTest
    {
        private IObjectDb _objectDb;
        private LicenseeDataAccess _dataAccess;

        [SetUp]
        public void Setup()
        {
            _objectDb = Substitute.For<IObjectDb>();
            _dataAccess = Substitute.ForPartsOf<LicenseeDataAccess>();
        }

        [Test]
        public void IsAllowUpdateMemberBAStatus_WhenCall_ParametersPassedCorrectly()
        {
            // Arrange
            _dataAccess.GetObjectDb(Arg.Is<string>("Lrf_BATimeCheck")).Returns(_objectDb);
            var request = new Request { Language = "en-US" };
            var licenseeId = 1;
            var isAllow = true;

            // Action
            _dataAccess.IsAllowUpdateMemberBAStatus(request, licenseeId, out isAllow);

            // Assert
            _objectDb.Received().AddInParameter("langkey", DbType.String, request.Language);
            _objectDb.Received().AddInParameter("LicenseeID", DbType.Int32, licenseeId);
            _objectDb.Received().AddOutParameter("isallow", DbType.Boolean, sizeof(bool));
        }

        [Test]
        public void GetBelongingStatus_WhenCall_ParametersPassedCorrectly()
        {
            // Arrange
            _dataAccess.GetObjectDb(Arg.Is<string>("Lrf_CustomerSelCheckBelonging")).Returns(_objectDb);
            var licenseeId = 1;
            var custId = 2;

            // Action
            _dataAccess.GetBelongingStatus(licenseeId, custId);

            // Assert
            _objectDb.Received().AddInParameter("LicenseeID", DbType.Int32, licenseeId);
            _objectDb.Received().AddInParameter("custid", DbType.Int32, custId);
            _objectDb.Received().AddOutParameter("status", DbType.Byte, 4);
        }

        [Test]
        public void GetByUser_WhenCall_ExecuteReaderMethodIsCalled()
        {
            // Arrange
            _dataAccess.GetObjectDb(Arg.Is<string>("Lrf_LicenseeSel")).Returns(_objectDb);
            var userId = 1;

            // Action
            _dataAccess.GetByUser(userId);

            // Assert
            _objectDb.ReceivedWithAnyArgs().ExecuteReader(null);
        }

        [Test]
        public void GetCurrencies_WhenCall_ExecuteReaderMethodIsCalled()
        {
            // Arrange
            _dataAccess.GetObjectDb(Arg.Is<string>("Lrf_SelCurrency")).Returns(_objectDb);
            var licenseeId = 1;
            var userId = 2;

            // Action
            _dataAccess.GetCurrencies(licenseeId, userId);

            // Assert
            _objectDb.ReceivedWithAnyArgs().ExecuteReader(null);
        }

        [Test]
        public void GetCustByLicensee_WhenCall_ExecuteReaderMethodIsCalled()
        {
            // Arrange
            _dataAccess.GetObjectDb(Arg.Is<string>("Lrf_LicenseeCustSel")).Returns(_objectDb);
            var licenseeId = 1;

            // Action
            _dataAccess.GetCustByLicensee(licenseeId);

            // Assert
            _objectDb.ReceivedWithAnyArgs().ExecuteReader(null);
        }

        [Test]
        public void GetByResellerId_WhenCall_ExecuteReaderMethodIsCalled()
        {
            // Arrange
            _dataAccess.GetObjectDb(Arg.Is<string>("Lrf_LicenseeSel")).Returns(_objectDb);
            var userId = 1;
            var resellerId = 2;

            // Action
            _dataAccess.GetByResellerId(userId, resellerId);

            // Assert
            _objectDb.ReceivedWithAnyArgs().ExecuteReader(null);
        }

        [Test]
        public void GetNumberOfOnlineUser_WhenCall_ExecuteScalarMethodIsCalled()
        {
            // Arrange
            _dataAccess.GetObjectDb(Arg.Is<string>("Lrf_GetNumberOfOnlineUsers")).Returns(_objectDb);

            // Action
            _dataAccess.GetNumberOfOnlineUser();

            // Assert
            _objectDb.Received().ExecuteScalar();
        }
    }
}