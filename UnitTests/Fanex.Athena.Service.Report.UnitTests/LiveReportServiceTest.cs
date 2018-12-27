namespace Fanex.Athena.Service.Report.UnitTests
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using Constant.DataDomain;
    using DataAccess.Report.Interfaces;
    using Model.Core.DTO.Common;
    using Model.Report.DTO.LiveReports;
    using Model.Report.MetaModel;
    using NSubstitute;
    using NUnit.Framework;
    using VendorAdapter.Interfaces;

    public class LiveReportServiceTest
    {
        private readonly IDictionary<int, string> _currencyList = new Dictionary<int, string> { { 20, "UUS" } };
        private ILiveReportDataAccess _dataAccess;
        private LiveReportService _service;

        [SetUp]
        public void SetUp()
        {
            SynchronizationContext.SetSynchronizationContext(new SynchronizationContext());
            _dataAccess = Substitute.For<ILiveReportDataAccess>();
            _service = new LiveReportService(_dataAccess, Substitute.For<ICacheRefAdapter>());
        }

        [TearDown]
        public void TearDown()
        {
            _dataAccess = null;
            _service = null;
        }

        [Test]
        public void GetLiveReportSummary_InvalidMetaModel_ReturnEmptyData()
        {
            // Arrange
            var metaModel = new LiveReportMetaModel { FromDate = new System.DateTime(2017, 2, 1), ToDate = new System.DateTime(2017, 1, 1) };

            // Action
            var result = _service.GetLiveReportSummary(metaModel);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result, Is.Empty);
                _dataAccess.DidNotReceive().GetLiveReportSummary(metaModel);
            });
        }

        [Test]
        public void GetLiveReportSummary_ValidMetaModel_ReturnDataFromDataAccess()
        {
            // Arrange
            var metaModel = new LiveReportMetaModel();

            // Action
            var result = _service.GetLiveReportSummary(metaModel);

            // Assert
            _dataAccess.Received().GetLiveReportSummary(metaModel);
        }

        [Test]
        public void GetLiveReportSumaryDetail_InvalidMetaModel_ReturnEmptyData()
        {
            // Arrange
            var metaModel = new LiveReportMetaModel { FromDate = new System.DateTime(2017, 2, 1), ToDate = new System.DateTime(2017, 1, 1) };
            var products = Enumerable.Empty<Product>();

            // Action
            var result = _service.GetLiveReportSumaryDetail(metaModel, _currencyList, products);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.DataBySport, Is.Empty);
                Assert.That(result.DataByCurrency, Is.Empty);
                Assert.That(result.DataByBetType, Is.Empty);
                _dataAccess.DidNotReceive().GetDetailBySport(metaModel);
                _dataAccess.DidNotReceive().GetDetailByCurrencyAndBettype(metaModel, _currencyList, products);
            });
        }

        [Test]
        public void GetLiveReportSumaryDetail_ValidMetaModel_ReturnCorrectData()
        {
            // Arrange
            var metaModel = new LiveReportMetaModel();
            var products = Enumerable.Empty<Product>();

            var detailBySportData = new List<LiveReport>();
            var detailByCurrencyData = new List<LiveReport>();
            var detailByBettypeData = new List<LiveReportByBetType>();

            _dataAccess.GetDetailBySport(metaModel).Returns(detailBySportData);
            _dataAccess.GetDetailByCurrencyAndBettype(metaModel, _currencyList, products)
                .Returns(new System.Tuple<IList<LiveReport>, IList<LiveReportByBetType>>(detailByCurrencyData, detailByBettypeData));

            // Action
            var result = _service.GetLiveReportSumaryDetail(metaModel, _currencyList, products);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.DataBySport, Is.EqualTo(detailBySportData));
                Assert.That(result.DataByCurrency, Is.EqualTo(detailByCurrencyData));
                Assert.That(result.DataByBetType, Is.EqualTo(detailByBettypeData));
            });
        }

        [Test]
        public void GetLiveReportWinLossByMatch_InvalidMetaModel_ReturnEmptyData()
        {
            // Arrange
            var metaModel = new LiveReportMatchListMetalModel { FromDate = new System.DateTime(2017, 2, 1), ToDate = new System.DateTime(2017, 1, 1) };

            // Action
            var result = _service.GetLiveReportWinLossByMatch(metaModel);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result, Is.Empty);
                _dataAccess.DidNotReceive().GetLiveReportWinlossByMatch(metaModel);
            });
        }

        [TestCase((int)SportType.LiveCasino, "ShowTime")]
        [TestCase((int)SportType.AllBet, "vs")]
        public void GetLiveReportWinLossByMatch_ValidMetaModel_ReturnCorrectData(int sportType, string expectedMatchName)
        {
            // Arrange
            var metaModel = new LiveReportMatchListMetalModel { SportId = sportType };

            var dataList = new List<LiveReportWinLossByMatch>
            {
                new LiveReportWinLossByMatch { ShowTime = "ShowTime" }
            };

            _dataAccess.GetLiveReportWinlossByMatch(metaModel).Returns(dataList);

            // Action
            var result = _service.GetLiveReportWinLossByMatch(metaModel);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.Count, Is.EqualTo(dataList.Count));
                StringAssert.Contains(expectedMatchName, result[0].MatchName);
            });
        }
    }
}