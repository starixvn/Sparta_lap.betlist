namespace Fanex.Athena.DataAccess.Core.UnitTests.Users
{
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
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

        [OneTimeSetUp]
        public void Setup()
        {
            _objectDb = Substitute.For<IObjectDb>();
            _dataAccess = Substitute.ForPartsOf<LicenseeDataAccess>();
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
        public void IsAllowUpdateMemberBAStatus_WhenCall_ParametersPassedCorrectly()
        {
            // Arrange
            _dataAccess.GetObjectDb(Arg.Is("Lrf_BATimeCheck")).Returns(_objectDb);
            var request = new Request { Language = "en-US" };
            var licenseeId = 1;

            // Action
            _dataAccess.IsAllowUpdateMemberBAStatus(request, licenseeId);

            // Assert
            _objectDb.Received().AddInParameter("langkey", DbType.String, request.Language);
            _objectDb.Received().AddInParameter("LicenseeID", DbType.Int32, licenseeId);
            _objectDb.Received().AddOutParameter("isallow", DbType.Boolean, sizeof(bool));
        }

        [Test]
        public void IsAllowUpdateMemberBAStatus_WhenCall_ValueReturnedCorrectly()
        {
            // Arrange
            _dataAccess.GetObjectDb(Arg.Is("Lrf_BATimeCheck")).Returns(_objectDb);
            _objectDb.GetParameterValue("isallow").Returns(true);
            var request = new Request { Language = "en-US" };
            var licenseeId = 1;
            ResultInfoHelper.MockResultInfo(_objectDb);

            // Action
            var result = _dataAccess.IsAllowUpdateMemberBAStatus(request, licenseeId);

            // Assert
            Assert.That(result.ExtendedData, Is.True);
            Assert.That(result.Code, Is.EqualTo(ResultInfoHelper.ResultCode));
            Assert.That(result.Message, Is.EqualTo(ResultInfoHelper.ResultMessage));
        }

        [TestCase(1, true)]
        [TestCase(2, false)]
        public void IsLicenseeBelongToUser_WhenCall_ValueReturnedCorrectly(int licenseeId, bool expectedResult)
        {
            // Arrange
            const int UserId = 1;
            _dataAccess.GetObjectDb(Arg.Is("Lrf_LicenseeSel")).Returns(_objectDb);
            _objectDb.ExecuteReader(Arg.Any<object>()).Returns(_reader);
            _reader.Read().Returns(true, false);
            _reader.GetInt32(0).ReturnsForAnyArgs(1);

            // Action
            var result = _dataAccess.IsLicenseeBelongToUser(licenseeId, UserId);

            // Assert
            Assert.That(result, Is.EqualTo(expectedResult));
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
            _dataAccess.GetObjectDb(Arg.Is("Lrf_LicenseeSel")).Returns(_objectDb);
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
        public void GetLicenseeByName_WhenCall_ValueReturnedCorrectly()
        {
            // Arrange
            _dataAccess.GetObjectDb(Arg.Is("Lrf_LicenseeSelBySiteName")).Returns(_objectDb);
            var expected = new Licensee
            {
                Id = 100
            };
            _objectDb.QueryEntity<Licensee>(Arg.Any<object>()).Returns(expected);

            // Action
            var result = _dataAccess.GetLicenseeByName(string.Empty);

            // Assert
            Assert.That(result.Id, Is.EqualTo(expected.Id));
        }

        [Test]
        public void GetLicenseeSourcedSettings_WhenCall_ValueReturnedCorrectly()
        {
            // Arrange
            _dataAccess.GetObjectDb(Arg.Is("LRF_LicenseeSourcedSel")).Returns(_objectDb);
            var expectedItem = new LicenseeSourcedSetting
            {
                SourcedAgentId = 1
            };
            _objectDb.Query<LicenseeSourcedSetting>(Arg.Any<object>()).Returns(
                new List<LicenseeSourcedSetting>
                {
                    expectedItem
                });

            // Action
            var result = _dataAccess.GetLicenseeSourcedSettings(0);

            // Assert
            Assert.That(result.First().SourcedAgentId, Is.EqualTo(expectedItem.SourcedAgentId));
        }
    }
}