namespace Fanex.Athena.DataAccess.Report.UnitTests
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using Core.UnitTests.TestHelpers;
    using Model.Core.MetaModel.Product;
    using Model.Report.MetaModel;
    using NSubstitute;
    using NUnit.Framework;
    using VendorAdapter.Interfaces;

    [TestFixture]
    public class WinLossByLeagueDataAccessTest : BaseDataAccessHelperTest<WinLossByLeagueDataAccess>
    {
        private const string GetSummarySp = "Lrf_Rpt_WinlostByLeague";
        private const string GetCustomersSp = "Lrf_Rpt_WinlossByLeagueDetail";

        protected override void SetUpDataAccess()
        {
            DataAccess = Substitute.ForPartsOf<WinLossByLeagueDataAccess>(
                Substitute.For<ICacheRefAdapter>());
        }

        [TestCase(true, true)]
        [TestCase(false, false)]
        public void GetSummary_WhenCall_ParametersPassedCorrectly(bool isShowSB, bool isShowBA)
        {
            // Arrange
            var metaModel = new WinLossByLeagueMetaModel
            {
                FromDate = new DateTime(2017, 1, 1),
                ToDate = new DateTime(2017, 2, 2),
                AgentListXml = "xml",
                Odds = 1,
                League = 2,
                Match = 3,
                ShowWin = true,
                ShowLoss = true,
                ShowDraw = true,
                BalanceUpDay = new DateTime(2018, 1, 1),
                MoveBetsDay = new DateTime(2018, 2, 2),
                ProductOptions = new List<ProductOption>
                {
                    new ProductOption { Code = "SB", Status = isShowSB },
                    new ProductOption { Code = "BA", Status = isShowBA }
                }
            };
            DataAccess.GetObjectDb(Arg.Is(GetSummarySp)).Returns(ObjectDb);

            // Action
            DataAccess.GetSummary(metaModel);

            // Assert
            Assert.Multiple(() =>
            {
                ObjectDb.Received(1).SetParameterValue("balanceupday", metaModel.BalanceUpDay);
                AssertCommonSpParam(metaModel, isShowSB, isShowBA);
            });
        }

        [Test]
        public void GetSummary_WhenCall_ReturnCorrectResult()
        {
            // Arrange
            DataAccess.GetObjectDb(Arg.Is(GetSummarySp)).Returns(ObjectDb);

            Reader.Read().Returns(true, false);
            var readerDataMapping = new
            {
                turnoverUSD = 999
            };
            MapReaderData(readerDataMapping);

            // Action
            var result = DataAccess.GetSummary(new WinLossByLeagueMetaModel
            {
                BaseCurrency = "USD"
            });

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.Count(), Is.EqualTo(1));
                Assert.That(result.ElementAt(0).TurnOver, Is.EqualTo(999));
            });
        }

        [TestCase(true, true)]
        [TestCase(false, false)]
        public void GetCustomers_WhenCall_ParametersPassedCorrectly(bool showSB, bool showBA)
        {
            // Arrange
            var metaModel = new WinLossByLeagueMetaModel
            {
                FromDate = new DateTime(2017, 1, 1),
                ToDate = new DateTime(2017, 2, 2),
                AgentListXml = "xml",
                Odds = 1,
                League = 2,
                Match = 3,
                ShowWin = true,
                ShowLoss = true,
                ShowDraw = true,
                BalanceUpDay = new DateTime(2018, 1, 1),
                MoveBetsDay = new DateTime(2018, 2, 2),
                ProductOptions = new List<ProductOption>
                {
                    new ProductOption { Code = "SB", Status = showSB },
                    new ProductOption { Code = "BA", Status = showBA }
                },
                PageIndex = 1,
                PageSize = 100
            };
            DataAccess.GetObjectDb(Arg.Is(GetCustomersSp)).Returns(ObjectDb);
            ObjectDb.GetParameterValue("totalrows").Returns(1000);

            // Action
            var result = DataAccess.GetCustomers(metaModel);

            // Assert
            Assert.Multiple(() =>
            {
                ObjectDb.Received(1).SetParameterValue("pageindex", metaModel.PageIndex);
                ObjectDb.Received(1).SetParameterValue("pagesize", metaModel.PageSize);
                AssertCommonSpParam(metaModel, showSB, showBA);

                Assert.That(result.TotalRows, Is.EqualTo(1000));
            });
        }

        [Test]
        public void GetCustomers_WhenCall_ReturnCorrectResult()
        {
            // Arrange
            DataAccess.GetObjectDb(Arg.Is(GetCustomersSp)).Returns(ObjectDb);

            Reader.Read().Returns(true, false, true, false);
            Reader.NextResult().Returns(true);
            var readerDataMapping = new
            {
                turnoverUSD = 999
            };
            MapReaderData(readerDataMapping);

            // Action
            var result = DataAccess.GetCustomers(
                new WinLossByLeagueMetaModel
                {
                    BaseCurrency = "USD"
                });

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.DataList.Count, Is.EqualTo(1));
                Assert.That(result.DataList.ElementAt(0).TurnOver, Is.EqualTo(999));
                Assert.That(result.TotalData.TurnOver, Is.EqualTo(999));
            });
        }

        [Test]
        public void GetCustomers_HasSystemIdColumn_ReturnCorrectData()
        {
            // Arrange
            DataAccess.GetObjectDb(Arg.Is(GetCustomersSp)).Returns(ObjectDb);

            Reader.Read().Returns(true, false);
            Reader.NextResult().Returns(false);
            Reader.FieldCount.Returns(1);
            Reader.GetName(0).Returns("username");
            var readerDataMapping = new
            {
                username = "user test"
            };
            MapReaderData(readerDataMapping);

            // Action
            var result = DataAccess.GetCustomers(
                new WinLossByLeagueMetaModel
                {
                    BaseCurrency = "USD"
                });

            // Assert
            Assert.That(result.DataList.ElementAt(0).SystemId, Is.EqualTo(readerDataMapping.username));
        }

        [Test]
        public void GetCustomers_HasExternalIdColumn_ReturnCorrectData()
        {
            // Arrange
            DataAccess.GetObjectDb(Arg.Is(GetCustomersSp)).Returns(ObjectDb);

            Reader.Read().Returns(true, false);
            Reader.NextResult().Returns(false);
            Reader.FieldCount.Returns(1);
            Reader.GetName(0).Returns("nickname");
            var readerDataMapping = new
            {
                nickname = "nick name test"
            };
            MapReaderData(readerDataMapping);

            // Action
            var result = DataAccess.GetCustomers(
                new WinLossByLeagueMetaModel
                {
                    BaseCurrency = "USD"
                });

            // Assert
            Assert.That(result.DataList.ElementAt(0).ExternalId, Is.EqualTo(readerDataMapping.nickname));
        }

        [Test]
        public void GetCustomers_HasCustomerIdColumn_ReturnCorrectData()
        {
            // Arrange
            DataAccess.GetObjectDb(Arg.Is(GetCustomersSp)).Returns(ObjectDb);

            Reader.Read().Returns(true, false);
            Reader.NextResult().Returns(false);
            Reader.FieldCount.Returns(1);
            Reader.GetName(0).Returns("custid");
            var readerDataMapping = new
            {
                custid = 123
            };
            MapReaderData(readerDataMapping);

            // Action
            var result = DataAccess.GetCustomers(
                new WinLossByLeagueMetaModel
                {
                    BaseCurrency = "USD"
                });

            // Assert
            Assert.That(result.DataList.ElementAt(0).CustomerId, Is.EqualTo(readerDataMapping.custid));
        }

        private void AssertCommonSpParam(WinLossByLeagueMetaModel metaModel, bool showSB, bool showBA)
        {
            ObjectDb.Received(1).SetParameterValue("fromdate", metaModel.FromDate);
            ObjectDb.Received(1).SetParameterValue("todate", metaModel.ToDate);
            ObjectDb.Received(1).SetParameterValue("odds", metaModel.Odds);
            ObjectDb.Received(1).SetParameterValue("leagueid", metaModel.League);
            ObjectDb.Received(1).SetParameterValue("matchid", metaModel.Match);
            ObjectDb.Received(1).SetParameterValue("movebetsday", metaModel.MoveBetsDay);
            ObjectDb.Received(1).SetParameterValue("showwin", metaModel.ShowWin);
            ObjectDb.Received(1).SetParameterValue("showloss", metaModel.ShowLoss);
            ObjectDb.Received(1).SetParameterValue("showdraw", metaModel.ShowDraw);
            ObjectDb.Received(1).SetParameterValue("showsb", showSB);
            ObjectDb.Received(1).SetParameterValue("showba", showBA);
            ObjectDb.Received(1).SetParameterValue("AgentList", metaModel.AgentListXml);
        }
    }
}