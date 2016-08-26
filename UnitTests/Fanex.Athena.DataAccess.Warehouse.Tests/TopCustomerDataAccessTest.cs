namespace Fanex.Athena.DataAccess.Warehouse.Tests
{
    using System.Data;
    using Data;
    using Model.Warehouse.DTO;
    using Model.Warehouse.MetaModel;
    using NSubstitute;
    using NUnit.Framework;
    using Warehouse;

    [TestFixture]
    public class TopCustomerDataAccessTest
    {
        private IObjectDb _objectDb;
        private TopCustomerDataAccess _dataAccess;
        private IDataReader _reader;

        [SetUp]
        public void Setup()
        {
            _objectDb = Substitute.For<IObjectDb>();
            _dataAccess = Substitute.ForPartsOf<TopCustomerDataAccess>();
            _reader = Substitute.For<IDataReader>();

            _dataAccess.GetObjectDb(Arg.Is<string>("Lrf_rpt_DWTopCustomerSel")).Returns(_objectDb);
            _objectDb.ExecuteReader(Arg.Any<object>()).Returns(_reader);
        }

        [Test]
        public void GetDataList_WhenCall_DataReturnedCorrectly()
        {
            // Arrange
            var metaModel = new TopCustomerMetaModel();
            var expected = new TopCustomer
            {
                Member = string.Empty,
                Currency = "CurrencyTest",
                MemberWinLoss = 10m,
                BetCount = 1,
                Turnover = 10.5m
            };
            ConfigureReaderForGetDataListMethod(expected);

            // Action
            var result = _dataAccess.GetDataList(metaModel)[0];

            // Assert
            Assert.That(result.Member, Is.EqualTo(expected.Member));
            Assert.That(result.Currency, Is.EqualTo(expected.Currency));
            Assert.That(result.MemberWinLoss, Is.EqualTo(expected.MemberWinLoss));
            Assert.That(result.BetCount, Is.EqualTo(expected.BetCount));
            Assert.That(result.Turnover, Is.EqualTo(expected.Turnover));
        }

        [Test]
        public void GetDataForExcel_WhenCall_DataReturnedCorrectly()
        {
            // Arrange
            var metaModel = new TopCustomerMetaModel();
            var dataSet = new DataSet();
            var expected = new DataTable();
            dataSet.Tables.Add(expected);
            _objectDb.ExecuteDataSet(Arg.Any<object>()).Returns(dataSet);

            // Action
            var result = _dataAccess.GetDataForExcel(metaModel);

            // Assert
            Assert.That(result, Is.EqualTo(expected));
        }

        private void ConfigureReaderForGetDataListMethod(TopCustomer expected)
        {
            _reader.Read().Returns(true, false);

            _reader["UserName"].Returns(expected.Member);
            _reader["Currency"].Returns(expected.Currency);
            _reader["MemberWinlost"].Returns(expected.MemberWinLoss);
            _reader["betcount"].Returns(expected.BetCount);
            _reader["Turnover"].Returns(expected.Turnover);
            _reader["LastActiveDate"].Returns(expected.LastLoginDate);
        }
    }
}