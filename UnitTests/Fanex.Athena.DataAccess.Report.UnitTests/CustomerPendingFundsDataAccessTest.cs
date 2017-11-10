namespace Fanex.Athena.DataAccess.Report.UnitTests
{
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using Data;
    using Model.Core.MetaModel.Product;
    using Model.Report.DTO;
    using Model.Report.MetaModel;
    using NSubstitute;
    using NUnit.Framework;

    [TestFixture]
    public class CustomerPendingFundsDataAccessTest
    {
        private IObjectDb _objectDb;
        private CustomerPendingFundsDataAccess _dataAccess;
        private IDataReader _reader;
        private CustomerPendingFundsMetaModel _metaModel;

        [OneTimeSetUp]
        public void Setup()
        {
            _objectDb = Substitute.For<IObjectDb>();

            _dataAccess = Substitute.ForPartsOf<CustomerPendingFundsDataAccess>();
            _dataAccess.GetObjectDb(Arg.Is<string>("Lrf_Rpt_Outstanding")).Returns(_objectDb);

            _reader = Substitute.For<IDataReader>();
            _objectDb.ExecuteReader().Returns(_reader);

            _metaModel = new CustomerPendingFundsMetaModel
            {
                ProductOptions = GetProductOptions(),
                BaseCurrency = "UUS"
            };
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            _objectDb = null;
            _dataAccess = null;
            _reader = null;
            _metaModel = null;
        }

        [Test]
        public void GetDataList_WhenCall_ParametersPassedCorrectly()
        {
            // Action
            _dataAccess.GetDataList(_metaModel);

            // Assert
            _objectDb.Received().AddInParameter("pagesize", DbType.Int32, _metaModel.PageSize);
            _objectDb.Received().AddInParameter("pageindex", DbType.Int32, _metaModel.PageIndex);
            _objectDb.Received().AddInParameter("AgentList", DbType.String, _metaModel.AgentListXml);
            _objectDb.Received().AddInParameter("showSB", DbType.Boolean, true);
            _objectDb.Received().AddInParameter("showNG", DbType.Boolean, false);
        }

        [Test]
        public void GetDataList_WhenCall_DataReturnedCorrectly()
        {
            // Arrange
            _reader.Read().Returns(true, false, true, false);
            _reader.NextResult().Returns(true);

            var expectedTotalData = new CustPendingFunds
            {
                BetCounts = 1,
                PendingFunds = 20m
            };

            var expectedResult = new CustPendingFunds
            {
                CustId = 1,
                ExternalId = "ExternalIdTest",
                SystemId = "SystemIdTest",
                BetCounts = 10,
                PendingFunds = 15m
            };

            ConfigureReaderForTotalData(expectedTotalData);
            ConfigureReaderForResult(expectedResult);

            // Action
            var result = _dataAccess.GetDataList(_metaModel);
            var item = result.DataList.First();

            // Assert
            Assert.That(item.CustId, Is.EqualTo(expectedResult.CustId));
            Assert.That(item.ExternalId, Is.EqualTo(expectedResult.ExternalId));
            Assert.That(item.SystemId, Is.EqualTo(expectedResult.SystemId));
            Assert.That(item.BetCounts, Is.EqualTo(expectedResult.BetCounts));
            Assert.That(item.PendingFunds, Is.EqualTo(expectedResult.PendingFunds));

            Assert.That(result.TotalData.BetCounts, Is.EqualTo(expectedTotalData.BetCounts));
            Assert.That(result.TotalData.PendingFunds, Is.EqualTo(expectedTotalData.PendingFunds));

            Assert.That(result.TotalRows, Is.EqualTo(100));
        }

        private IList<ProductOption> GetProductOptions()
        {
            return new List<ProductOption>
            {
                new ProductOption { Code = "SB", Status = true },
                new ProductOption { Code = "NG", Status = false }
            };
        }

        private void ConfigureReaderForResult(CustPendingFunds expected)
        {
            var outstandingColumn = "Outstanding" + _metaModel.BaseCurrency;

            _reader["CustID"].Returns(expected.CustId);
            _reader["nickname"].Returns(expected.ExternalId);
            _reader["username"].Returns(expected.SystemId);
            _reader["BetCounts"].Returns(expected.BetCounts);
            _reader[outstandingColumn].Returns(expected.PendingFunds);
        }

        private void ConfigureReaderForTotalData(CustPendingFunds expected)
        {
            var outstandingColumn = "SumOutstanding" + _metaModel.BaseCurrency;

            _reader["SumBetCounts"].Returns(expected.BetCounts);
            _reader[outstandingColumn].Returns(expected.PendingFunds);

            _reader["TotalRows"].Returns(100);
        }
    }
}