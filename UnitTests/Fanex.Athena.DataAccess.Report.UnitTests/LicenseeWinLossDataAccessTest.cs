namespace Fanex.Athena.DataAccess.Report.UnitTests
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Threading.Tasks;
    using Core.Configuration.Interfaces;
    using Core.UnitTests.TestHelpers;
    using Data;
    using Model.Core.DTO.Configuration;
    using Model.Core.MetaModel.Product;
    using Model.Report.MetaModel;
    using NSubstitute;
    using NUnit.Framework;

    [TestFixture]
    public class LicenseeWinLossDataAccessTest : BaseDataAccessHelperTest<LicenseeWinLossDataAccess>
    {
        private const string GetSummaryWinlossSp = "Lrf_Rpt_LicWinloss";

        private const string GetCustomerWinlossDetailSp = "Lrf_Rpt_LicWinlossDetail";

        private const string GetAgentWinlossDetailSp = "Lrf_Rpt_LicWinlossDetail_Ag";

        private LicenseeWinLossMetaModel _metaModel;

        [OneTimeSetUp]
        public override void FixtureSetUp()
        {
            var balanceUpDataAccess = Substitute.For<IBalanceUpDataAccess>();
            var balance = new BalanceUp
            {
                MoveBetsDay = new DateTime(2016, 8, 10),
                BalanceUpDay = new DateTime(2016, 8, 31)
            };
            balanceUpDataAccess.GetBalanceUp().Returns(balance);

            ObjectDb = Substitute.For<IObjectDb>();
            DataAccess = Substitute.ForPartsOf<LicenseeWinLossDataAccess>(
                balanceUpDataAccess);

            _metaModel = new LicenseeWinLossMetaModel
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
        public override void FixtureTearDown()
        {
            _metaModel = null;

            base.FixtureTearDown();
        }

        [Test]
        public async Task GetSummaryWinloss_WhenCall_ParametersPassedCorrectly()
        {
            // Arrange
            DataAccess.GetObjectDb(Arg.Is(GetSummaryWinlossSp)).Returns(ObjectDb);

            // Action
            await DataAccess.GetSummaryWinloss(_metaModel, UserCurrencyList);

            // Assert
            Assert.Multiple(() =>
            {
                ObjectDb.Received(1).SetParameterValue("reporttype", _metaModel.ReportBy);
                AssertCommonSpParameters(isDrilldown: false);
            });
        }

        [Test]
        public async Task ReadData_WhenCall_ReturnCorrectResult()
        {
            // Arrange
            DataAccess.GetObjectDb(Arg.Is(GetSummaryWinlossSp)).Returns(ObjectDb);

            Reader.Read().Returns(true, false);
            Reader.FieldCount.Returns(5);
            Reader.GetName(0).Returns("LicenseeWLSBUSD");
            Reader.GetName(1).Returns("turnoverSBUSD");
            Reader.GetName(2).Returns("username");
            Reader.GetName(3).Returns("nickname");
            Reader.GetName(4).Returns("custid");

            var readerDataMapping = new
            {
                username = "testacc",
                LicenseeWLSBUSD = 100,
                turnoverSBUSD = 200,
                reporttype = "20170101",
                nickname = "nickname test",
                custid = 108,
                PromotionPrize = 1
            };
            MapReaderData(readerDataMapping);

            // Action
            var result = await DataAccess.GetSummaryWinloss(_metaModel, UserCurrencyList);
            var firstItem = result.DataList.First();

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.DataList.Count, Is.EqualTo(1));
                Assert.That(firstItem.SystemId, Is.EqualTo(readerDataMapping.username));
                Assert.That(firstItem.ExternalId, Is.EqualTo(readerDataMapping.nickname));
                Assert.That(firstItem.CustomerId, Is.EqualTo(readerDataMapping.custid));
                Assert.That(firstItem.LicenseeWL["SB"], Is.EqualTo(100));
                Assert.That(firstItem.TurnOver["SB"], Is.EqualTo(200));
                Assert.That(result.AdditionalInfo, Is.EqualTo(readerDataMapping.PromotionPrize.ToString()));
            });
        }

        [Test]
        public async Task ReadSummaryTotal_WhenCall_ReturnSummaryTotalCorrectly()
        {
            // Arrange
            DataAccess.GetObjectDb(Arg.Is(GetSummaryWinlossSp)).Returns(ObjectDb);

            Reader.Read().Returns(true, false);
            Reader.FieldCount.Returns(2);
            Reader.GetName(0).Returns("LicenseeWLSBUSD");
            Reader.GetName(1).Returns("turnoverSBUSD");

            var readerDataMapping = new
            {
                LicenseeWLSBUSD = 100,
                turnoverSBUSD = 200,
                reporttype = "20170101"
            };
            MapReaderData(readerDataMapping);

            // Action
            var result = await DataAccess.GetSummaryWinloss(_metaModel, UserCurrencyList);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.SummaryTotal["USD"].LicenseeWL["SB"], Is.EqualTo(100));
                Assert.That(result.SummaryTotal["USD"].TurnOver["SB"], Is.EqualTo(200));
            });
        }

        [Test]
        public async Task GetCustomerWinlossDetail_WhenCall_ParametersPassedCorrectly()
        {
            // Arrange
            DataAccess.GetObjectDb(Arg.Is(GetCustomerWinlossDetailSp)).Returns(ObjectDb);

            // Action
            await DataAccess.GetCustomerWinlossDetail(_metaModel, UserCurrencyList);

            // Assert
            Assert.Multiple(() =>
            {
                ObjectDb.Received(1).SetParameterValue("custid", _metaModel.DrillDownCustomerId);
                AssertCommonSpParameters(isDrilldown: true);
            });
        }

        [Test]
        public async Task GetAgentWinlossDetail_WhenCall_ParametersPassedCorrectly()
        {
            // Arrange
            DataAccess.GetObjectDb(Arg.Is(GetAgentWinlossDetailSp)).Returns(ObjectDb);

            // Action
            await DataAccess.GetAgentWinlossDetail(_metaModel, UserCurrencyList);

            // Assert
            AssertCommonSpParameters(isDrilldown: true);
        }

        private void AssertCommonSpParameters(bool isDrilldown)
        {
            ObjectDb.Received(1).SetParameterValue("licenseeid", _metaModel.LicenseeId);
            ObjectDb.Received(1).SetParameterValue("AgentList", _metaModel.AgentListXml);
            ObjectDb.Received(1).SetParameterValue("fromdate", isDrilldown ? _metaModel.DrillDownFromDate : _metaModel.FromDate);
            ObjectDb.Received(1).SetParameterValue("todate", isDrilldown ? _metaModel.DrillDownToDate : _metaModel.ToDate);
            ObjectDb.Received(1).SetParameterValue("drawfilter", _metaModel.DrawFilter);
            ObjectDb.Received(1).SetParameterValue("showSB", true);
            ObjectDb.Received(1).SetParameterValue("showNG", false);
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