namespace Fanex.Athena.DataAccess.Report.UnitTests
{
    using System;
    using System.Data;
    using System.Linq;
    using Data;
    using Model.Report.MetaModel;
    using NSubstitute;
    using NUnit.Framework;
    using Report;
    using UnitTest.Common.Helpers;

    [TestFixture]
    public class ThirdPartyWinLosDataAccessTest
    {
        private ThirdPartyWinLossDataAccess _dataAccess;
        private ThirdPartyWinLossMetaModel _metaModel;

        [OneTimeSetUp]
        public void Setup()
        {
            _dataAccess = Substitute.ForPartsOf<ThirdPartyWinLossDataAccess>();

            _metaModel = new ThirdPartyWinLossMetaModel
            {
                BaseCurrency = "USD",
                ReportBy = 1,
                ProductId = 2,
                AgentListXml = "xml",
                FromDate = new DateTime(2017, 1, 1),
                ToDate = new DateTime(2017, 2, 2),
                DrillDownFromDate = new DateTime(2018, 1, 1),
                DrillDownToDate = new DateTime(2018, 2, 2),
                DrillDownCustomerId = 999
            };
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            _dataAccess = null;
            _metaModel = null;
        }

        [Test]
        public void GetSummaryWinloss_WhenCall_ParametersPassedCorrectly()
        {
            // Arrange
            var objectDb = Substitute.For<IObjectDb>();
            _dataAccess.GetObjectDb(Arg.Is("Lrf_Rpt_ThirdPartyWinloss")).Returns(objectDb);

            // Action
            _dataAccess.GetSummaryWinloss(_metaModel);

            // Assert
            Assert.Multiple(() =>
            {
                objectDb.Received().AddInParameter("ReportType", DbType.Int32, _metaModel.ReportBy);
                objectDb.Received().AddInParameter("AgentList", DbType.String, _metaModel.AgentListXml);
                objectDb.Received().AddInParameter("productid", DbType.Int32, _metaModel.ProductId);
                objectDb.Received().AddInParameter("fromdate", DbType.Date, _metaModel.FromDate);
                objectDb.Received().AddInParameter("todate", DbType.Date, _metaModel.ToDate);
            });
        }

        [Test]
        public void GetSummaryWinloss_WhenCall_ParametersPassedCorrectly_WhenCall_ReturnCorrectResult()
        {
            // Arrange
            var reader = Substitute.For<IDataReader>();
            var objectDb = Substitute.For<IObjectDb>();
            _dataAccess.GetObjectDb(Arg.Is("Lrf_Rpt_ThirdPartyWinloss")).Returns(objectDb);
            objectDb.ExecuteReader().ReturnsForAnyArgs(reader);
            reader.Read().Returns(true, false);
            var readerDataMapping = new
            {
                winlostUSD = 100
            };

            DataAccessTestHelper.MapReaderData(reader, readerDataMapping);

            // Action
            var result = _dataAccess.GetSummaryWinloss(_metaModel);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.DataList.Count, Is.EqualTo(1));
                Assert.That(result.DataList.First().CustomerWinLoss, Is.EqualTo(100));

                Assert.That(result.SummaryTotal["USD"].CustomerWinLoss, Is.EqualTo(100));
            });
        }

        [Test]
        public void GetCustomerWinlossDetail_WhenCall_ParametersPassedCorrectly()
        {
            // Arrange
            var objectDb = Substitute.For<IObjectDb>();
            _dataAccess.GetObjectDb(Arg.Is<string>("LRF_Rpt_ThirdPartyWinloss_Mem")).Returns(objectDb);

            // Action
            _dataAccess.GetCustomerWinlossDetail(_metaModel);

            // Assert
            Assert.Multiple(() =>
            {
                objectDb.Received().AddInParameter("custid", DbType.Int32, _metaModel.DrillDownCustomerId);
                objectDb.Received().AddInParameter("AgentList", DbType.String, _metaModel.AgentListXml);
                objectDb.Received().AddInParameter("productid", DbType.Int32, _metaModel.ProductId);
                objectDb.Received().AddInParameter("fromdate", DbType.Date, _metaModel.DrillDownFromDate);
                objectDb.Received().AddInParameter("todate", DbType.Date, _metaModel.DrillDownToDate);
            });
        }

        [Test]
        public void GetCustomerWinlossDetail_WhenCall_ParametersPassedCorrectly_WhenCall_ReturnCorrectResult()
        {
            // Arrange
            var reader = Substitute.For<IDataReader>();
            var objectDb = Substitute.For<IObjectDb>();
            _dataAccess.GetObjectDb(Arg.Is<string>("LRF_Rpt_ThirdPartyWinloss_Mem")).Returns(objectDb);
            objectDb.ExecuteReader().ReturnsForAnyArgs(reader);
            reader.Read().Returns(true, false);
            var readerDataMapping = new
            {
                winlostUSD = 100
            };

            DataAccessTestHelper.MapReaderData(reader, readerDataMapping);

            // Action
            var result = _dataAccess.GetCustomerWinlossDetail(_metaModel);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.DataList.Count, Is.EqualTo(1));
                Assert.That(result.DataList.First().CustomerWinLoss, Is.EqualTo(100));
            });
        }

        [Test]
        public void GetAgentWinlossDetail_WhenCall_ParametersPassedCorrectly()
        {
            // Arrange
            var objectDb = Substitute.For<IObjectDb>();
            _dataAccess.GetObjectDb(Arg.Is<string>("LRF_Rpt_ThirdPartyWinloss_Ag")).Returns(objectDb);

            // Action
            _dataAccess.GetAgentWinlossDetail(_metaModel);

            // Assert
            Assert.Multiple(() =>
            {
                objectDb.Received().AddInParameter("AgentList", DbType.String, _metaModel.AgentListXml);
                objectDb.Received().AddInParameter("productid", DbType.Int32, _metaModel.ProductId);
                objectDb.Received().AddInParameter("fromdate", DbType.Date, _metaModel.DrillDownFromDate);
                objectDb.Received().AddInParameter("todate", DbType.Date, _metaModel.DrillDownToDate);
            });
        }

        [Test]
        public void GetAgentWinlossDetail_WhenCall_ParametersPassedCorrectly_WhenCall_ReturnCorrectResult()
        {
            // Arrange
            var reader = Substitute.For<IDataReader>();
            var objectDb = Substitute.For<IObjectDb>();
            _dataAccess.GetObjectDb(Arg.Is<string>("LRF_Rpt_ThirdPartyWinloss_Ag")).Returns(objectDb);
            objectDb.ExecuteReader().ReturnsForAnyArgs(reader);
            reader.Read().Returns(true, false);
            var readerDataMapping = new
            {
                winlostUSD = 100
            };

            DataAccessTestHelper.MapReaderData(reader, readerDataMapping);

            // Action
            var result = _dataAccess.GetAgentWinlossDetail(_metaModel);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.DataList.Count, Is.EqualTo(1));
                Assert.That(result.DataList.First().CustomerWinLoss, Is.EqualTo(100));
            });
        }
    }
}