namespace Fanex.Athena.DataAccess.Customer.UnitTests
{
    using System.Data;
    using Data;
    using Model.Customer.MetaModel.CustomerLog;
    using NSubstitute;
    using NUnit.Framework;

    [TestFixture]
    public class CustomerLogDataAccessTest
    {
        private IObjectDb _objectDb;
        private CustomerLogDataAccess _dataAccess;

        [SetUp]
        public void Setup()
        {
            _objectDb = Substitute.For<IObjectDb>();
            _dataAccess = Substitute.ForPartsOf<CustomerLogDataAccess>();
        }
        
        [Test]
        public void GetCustomerSettingList_WhenCall_ParametersPassedCorrectly()
        {
            // Arrange
            _dataAccess.GetObjectDb(Arg.Is<string>("Lrf_CustomerSettingLog_Sel")).Returns(_objectDb);
            var metaModel = new CustomerLogMetaModel();

            // Action
            _dataAccess.GetCustomerSettingList(metaModel);

            // Assert
            _objectDb.Received().AddInParameter("UserName", DbType.String, metaModel.UserName);
            _objectDb.Received().AddInParameter("FromDate", DbType.DateTime, metaModel.FromDate);
            _objectDb.Received().AddInParameter("ToDate", DbType.DateTime, metaModel.ToDate);
            _objectDb.Received().AddInParameter("FilterBy", DbType.Int16, metaModel.ViewById);
            _objectDb.Received().AddInParameter("LicenseeId", DbType.Int32, metaModel.LicenseeId);
        }

        [Test]
        public void GetGroupCommList_WhenCall_ParametersPassedCorrectly()
        {
            // Arrange
            _dataAccess.GetObjectDb(Arg.Is<string>("Lrf_GroupCommLog_Sel")).Returns(_objectDb);
            var metaModel = new CustomerLogMetaModel();

            // Action
            _dataAccess.GetGroupCommList(metaModel);

            // Assert
            _objectDb.Received().AddInParameter("FromDate", DbType.DateTime, metaModel.FromDate);
            _objectDb.Received().AddInParameter("ToDate", DbType.DateTime, metaModel.ToDate);
            _objectDb.Received().AddInParameter("LicenseeId", DbType.Int32, metaModel.LicenseeId);
        }
    }
}