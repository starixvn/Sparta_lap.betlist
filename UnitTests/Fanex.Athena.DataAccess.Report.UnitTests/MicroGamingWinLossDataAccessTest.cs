namespace Fanex.Athena.DataAccess.Report.UnitTests
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using Data;
    using Model.Report.MetaModel;
    using NSubstitute;
    using NUnit.Framework;
    using UnitTest.Common.Helpers;

    [TestFixture]
    public class MicroGamingWinLossDataAccessTest
    {
        private MicroGamingWinLossDataAccess _dataAccess;

        private ThirdPartyWinLossMetaModel _metaModel;

        [OneTimeSetUp]
        public void Setup()
        {
            _dataAccess = Substitute.ForPartsOf<MicroGamingWinLossDataAccess>();

            _metaModel = new ThirdPartyWinLossMetaModel
            {
                LicenseeId = 1,
                BaseCurrency = "USD",
                AgentListXml = "<xml>",
                FromDate = new DateTime(2017, 1, 1),
                ToDate = new DateTime(2017, 2, 2),
                ReportBy = 2
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
            _dataAccess.GetObjectDb(Arg.Is<string>("LRF_Rpt_MicroGamingWinloss")).Returns(objectDb);

            // Action
            _dataAccess.GetSummaryWinloss(_metaModel);

            // Assert
            objectDb.Received().AddInParameter("AgentList", DbType.String, _metaModel.AgentListXml);
            objectDb.Received().AddInParameter("licenseeid", DbType.Int32, _metaModel.LicenseeId);
            objectDb.Received().AddInParameter("fromdate", DbType.Date, _metaModel.FromDate);
            objectDb.Received().AddInParameter("todate", DbType.Date, _metaModel.ToDate);
            objectDb.Received().AddInParameter("ReportType", DbType.Int32, _metaModel.ReportBy);
        }

        [Test]
        public void GetSummaryWinloss_WhenCall_ReturnCorrectResult()
        {
            // Arrange
            var reader = Substitute.For<IDataReader>();
            var objectDb = Substitute.For<IObjectDb>();
            _dataAccess.GetObjectDb(Arg.Is<string>("LRF_Rpt_MicroGamingWinloss")).Returns(objectDb);
            objectDb.ExecuteReader().Returns(reader);
            reader.Read().Returns(true, false);
            var readerDataMapping = new Dictionary<string, object>(StringComparer.InvariantCultureIgnoreCase)
            {
                { "typeid", 1 },
                { "winlostdate", "20170101" },
                { "TurnoverUSD", 100 }
            };

            DataAccessTestHelper.MapReaderData(reader, readerDataMapping);

            // Action
            var result = _dataAccess.GetSummaryWinloss(_metaModel);
            var firstItem = result.DataList.First();

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.DataList.Count, Is.EqualTo(1));
                Assert.That(firstItem.TypeID, Is.EqualTo(1));
                Assert.That(firstItem.Date, Is.EqualTo(new DateTime(2017, 1, 1)));
                Assert.That(firstItem.TurnOver, Is.EqualTo(100));

                Assert.That(result.SummaryTotal["USD"].TurnOver, Is.EqualTo(100));
            });
        }
    }
}