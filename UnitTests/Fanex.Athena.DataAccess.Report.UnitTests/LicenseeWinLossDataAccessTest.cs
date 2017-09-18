namespace Fanex.Athena.DataAccess.Report.UnitTests
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using Core.Configuration.Interfaces;
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
        private LicenseeWinLossDataAccess _dataAccess;
        private BalanceUp _balance;
        private LicenseeWinLossMetaModel _metaModel;

        [OneTimeSetUp]
        public void Setup()
        {
            var balanceUpService = Substitute.For<IBalanceUpDataAccess>();
            _balance = new BalanceUp
            {
                MoveBetsDay = new DateTime(2016, 8, 10),
                BalanceUpDay = new DateTime(2016, 8, 31)
            };
            balanceUpService.GetBalanceUp().Returns(_balance);

            _dataAccess = Substitute.ForPartsOf<LicenseeWinLossDataAccess>(balanceUpService);

            var appSetting = Substitute.For<IAppSettings>();
            appSetting.HistoryDate.Returns(new DateTime(2016, 8, 31));
            _metaModel = new LicenseeWinLossMetaModel(appSetting)
            {
                DrillDownCustomerId = 999,
                DrillDownFromDate = new DateTime(2018, 1, 1),
                DrillDownToDate = new DateTime(2018, 2, 2),
                ProductOptions = GetProductOptions(),
                BaseCurrency = "USD",
                ReportBy = 1,
                LicenseeId = 2,
                AgentListXml = "3",
                FromDate = new DateTime(2017, 1, 1),
                ToDate = new DateTime(2017, 2, 2),
                DrawFilter = 4
            };
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            _dataAccess = null;
            _metaModel = null;
            _balance = null;
        }

        [Test]
        public void GetSummaryWinloss_WhenCall_ParametersPassedCorrectly()
        {
            // Arrange
            var objectDb = Substitute.For<IObjectDb>();
            _dataAccess.GetObjectDb(Arg.Is<string>("Lrf_Rpt_LicWinloss")).Returns(objectDb);
            IDictionary<string, LicenseeWinLoss> summaryTotal;

            // Action
            _dataAccess.GetSummaryWinloss(_metaModel, out summaryTotal);

            // Assert
            objectDb.Received().AddInParameter("reporttype", DbType.Byte, _metaModel.ReportBy);
            objectDb.Received().AddInParameter("licenseeid", DbType.Int32, _metaModel.LicenseeId);
            objectDb.Received().AddInParameter("AgentList", DbType.String, _metaModel.AgentListXml);
            objectDb.Received().AddInParameter("fromdate", DbType.Date, _metaModel.FromDate.Date);
            objectDb.Received().AddInParameter("todate", DbType.Date, _metaModel.ToDate.Date);
            objectDb.Received().AddInParameter("drawfilter", DbType.Int32, _metaModel.DrawFilter);
            objectDb.Received().AddInParameter("movebetsday", DbType.DateTime, _balance.MoveBetsDay);
            objectDb.Received().AddInParameter("showSB", DbType.Boolean, true);
            objectDb.Received().AddInParameter("showNG", DbType.Boolean, false);
        }

        [Test]
        public void GetSummaryWinloss_WhenCall_ReturnCorrectResult()
        {
            // Arrange
            var reader = Substitute.For<IDataReader>();
            var objectDb = Substitute.For<IObjectDb>();
            _dataAccess.GetObjectDb(Arg.Is<string>("Lrf_Rpt_LicWinloss")).Returns(objectDb);
            objectDb.ExecuteReader().Returns(reader);
            IDictionary<string, LicenseeWinLoss> summaryTotal;
            reader.Read().Returns(true, false);
            reader.FieldCount.Returns(3);
            reader.GetName(0).Returns("LicenseeWLSBUSD");
            reader.GetName(1).Returns("turnoverSBUSD");
            reader.GetName(2).Returns("username");

            var readerDataMapping = new Dictionary<string, object>(StringComparer.InvariantCultureIgnoreCase)
            {
                { "username", "testacc" },
                { "LicenseeWLSBUSD", 100 },
                { "turnoverSBUSD", 200 },
                { "reporttype", "20170101" }
            };

            reader[Arg.Any<string>()].Returns(x =>
            {
                var paramName = x.ArgAt<string>(0);
                if (readerDataMapping.ContainsKey(paramName))
                {
                    return readerDataMapping[paramName];
                }

                return string.Empty;
            });

            // Action
            var result = _dataAccess.GetSummaryWinloss(_metaModel, out summaryTotal);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.Count, Is.EqualTo(1));
                Assert.That(result[0].SystemId, Is.EqualTo("testacc"));
                Assert.That(result[0].LicenseeWL["SB"], Is.EqualTo(100));
                Assert.That(result[0].TurnOver["SB"], Is.EqualTo(200));
            });
        }

        [Test]
        public void GetSummaryWinloss_WhenCall_ReturnSummaryTotalCorrectly()
        {
            // Arrange
            var reader = Substitute.For<IDataReader>();
            var objectDb = Substitute.For<IObjectDb>();
            _dataAccess.GetObjectDb(Arg.Is<string>("Lrf_Rpt_LicWinloss")).Returns(objectDb);
            objectDb.ExecuteReader().Returns(reader);
            IDictionary<string, LicenseeWinLoss> summaryTotal;
            reader.Read().Returns(true, false);
            reader.FieldCount.Returns(2);
            reader.GetName(0).Returns("LicenseeWLSBUSD");
            reader.GetName(1).Returns("turnoverSBUSD");

            var readerDataMapping = new Dictionary<string, object>(StringComparer.InvariantCultureIgnoreCase)
            {
                { "LicenseeWLSBUSD", 100 },
                { "turnoverSBUSD", 200 },
                { "reporttype", "20170101" }
            };

            reader[Arg.Any<string>()].Returns(x =>
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

        [Test]
        public void GetCustomerWinlossDetail_WhenCall_ParametersPassedCorrectly()
        {
            // Arrange
            var objectDb = Substitute.For<IObjectDb>();
            _dataAccess.GetObjectDb(Arg.Is<string>("Lrf_Rpt_LicWinlossDetail")).Returns(objectDb);
            IDictionary<string, LicenseeWinLoss> summaryTotal;

            // Action
            _dataAccess.GetCustomerWinlossDetail(_metaModel, out summaryTotal);

            // Assert
            objectDb.Received().AddInParameter("custid", DbType.Int32, _metaModel.DrillDownCustomerId);
            objectDb.Received().AddInParameter("licenseeid", DbType.Int32, _metaModel.LicenseeId);
            objectDb.Received().AddInParameter("AgentList", DbType.String, _metaModel.AgentListXml);
            objectDb.Received().AddInParameter("fromdate", DbType.Date, _metaModel.DrillDownFromDate);
            objectDb.Received().AddInParameter("todate", DbType.Date, _metaModel.DrillDownToDate);
            objectDb.Received().AddInParameter("drawfilter", DbType.Int32, _metaModel.DrawFilter);
            objectDb.Received().AddInParameter("showSB", DbType.Boolean, true);
            objectDb.Received().AddInParameter("showNG", DbType.Boolean, false);
        }

        [Test]
        public void GetAgentWinlossDetail_WhenCall_ParametersPassedCorrectly()
        {
            // Arrange
            var objectDb = Substitute.For<IObjectDb>();
            _dataAccess.GetObjectDb(Arg.Is<string>("Lrf_Rpt_LicWinlossDetail_Ag")).Returns(objectDb);
            IDictionary<string, LicenseeWinLoss> summaryTotal;

            // Action
            _dataAccess.GetAgentWinlossDetail(_metaModel, out summaryTotal);

            // Assert
            objectDb.Received().AddInParameter("licenseeid", DbType.Int32, _metaModel.LicenseeId);
            objectDb.Received().AddInParameter("AgentList", DbType.String, _metaModel.AgentListXml);
            objectDb.Received().AddInParameter("fromdate", DbType.Date, _metaModel.DrillDownFromDate);
            objectDb.Received().AddInParameter("todate", DbType.Date, _metaModel.DrillDownToDate);
            objectDb.Received().AddInParameter("drawfilter", DbType.Int32, _metaModel.DrawFilter);
            objectDb.Received().AddInParameter("showSB", DbType.Boolean, true);
            objectDb.Received().AddInParameter("showNG", DbType.Boolean, false);
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