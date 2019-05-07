namespace Fanex.Athena.DataAccess.Report.UnitTests
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Threading.Tasks;
    using Core.Configuration.Interfaces;
    using Core.UnitTests.TestHelpers;
    using Model.Core.DataDomain;
    using Model.Core.DTO.Configuration;
    using Model.Report.MetaModel;
    using NSubstitute;
    using NUnit.Framework;
    using Resources;

    [TestFixture]
    public class PlatformWinLossDataAccessTest : BaseDataAccessHelperTest<PlatformWinLossDataAccess>
    {
        private const string GetSummaryDataSp = "Lrf_Rpt_PlatformWinloss";
        private const string GetMemberDataSp = "Lrf_Rpt_PlatformWinlossDetail";

        private readonly BalanceUp _balance = new BalanceUp
        {
            MoveBetsDay = new DateTime(2016, 8, 10),
            BalanceUpDay = new DateTime(2016, 8, 31)
        };

        private readonly PlatformWinLossMetaModel _metaModel = new PlatformWinLossMetaModel
        {
            LicenseeId = 1,
            BaseCurrency = "USD",
            AgentListXml = "<xml>",
            Year = 2017,
            Month = 1,
            FromDay = 1,
            ToDay = 2
        };

        protected override void SetUpDataAccess()
        {
            var balanceUpDataAccess = Substitute.For<IBalanceUpDataAccess>();
            balanceUpDataAccess.GetBalanceUp().Returns(_balance);
            DataAccess = Substitute.ForPartsOf<PlatformWinLossDataAccess>(
                balanceUpDataAccess);
        }

        [Test]
        public async Task GetSummaryData_WhenCall_ParametersPassedCorrectly()
        {
            // Arrange
            DataAccess.GetObjectDb(Arg.Is(GetSummaryDataSp)).Returns(ObjectDb);

            // Action
            await DataAccess.GetSummaryData(_metaModel);

            // Assert
            AssertSpParameters();
        }

        [Test]
        public async Task GetSummaryData_WhenCall_ReturnCorrectResult()
        {
            // Arrange
            DataAccess.GetObjectDb(Arg.Is(GetSummaryDataSp)).Returns(ObjectDb);

            Reader.Read().Returns(true, true, false);
            var readerDataMapping = new Dictionary<string, object>(StringComparer.InvariantCultureIgnoreCase)
            {
                { "PlatformType", PlatformType.Asia },
                { "TurnoverUSD", 300 },
                { "productprefix", "SB" }
            };

            MapReaderData(readerDataMapping);

            // Action
            var result = await DataAccess.GetSummaryData(_metaModel);
            var firstItem = result.DataList.First();
            var secondItem = result.DataList.ElementAt(1);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.DataList.Count, Is.EqualTo(3));
                Assert.That(firstItem.PlatformType, Is.EqualTo(PlatformType.Asia));
                Assert.That(firstItem.Name, Is.EqualTo(ViewResources.Asia));
                Assert.That(firstItem.Turnover, Is.EqualTo(600));

                Assert.That(secondItem.Parent, Is.EqualTo(PlatformType.Asia.ToString()));
                Assert.That(secondItem.Name, Is.EqualTo(ViewResources.ProductSB));
                Assert.That(secondItem.Turnover, Is.EqualTo(300));

                Assert.That(result.TotalData.TurnoverPercent, Is.EqualTo(100));
            });
        }

        [Test]
        public async Task GetMemberData_WhenCall_ParametersPassedCorrectly()
        {
            // Arrange
            DataAccess.GetObjectDb(Arg.Is(GetMemberDataSp)).Returns(ObjectDb);

            // Action
            await DataAccess.GetMemberData(_metaModel, UserCurrencyList);

            // Assert
            AssertSpParameters();
        }

        [Test]
        public async Task GetMemberData_WhenCall_ReturnCorrectResult()
        {
            // Arrange
            DataAccess.GetObjectDb(Arg.Is(GetMemberDataSp)).Returns(ObjectDb);

            Reader.Read().Returns(true, false);
            var readerDataMapping = new
            {
                TurnoverUSD = 100,
                WinlostUSD = 200
            };

            MapReaderData(readerDataMapping);

            // Action
            var result = await DataAccess.GetMemberData(_metaModel, UserCurrencyList).ConfigureAwait(false);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.DataList.Count, Is.EqualTo(1));
                Assert.That(result.DataList.ElementAt(0).Turnover, Is.EqualTo(100));
                Assert.That(result.DataList.ElementAt(0).CustomerWinLoss, Is.EqualTo(200));

                Assert.That(result.TotalData.Turnover, Is.EqualTo(100));
            });
        }

        private void AssertSpParameters()
        {
            Assert.Multiple(() =>
            {
                ObjectDb.Received(1).SetParameterValue("PlatformType", _metaModel.PlatformType);
                ObjectDb.Received(1).SetParameterValue("movebetsday", _balance.MoveBetsDay);
                ObjectDb.Received(1).SetParameterValue("DrawFilter", _metaModel.DrawFilter);
                ObjectDb.Received(1).SetParameterValue("FromDate", _metaModel.FromDate);
                ObjectDb.Received(1).SetParameterValue("ToDate", _metaModel.ToDate);
                ObjectDb.Received(1).SetParameterValue("AgentList", _metaModel.AgentListXml);
            });
        }
    }
}