namespace Fanex.Athena.DataAccess.Report.UnitTests
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using Core.UnitTests.TestHelpers;
    using Model.Report.MetaModel;
    using NSubstitute;
    using NUnit.Framework;

    [TestFixture]
    public class MicroGamingWinLossDataAccessTest : BaseDataAccessHelperTest<MicroGamingWinLossDataAccess>
    {
        private const string SpName = "LRF_Rpt_MicroGamingWinloss";

        private readonly ThirdPartyWinLossMetaModel _metaModel = new ThirdPartyWinLossMetaModel
        {
            LicenseeId = 1,
            BaseCurrency = "USD",
            AgentListXml = "<xml>",
            FromDate = new DateTime(2017, 1, 1),
            ToDate = new DateTime(2017, 2, 2),
            ReportBy = 2
        };

        [Test]
        public void GetSummaryWinloss_WhenCall_ParametersPassedCorrectly()
        {
            // Arrange
            DataAccess.GetObjectDb(Arg.Is(SpName)).Returns(ObjectDb);

            // Action
            DataAccess.GetSummaryWinloss(_metaModel, UserCurrencyList);

            // Assert
            Assert.Multiple(() =>
            {
                ObjectDb.Received(1).AddInParameter("AgentList", DbType.String, _metaModel.AgentListXml);
                ObjectDb.Received(1).AddInParameter("licenseeid", DbType.Int32, _metaModel.LicenseeId);
                ObjectDb.Received(1).AddInParameter("fromdate", DbType.Date, _metaModel.FromDate);
                ObjectDb.Received(1).AddInParameter("todate", DbType.Date, _metaModel.ToDate);
                ObjectDb.Received(1).AddInParameter("ReportType", DbType.Int32, _metaModel.ReportBy);
            });
        }

        [Test]
        public void GetSummaryWinloss_WhenCall_ReturnCorrectResult()
        {
            // Arrange
            DataAccess.GetObjectDb(Arg.Is(SpName)).Returns(ObjectDb);

            Reader.Read().Returns(true, false);
            var readerDataMapping = new Dictionary<string, object>(StringComparer.InvariantCultureIgnoreCase)
            {
                { "typeid", 1 },
                { "winlostdate", "20170101" },
                { "TurnoverUSD", 100 }
            };

            MapReaderData(readerDataMapping);

            // Action
            var result = DataAccess.GetSummaryWinloss(_metaModel, UserCurrencyList);
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

        [Test]
        public void GetSummaryWinloss_WhenReceiveObjectDbException_ThrowDataAccessException()
        {
            TestException_WhenObjectDbThrowsException_ThrowDataAccessException(
                SpName,
                () => DataAccess.GetSummaryWinloss(_metaModel, UserCurrencyList));
        }
    }
}