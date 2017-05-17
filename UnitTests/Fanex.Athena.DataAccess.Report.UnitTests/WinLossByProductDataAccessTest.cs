namespace Fanex.Athena.DataAccess.Report.UnitTests
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using Data;
    using Extension.Utility;
    using Model.Core.DTO.Common;
    using Model.Core.DTO.Configuration;
    using Model.Core.MetaModel.Product;
    using Model.Report.DTO;
    using Model.Report.MetaModel;
    using NSubstitute;
    using NUnit.Framework;

    [TestFixture]
    public class WinLossByProductDataAccessTest
    {
        private IAppSettings _appSetting;

        private WinLossByProductDataAccess _dataAccess;

        private WinLossByProductMetaModel _metaModel;

        private BalanceUp _balanceUp = new BalanceUp
        {
            BalanceUpDay = new DateTime(2018, 1, 1),
            MoveBetsDay = new DateTime(2018, 2, 2),
        };

        [OneTimeSetUp]
        public void Setup()
        {
            _appSetting = Substitute.For<IAppSettings>();
            _appSetting.HistoryDate.Returns(new DateTime(2016, 8, 31));
            _dataAccess = Substitute.ForPartsOf<WinLossByProductDataAccess>(_balanceUp);
            _metaModel = new WinLossByProductMetaModel(_appSetting)
            {
                ReportBy = 1,
                Username = "abc",
                AgentListXml = "<xml>",
                FromDate = new DateTime(2017, 1, 1),
                ToDate = new DateTime(2017, 2, 2),
                DrillDownFromDate = new DateTime(2018, 1, 1),
                DrillDownToDate = new DateTime(2018, 2, 2),
                DrawFilter = 2,
                MemberId = 3,
                CustomerId = 4,
                ProductOptions = new List<ProductOption>
                {
                    new ProductOption { Code = "SB", Status = true }
                },
                ProductList = new string[] { "SB" }
            };
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            _dataAccess = null;
            _appSetting = null;
        }

        [Test]
        public void GetWinlossByProducts_WhenCall_ParametersPassedCorrectly()
        {
            // Arrange
            var objectDb = Substitute.For<IObjectDb>();
            _dataAccess.GetObjectDb(Arg.Is<string>("Lrf_Rpt_WinlossByProduct")).Returns(objectDb);
            IDictionary<string, IList<WinLossByProduct>> summaryTotal;
            IResultInfo resultInfo;

            // Action
            _dataAccess.GetWinlossByProducts(_metaModel, out summaryTotal, out resultInfo);

            // Assert
            Assert.Multiple(() =>
            {
                objectDb.Received().AddInParameter("reporttype", DbType.Byte, _metaModel.ReportBy);
                objectDb.Received().AddInParameter("username", DbType.String, _metaModel.Username);
                objectDb.Received().AddInParameter("AgentList", DbType.String, _metaModel.AgentListXml);
                objectDb.Received().AddInParameter("fromdate", DbType.Date, _metaModel.FromDate.Date);
                objectDb.Received().AddInParameter("todate", DbType.Date, _metaModel.ToDate.Date);
                objectDb.Received().AddInParameter("isold", DbType.Boolean, _metaModel.IsOld);
                objectDb.Received().AddInParameter("drawfilter", DbType.Int32, _metaModel.DrawFilter);
                objectDb.Received().AddInParameter("movebetsday", DbType.DateTime, _balanceUp.MoveBetsDay);
                objectDb.Received().AddInParameter("showSB", DbType.Boolean, true);
            });
        }

        [Test]
        public void GetWinlossByProducts_WhenCall_ReturnCorrectResult()
        {
            // Arrange
            var reader = Substitute.For<IDataReader>();
            var objectDb = Substitute.For<IObjectDb>();
            _dataAccess.GetObjectDb(Arg.Is<string>("Lrf_Rpt_WinlossByProduct")).Returns(objectDb);
            objectDb.ExecuteReader().ReturnsForAnyArgs(reader);
            objectDb.GetParameterValue("CustID").Returns(1000);
            objectDb.GetParameterValue("SystemName").Returns("ABC");
            reader.Read().Returns(true, false);

            var readerDataMapping = new Dictionary<string, object>(StringComparer.InvariantCultureIgnoreCase)
            {
                { "TurnoverUSD", 999 },
                { "reporttype", "20190101" }
            };

            MapReaderData(reader, readerDataMapping);
            IDictionary<string, IList<WinLossByProduct>> summaryTotal;
            IResultInfo resultInfo;

            // Action
            var result = _dataAccess.GetWinlossByProducts(
                new WinLossByProductMetaModel(_appSetting)
                {
                    BaseCurrency = "USD"
                },
                out summaryTotal,
                out resultInfo);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.Count, Is.EqualTo(1));
                Assert.That(result[0].TurnOver, Is.EqualTo(999));
                Assert.That(result[0].Date, Is.EqualTo(new DateTime(2019, 1, 1)));

                Assert.That(resultInfo.ExtendedData, Is.EqualTo("1000;ABC"));
            });
        }

        [Test]
        public void GetWinlossByProductAgents_WhenCall_ParametersPassedCorrectly()
        {
            // Arrange
            var objectDb = Substitute.For<IObjectDb>();
            _dataAccess.GetObjectDb(Arg.Is<string>("Lrf_Rpt_WinlossByProductDetail_Ag")).Returns(objectDb);
            IDictionary<string, IList<WinLossByProduct>> summaryTotal;

            // Action
            _dataAccess.GetWinlossByProductAgents(_metaModel, out summaryTotal);

            // Assert
            Assert.Multiple(() =>
            {
                objectDb.Received().AddInParameter("CustID", DbType.Int32, _metaModel.MemberId);
                objectDb.Received().AddInParameter("AgentList", DbType.String, _metaModel.AgentListXml);
                objectDb.Received().AddInParameter("fromdate", DbType.Date, _metaModel.DrillDownFromDate.Date);
                objectDb.Received().AddInParameter("todate", DbType.Date, _metaModel.DrillDownToDate.Date);
                objectDb.Received().AddInParameter("isold", DbType.Boolean, _metaModel.IsOld);
                objectDb.Received().AddInParameter("drawfilter", DbType.Int32, _metaModel.DrawFilter);
                objectDb.Received().AddInParameter("movebetsday", DbType.DateTime, _balanceUp.MoveBetsDay);
                objectDb.Received().AddInParameter("showSB", DbType.Boolean, true);
            });
        }

        [Test]
        public void GetWinlossByProductCustomers_WhenCall_ParametersPassedCorrectly()
        {
            // Arrange
            var objectDb = Substitute.For<IObjectDb>();
            _dataAccess.GetObjectDb(Arg.Is<string>("Lrf_Rpt_WinlossByProductDetail")).Returns(objectDb);
            IDictionary<string, IList<WinLossByProduct>> summaryTotal;

            // Action
            _dataAccess.GetWinlossByProductCustomers(_metaModel, out summaryTotal);

            // Assert
            Assert.Multiple(() =>
            {
                objectDb.Received().AddInParameter("custid", DbType.Int32, _metaModel.CustomerId);
                objectDb.Received().AddInParameter("MemberID", DbType.Int32, _metaModel.MemberId);
                objectDb.Received().AddInParameter("AgentList", DbType.String, _metaModel.AgentListXml);
                objectDb.Received().AddInParameter("fromdate", DbType.Date, _metaModel.DrillDownFromDate.Date);
                objectDb.Received().AddInParameter("todate", DbType.Date, _metaModel.DrillDownToDate.Date);
                objectDb.Received().AddInParameter("isold", DbType.Boolean, _metaModel.IsOld);
                objectDb.Received().AddInParameter("drawfilter", DbType.Int32, _metaModel.DrawFilter);
                objectDb.Received().AddInParameter("movebetsday", DbType.DateTime, _balanceUp.MoveBetsDay);
                objectDb.Received().AddInParameter("showSB", DbType.Boolean, true);
            });
        }

        private static void MapReaderData(IDataReader reader, Dictionary<string, object> readerDataMapping)
        {
            reader[Arg.Any<string>()].Returns(x =>
            {
                var paramName = x.ArgAt<string>(0);
                if (readerDataMapping.ContainsKey(paramName))
                {
                    return readerDataMapping[paramName];
                }

                return string.Empty;
            });
        }
    }
}