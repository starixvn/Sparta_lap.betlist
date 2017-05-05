namespace Fanex.Athena.DataAccess.BetSettings.UnitTests
{
    using System.Data;
    using Data;
    using Model.Core.DataDomain;
    using NSubstitute;
    using NUnit.Framework;

    [TestFixture]
    public class DcsBetSettingDataAccessTest
    {
        private IObjectDb _objectDb;
        private DcsBetSettingDataAccess _dataAccess;

        [OneTimeSetUp]
        public void SetUp()
        {
            _objectDb = Substitute.For<IObjectDb>();
            _dataAccess = Substitute.ForPartsOf<DcsBetSettingDataAccess>();
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            _objectDb = null;
            _dataAccess = null;
        }

        [Test]
        public void GetStatus_WhenCall_ParameterPassedCorrectly()
        {
            // Arrange
            _dataAccess.GetObjectDb(Arg.Is<string>("Acc_Dep_CustSetting_StatusSettingSelect")).Returns(_objectDb);
            var custId = 1;
            var productId = 2;

            // Action
            _dataAccess.GetStatus(custId, productId);

            // Assert
            _objectDb.Received().AddInParameter("CustID", DbType.Int32, custId);
            _objectDb.Received().AddInParameter("ProductId", DbType.Int32, productId);
        }

        [TestCase(0, true)]
        [TestCase(1, false)]
        public void GetStatus_WhenCall_DataReturnCorrectly(int? isDisabled, bool expectedResult)
        {
            // Arrange
            _dataAccess.GetObjectDb(Arg.Is<string>("Acc_Dep_CustSetting_StatusSettingSelect")).Returns(_objectDb);
            var reader = Substitute.For<IDataReader>();
            _objectDb.ExecuteReader().ReturnsForAnyArgs(reader);
            reader.Read().Returns(true);
            reader["IsDisabled"].Returns(isDisabled);
            var custId = 1;
            var productId = 2;

            // Action
            var result = _dataAccess.GetStatus(custId, productId);

            // Assert
            Assert.That(result, Is.EqualTo(expectedResult));
        }

        [Test]
        public void UpdateStatus_WhenCall_ParameterPassedCorrectly()
        {
            // Arrange
            const string ExpectedIsDisabledParam = @"<ROOT><ProductStatus SportType=""211"" BetType=""2401"" IsDisabled=""0"" IsDisabledDownline=""0"" /></ROOT>";
            _dataAccess.GetObjectDb(Arg.Is<string>("Acc_Dep_CustSetting_DisableStatUpd")).Returns(_objectDb);
            var custId = 1;
            var userId = 2;
            var productId = ProductTypes.AllBet;
            var status = true;
            var ip = "local";
            _objectDb.GetParameterValue("ErrCode").Returns(1);

            // Action
            _dataAccess.UpdateStatus(userId, custId, ip, status, productId);

            // Assert
            _objectDb.Received().AddInParameter("custid", DbType.Int32, custId);
            _objectDb.Received().AddInParameter("userid", DbType.Int32, userId);
            _objectDb.Received().AddInParameter("IsDisabled", DbType.String, ExpectedIsDisabledParam);
            _objectDb.Received().AddInParameter("ip", DbType.String, ip);
            _objectDb.Received().AddOutParameter("ErrCode", DbType.Int32, 8);
            _objectDb.Received().AddOutParameter("ErrMsg", DbType.String, 8000);
        }

        [Test]
        public void UpdateStatus_WhenCall_DataReturnCorrectly()
        {
            // Arrange
            _dataAccess.GetObjectDb(Arg.Is<string>("Acc_Dep_CustSetting_DisableStatUpd")).Returns(_objectDb);
            _objectDb.GetParameterValue("ErrCode").Returns(1);
            _objectDb.GetParameterValue("ErrMsg").Returns("abc");

            // Action
            var result = _dataAccess.UpdateStatus(0, 0, string.Empty, false, ProductTypes.AllBet);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.Code, Is.EqualTo(1));
                Assert.That(result.Message, Is.EqualTo("abc"));
            });
        }
    }
}