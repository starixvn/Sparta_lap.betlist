namespace Fanex.Athena.DataAccess.Report.UnitTests
{
    using System;
    using System.Data;
    using System.Linq;
    using System.Threading.Tasks;
    using Core.UnitTests.TestHelpers;
    using Model.Report.DTO;
    using Model.Report.MetaModel;
    using NSubstitute;
    using NUnit.Framework;
    using Report;

    [TestFixture]
    public class ThirdPartyWinLosDataAccessTest : BaseDataAccessHelperTest<ThirdPartyWinLossDataAccess<ThirdPartyWinLoss>>
    {
        private ThirdPartyWinLossMetaModel _metaModel;

        [SetUp]
        public override void SetUp()
        {
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

            base.SetUp();
        }

        [TestCase("Test", "Test")]
        public void GetSummaryWinloss_WhenCall_ParametersPassedCorrectly(string spname, string expectedSpName)
        {
            // Arrange
            DataAccess.GetObjectDb(Arg.Is(expectedSpName)).Returns(ObjectDb);

            // Action
            DataAccess.GetSummaryWinloss(_metaModel, UserCurrencyList, spname);

            // Assert
            Assert.Multiple(() =>
            {
                DataAccess.Received(1).GetObjectDb(expectedSpName);
                ObjectDb.Received(1).SetParameterValue("ReportType", _metaModel.ReportBy);
                ObjectDb.Received(1).SetParameterValue("AgentList", _metaModel.AgentListXml);
                ObjectDb.Received(1).SetParameterValue("productid", _metaModel.ProductId);
                ObjectDb.Received(1).SetParameterValue("fromdate", _metaModel.FromDate);
                ObjectDb.Received(1).SetParameterValue("todate", _metaModel.ToDate);
            });
        }

        [Test]
        public async Task GetSummaryWinloss_WhenCall_ReturnCorrectResult()
        {
            // Arrange
            const string SpName = "Summary1";
            DataAccess.GetObjectDb(Arg.Is(SpName)).Returns(ObjectDb);

            Reader.Read().Returns(true, false);
            var readerDataMapping = new
            {
                winlostUSD = 100
            };

            MapReaderData(readerDataMapping);

            // Action
            var result = await DataAccess.GetSummaryWinloss(_metaModel, UserCurrencyList, SpName);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.DataList.Count, Is.EqualTo(1));
                Assert.That(result.DataList.First().CustomerWinLoss, Is.EqualTo(readerDataMapping.winlostUSD));

                Assert.That(result.SummaryTotal["USD"].CustomerWinLoss, Is.EqualTo(readerDataMapping.winlostUSD));
            });
        }

        [TestCase("Test", "Test")]
        public void GetCustomerWinlossDetail_WhenCall_ParametersPassedCorrectly(string spname, string expectedSpName)
        {
            // Arrange
            DataAccess.GetObjectDb(Arg.Is(expectedSpName)).Returns(ObjectDb);

            // Action
            DataAccess.GetCustomerWinlossDetail(_metaModel, UserCurrencyList, spname);

            // Assert
            Assert.Multiple(() =>
            {
                DataAccess.Received(1).GetObjectDb(expectedSpName);
                ObjectDb.Received(1).SetParameterValue("custid", _metaModel.DrillDownCustomerId);
                ObjectDb.Received(1).SetParameterValue("AgentList", _metaModel.AgentListXml);
                ObjectDb.Received(1).SetParameterValue("productid", _metaModel.ProductId);
                ObjectDb.Received(1).SetParameterValue("fromdate", _metaModel.DrillDownFromDate);
                ObjectDb.Received(1).SetParameterValue("todate", _metaModel.DrillDownToDate);
            });
        }

        [Test]
        public async Task GetCustomerWinlossDetail_WhenCall_ReturnCorrectResult()
        {
            // Arrange
            const string SpName = "Detail1";
            DataAccess.GetObjectDb(Arg.Is(SpName)).Returns(ObjectDb);

            Reader.Read().Returns(true, false);
            var readerDataMapping = new
            {
                winlostUSD = 100
            };

            MapReaderData(readerDataMapping);

            // Action
            var result = await DataAccess.GetCustomerWinlossDetail(_metaModel, UserCurrencyList, SpName);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.DataList.Count, Is.EqualTo(1));
                Assert.That(result.DataList.First().CustomerWinLoss, Is.EqualTo(readerDataMapping.winlostUSD));
            });
        }

        [TestCase("Test", "Test")]
        public async Task GetAgentWinlossDetail_WhenCall_ParametersPassedCorrectly(string spname, string expectedSpName)
        {
            // Arrange
            DataAccess.GetObjectDb(Arg.Is(expectedSpName)).Returns(ObjectDb);
            _metaModel.ProductId = -1;

            // Action
            await DataAccess.GetAgentWinlossDetail(_metaModel, UserCurrencyList, spname);

            // Assert
            Assert.Multiple(() =>
            {
                DataAccess.Received(1).GetObjectDb(expectedSpName);
                ObjectDb.Received(1).SetParameterValue("AgentList", _metaModel.AgentListXml);
                ObjectDb.DidNotReceive().SetParameterValue("productid", _metaModel.ProductId);
                ObjectDb.Received(1).SetParameterValue("fromdate", _metaModel.DrillDownFromDate);
                ObjectDb.Received(1).SetParameterValue("todate", _metaModel.DrillDownToDate);
            });
        }

        [Test]
        public async Task GetAgentWinlossDetail_WhenCall_ReturnCorrectResult()
        {
            // Arrange
            const string SpName = "Agent1";
            DataAccess.GetObjectDb(Arg.Is(SpName)).Returns(ObjectDb);

            Reader.Read().Returns(true, false);
            var readerDataMapping = new
            {
                winlostUSD = 100
            };

            MapReaderData(readerDataMapping);

            // Action
            var result = await DataAccess.GetAgentWinlossDetail(_metaModel, UserCurrencyList, SpName);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.DataList.Count, Is.EqualTo(1));
                Assert.That(result.DataList.First().CustomerWinLoss, Is.EqualTo(readerDataMapping.winlostUSD));
            });
        }

        [Test]
        public async Task GetData_HasSystemIdColumn_ReturnCorrectData()
        {
            // Arrange
            const string SpName = "Agent2";
            DataAccess.GetObjectDb(Arg.Is(SpName)).Returns(ObjectDb);

            Reader.Read().Returns(true, false);
            Reader.FieldCount.Returns(1);
            Reader.GetName(0).Returns("username");

            var readerDataMapping = new
            {
                username = "Test"
            };
            MapReaderData(readerDataMapping);

            // Action
            var result = await DataAccess.GetAgentWinlossDetail(_metaModel, UserCurrencyList, SpName);

            // Assert
            Assert.That(result.DataList.First().SystemId, Is.EqualTo(readerDataMapping.username));
        }

        [Test]
        public async Task GetData_HasExternalIdColumn_ReturnCorrectData()
        {
            // Arrange
            const string SpName = "Agent3";
            DataAccess.GetObjectDb(Arg.Is(SpName)).Returns(ObjectDb);

            Reader.Read().Returns(true, false);
            Reader.FieldCount.Returns(1);
            Reader.GetName(0).Returns("nickname");

            var readerDataMapping = new
            {
                nickname = "Test"
            };
            MapReaderData(readerDataMapping);

            // Action
            var result = await DataAccess.GetAgentWinlossDetail(_metaModel, UserCurrencyList, SpName);

            // Assert
            Assert.That(result.DataList.First().ExternalId, Is.EqualTo(readerDataMapping.nickname));
        }

        [Test]
        public async Task GetData_HasCustomerIdColumn_ReturnCorrectData()
        {
            // Arrange
            const string SpName = "Agent4";
            DataAccess.GetObjectDb(Arg.Is(SpName)).Returns(ObjectDb);

            Reader.Read().Returns(true, false);
            Reader.FieldCount.Returns(1);
            Reader.GetName(0).Returns("custid");

            var readerDataMapping = new
            {
                custid = 2
            };
            MapReaderData(readerDataMapping);

            // Action
            var result = await DataAccess.GetAgentWinlossDetail(_metaModel, UserCurrencyList, SpName);

            // Assert
            Assert.That(result.DataList.First().CustomerId, Is.EqualTo(readerDataMapping.custid));
        }

        [Test]
        public async Task GetData_HasWinlostDateColumn_ReturnCorrectData()
        {
            // Arrange
            const string SpName = "Agent5";
            DataAccess.GetObjectDb(Arg.Is(SpName)).Returns(ObjectDb);

            Reader.Read().Returns(true, false);
            Reader.FieldCount.Returns(1);
            Reader.GetName(0).Returns("winlostdate");

            var readerDataMapping = new
            {
                winlostdate = new DateTime(2017, 1, 2)
            };
            MapReaderData(readerDataMapping);

            // Action
            var result = await DataAccess.GetAgentWinlossDetail(_metaModel, UserCurrencyList, SpName);

            // Assert
            Assert.That(result.DataList.First().Date, Is.EqualTo(readerDataMapping.winlostdate));
        }
    }
}