namespace Fanex.Athena.DataAccess.BetList.UnitTests
{
    using System.Collections.Generic;
    using System.Data;
    using Data;
    using Model.BetList.DTO;
    using Model.BetList.MetaModel;
    using NSubstitute;
    using NUnit.Framework;

    [TestFixture]
    public class CancelledBetListDataAccessTest
    {
        private IObjectDb _objectDb;
        private CancelledBetListDataAccess _dataAccess;

        [OneTimeSetUp]
        public void SetUp()
        {
            _objectDb = Substitute.For<IObjectDb>();
            _dataAccess = Substitute.ForPartsOf<CancelledBetListDataAccess>();
            _dataAccess.GetObjectDb(Arg.Is<string>("Lrf_BetList_SelDeleted_UserList")).Returns(_objectDb);
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            _objectDb = null;
            _dataAccess = null;
        }

        [Test]
        public void GetCancelBetCustList_WhenCall_ParametersPassedCorrectly()
        {
            // Arrange
            var metaModel = GetFakeBetListMetaModel();

            // Action
            _dataAccess.GetCancelBetCustList(metaModel);

            // Assert
            _objectDb.Received().AddInParameter("licenseeid", DbType.Int32, metaModel.LicenseeId);
            _objectDb.Received().AddInParameter("userid", DbType.Int32, metaModel.UserId);
            _objectDb.Received().AddInParameter("LicenseeType", DbType.String, metaModel.LicenseeType);
            _objectDb.Received().AddInParameter("CurrencyId", DbType.String, metaModel.CurrencyId.ToString());
        }

        [Test]
        public void GetCancelBetCustList_WhenCall_DataReturnedCorrectly()
        {
            // Arrange
            var metaModel = GetFakeBetListMetaModel();
            var expected = GetFakeDataReturn();
            _objectDb.Query<CustomerCancelBetItem>().Returns(new List<CustomerCancelBetItem> { expected });

            // Action
            var result = _dataAccess.GetCancelBetCustList(metaModel)[0];

            // Assert
            Assert.That(result, Is.EqualTo(expected));
        }

        private BetListMetaModel GetFakeBetListMetaModel()
        {
            return new BetListMetaModel
            {
                LicenseeId = 1,
                UserId = 2,
                LicenseeType = "LicenseeTypeTest",
                CurrencyId = 3
            };
        }

        private CustomerCancelBetItem GetFakeDataReturn()
        {
            return new CustomerCancelBetItem
            {
                CustId = 1,
                UserName = "UserNameTest",
                NickName = "NickNameTest",
                BetCount = 10
            };
        }
    }
}