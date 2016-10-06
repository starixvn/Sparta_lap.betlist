namespace Fanex.Athena.DataAccess.Core.UnitTests.Users
{
    using System.Data;
    using Core.Users;
    using Data;
    using Model.Core.DTO.Common;
    using Model.Core.DTO.Users;
    using NSubstitute;
    using NUnit.Framework;
    using TestHelpers;

    [TestFixture]
    public class LicenseeDataAccessTest
    {
        private IObjectDb _objectDb;
        private LicenseeDataAccess _dataAccess;
        private IDataReader _reader;

        [SetUp]
        public void Setup()
        {
            _objectDb = Substitute.For<IObjectDb>();
            _dataAccess = Substitute.ForPartsOf<LicenseeDataAccess>();
            _reader = Substitute.For<IDataReader>();
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
        public void IsAllowUpdateMemberBAStatus_WhenCall_ValueReturnedCorrectly()
        {
            // Arrange
            _dataAccess.GetObjectDb(Arg.Is<string>("Lrf_BATimeCheck")).Returns(_objectDb);
            _objectDb.GetParameterValue("isallow").Returns(false);
            var request = new Request { Language = "en-US" };
            var licenseeId = 1;
            var isAllow = true;
            ResultInfoHelper.MockResultInfo(_objectDb);

            // Action
            var result = _dataAccess.IsAllowUpdateMemberBAStatus(request, licenseeId, out isAllow);

            // Assert
            Assert.That(isAllow, Is.False);
            Assert.That(result.Code, Is.EqualTo(ResultInfoHelper.ResultCode));
            Assert.That(result.Message, Is.EqualTo(ResultInfoHelper.ResultMessage));
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
        public void GetBelongingStatus_WhenCall_ValueReturnedCorrectly()
        {
            // Arrange
            _dataAccess.GetObjectDb(Arg.Is<string>("Lrf_CustomerSelCheckBelonging")).Returns(_objectDb);
            var licenseeId = 1;
            var custId = 2;
            var expected = 5;
            _objectDb.GetParameterValue("status").Returns(expected);

            // Action
            var result = _dataAccess.GetBelongingStatus(licenseeId, custId);

            // Assert
            Assert.That(result, Is.EqualTo(expected));
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
        public void GetByUser_WhenCall_DataReturnedCorrectly()
        {
            // Arrange
            _dataAccess.GetObjectDb(Arg.Is<string>("Lrf_LicenseeSel")).Returns(_objectDb);
            _objectDb.ExecuteReader(Arg.Any<object>()).Returns(_reader);
            _reader.Read().Returns(true, false);

            var expected = new Licensee
            {
                Id = 1,
                Name = "LicenseeTest",
                IsAllowedCustomerComm = true
            };
            _reader.GetInt32(Arg.Any<int>()).Returns(expected.Id);
            _reader.GetString(Arg.Any<int>()).Returns(expected.Name);
            _reader.GetBoolean(Arg.Any<int>()).Returns(expected.IsAllowedCustomerComm);

            var userId = 1;

            // Action
            var result = _dataAccess.GetByUser(userId);

            // Assert
            Assert.That(result[0].Id, Is.EqualTo(expected.Id));
            Assert.That(result[0].Name, Is.EqualTo(expected.Name));
            Assert.That(result[0].IsAllowedCustomerComm, Is.EqualTo(expected.IsAllowedCustomerComm));
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
        public void GetCurrencies_WhenCall_DataReturnCorrectly()
        {
            // Arrange
            _dataAccess.GetObjectDb(Arg.Is<string>("Lrf_SelCurrency")).Returns(_objectDb);
            _objectDb.ExecuteReader(Arg.Any<object>()).Returns(_reader);
            _reader.Read().Returns(true, false);

            var expected = new Currency
            {
                CurrencyId = 1,
                Name = "Currency Test"
            };
            _reader.GetInt32(Arg.Any<int>()).Returns(expected.CurrencyId);
            _reader.GetString(Arg.Any<int>()).Returns(expected.Name);

            var licenseeId = 1;
            var userId = 2;

            // Action
            var result = _dataAccess.GetCurrencies(licenseeId, userId);

            // Assert
            Assert.That(result[0].CurrencyId, Is.EqualTo(expected.CurrencyId));
            Assert.That(result[0].Name, Is.EqualTo(expected.Name));
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
        public void GetCustByLicensee_WhenCall_DataReturnedCorrectly()
        {
            // Arrange
            _dataAccess.GetObjectDb(Arg.Is<string>("Lrf_LicenseeCustSel")).Returns(_objectDb);
            _objectDb.ExecuteReader(Arg.Any<object>()).Returns(_reader);
            _reader.Read().Returns(true, false);

            var licenseeId = 1;
            var expected = new LicenseeCustomer
            {
                Id = licenseeId,
                Username = "UserNameTest",
                CustId = 2,
                Currency = 3,
                RoleId = 4,
                Description = "Description Test",
                CurrencyName = "CurrencyName Test"
            };
            ConfigureReaderForGetCustByLicensee(expected);

            // Action
            var result = _dataAccess.GetCustByLicensee(licenseeId);

            // Assert
            Assert.That(result[0].Id, Is.EqualTo(expected.Id));
            Assert.That(result[0].Username, Is.EqualTo(expected.Username));
            Assert.That(result[0].CustId, Is.EqualTo(expected.CustId));
            Assert.That(result[0].Currency, Is.EqualTo(expected.Currency));
            Assert.That(result[0].RoleId, Is.EqualTo(expected.RoleId));
            Assert.That(result[0].Description, Is.EqualTo(expected.Description));
            Assert.That(result[0].CurrencyName, Is.EqualTo(expected.CurrencyName));
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

        [Test]
        public void GetNumberOfOnlineUser_WhenCall_ValueReturnedCorrectly()
        {
            // Arrange
            _dataAccess.GetObjectDb(Arg.Is<string>("Lrf_GetNumberOfOnlineUsers")).Returns(_objectDb);
            var expected = "ResultTest";
            _objectDb.ExecuteScalar().Returns(expected);

            // Action
            var result = _dataAccess.GetNumberOfOnlineUser();

            // Assert
            Assert.That(result, Is.EqualTo(expected));
        }

        private void ConfigureReaderForGetCustByLicensee(LicenseeCustomer expected)
        {
            int usernameIndex = 0, custIdIndex = 1, currencyIndex = 2, roleIdIndex = 3, currencyNameIndex = 4;

            _reader.GetOrdinal("username").Returns(usernameIndex);
            _reader.GetOrdinal("CustId").Returns(custIdIndex);
            _reader.GetOrdinal("Currency").Returns(currencyIndex);
            _reader.GetOrdinal("RoleID").Returns(roleIdIndex);
            _reader.GetOrdinal("CurrencyName").Returns(currencyNameIndex);

            _reader.GetString(usernameIndex).Returns(expected.Username);
            _reader.GetInt32(custIdIndex).Returns(expected.CustId);
            _reader.GetInt32(currencyIndex).Returns(expected.Currency);
            _reader.GetByte(roleIdIndex).Returns((byte)expected.RoleId);
            _reader.GetString(currencyNameIndex).Returns(expected.CurrencyName);
            _reader["Description"].Returns(expected.Description);
        }
    }
}