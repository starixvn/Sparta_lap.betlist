namespace Fanex.Athena.DataAccess.Report.UnitTests
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using Data;
    using Extension.Utility;
    using Model.Core.DTO.Configuration;
    using Model.Core.MetaModel.Product;
    using Model.Report.DTO;
    using Model.Report.MetaModel;
    using NSubstitute;
    using NUnit.Framework;

    [TestFixture]
    public class LicenseeWinLossDataAccessTest
    {
        private IObjectDb _objectDb;
        private LicenseeWinLossDataAccess _dataAccess;
        private BalanceUp _balance;
        private IDataReader _reader;
        private LicenseeWinLossMetaModel _metaModel;

        [OneTimeSetUp]
        public void Setup()
        {
            _objectDb = Substitute.For<IObjectDb>();
            _balance = new BalanceUp
            {
                MoveBetsDay = new DateTime(2016, 8, 10),
                BalanceUpDay = new DateTime(2016, 8, 31)
            };

            _dataAccess = Substitute.ForPartsOf<LicenseeWinLossDataAccess>(_balance);
            _dataAccess.GetObjectDb(Arg.Is<string>("Lrf_Rpt_LicWinloss")).Returns(_objectDb);

            _reader = Substitute.For<IDataReader>();
            _objectDb.ExecuteReader().Returns(_reader);

            var appSetting = Substitute.For<IAppSettings>();
            appSetting.HistoryDate.Returns(new DateTime(2016, 8, 31));
            _metaModel = new LicenseeWinLossMetaModel(appSetting)
            {
                ProductOptions = GetProductOptions(),
                BaseCurrency = "UUS",
                ReportBy = 1,
                LicenseeId = 2,
                AgentListXml = "3"
            };
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            _objectDb = null;
            _dataAccess = null;
            _reader = null;
            _metaModel = null;
            _balance = null;
        }

        [Test]
        public void GetSummaryWinloss_WhenCall_ParametersPassedCorrectly()
        {
            // Arrange
            IDictionary<string, LicenseeWinLoss> summaryTotal;

            // Action
            _dataAccess.GetSummaryWinloss(_metaModel, out summaryTotal);

            // Assert
            _objectDb.Received().AddInParameter("reporttype", DbType.Byte, _metaModel.ReportBy);
            _objectDb.Received().AddInParameter("licenseeid", DbType.Int32, _metaModel.LicenseeId);
            _objectDb.Received().AddInParameter("AgentList", DbType.String, _metaModel.AgentListXml);
        }

        [Test]
        public void GetSummaryWinloss_WhenCall_ReturnSummaryTotalCorrectly()
        {
            // Arrange
            IDictionary<string, LicenseeWinLoss> summaryTotal;
            _reader.Read().Returns(true, false);
            _reader.FieldCount.Returns(2);
            _reader.GetName(0).Returns("LicenseeWLSBUSD");
            _reader.GetName(1).Returns("turnoverSBUSD");

            var readerDataMapping = new Dictionary<string, object>
            {
                { "LicenseeWLSBUSD", 100 },
                { "turnoverSBUSD", 200 },
                { "reporttype", "20170101" }
            };

            _reader[Arg.Any<string>()].Returns(x =>
            {
                var paramName = x.ArgAt<string>(0);
                if (readerDataMapping.ContainsKey(paramName))
                {
                    return readerDataMapping[paramName];
                }

                return string.Empty;
            });

            // Action
            _dataAccess.GetSummaryWinloss(_metaModel, out summaryTotal);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(summaryTotal["USD"].LicenseeWL["SB"], Is.EqualTo(100));
                Assert.That(summaryTotal["USD"].TurnOver["SB"], Is.EqualTo(200));
            });
        }

        private IList<ProductOption> GetProductOptions()
        {
            return new List<ProductOption>
            {
                new ProductOption { Code = "SB", Status = true },
                new ProductOption { Code = "NG", Status = false }
            };
        }
    }
}