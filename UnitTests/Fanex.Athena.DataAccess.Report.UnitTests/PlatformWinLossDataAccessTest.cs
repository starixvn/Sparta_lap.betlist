namespace Fanex.Athena.DataAccess.Report.UnitTests
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using Core.Configuration.Interfaces;
    using Core.UnitTests.TestHelpers;
    using Data;
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

        private BalanceUp _balance;
        private PlatformWinLossMetaModel _metaModel;

        [OneTimeSetUp]
        public override void FixtureSetUp()
        {
            var balanceUpDataAccess = Substitute.For<IBalanceUpDataAccess>();
            _balance = new BalanceUp
            {
                MoveBetsDay = new DateTime(2016, 8, 10),
                BalanceUpDay = new DateTime(2016, 8, 31)
            };
            balanceUpDataAccess.GetBalanceUp().Returns(_balance);

            ObjectDb = Substitute.For<IObjectDb>();
            DataAccess = Substitute.ForPartsOf<PlatformWinLossDataAccess>(
                CreateSessionStorage(),
                balanceUpDataAccess);

            _metaModel = new PlatformWinLossMetaModel
            {
                LicenseeId = 1,
                BaseCurrency = "USD",
                AgentListXml = "<xml>",
                Year = 2017,
                Month = 1,
                FromDay = 1,
                ToDay = 2
            };
        }

        [OneTimeTearDown]
        public override void FixtureTearDown()
        {
            _metaModel = null;
            _balance = null;

            base.FixtureTearDown();
        }

        [Test]
        public void GetSummaryData_WhenCall_ParametersPassedCorrectly()
        {
            // Arrange
            DataAccess.GetObjectDb(Arg.Is(GetSummaryDataSp)).Returns(ObjectDb);

            // Action
            DataAccess.GetSummaryData(_metaModel);

            // Assert
            AssertSpParameters();
        }

        [Test]
        public void GetSummaryData_WhenCall_ReturnCorrectResult()
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
            var result = DataAccess.GetSummaryData(_metaModel);
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
        public void GetSummaryData_WhenReceiveObjectDbException_ThrowDataAccessException()
        {
            TestException_WhenObjectDbThrowsException_ThrowDataAccessException(
                GetSummaryDataSp,
                () => DataAccess.GetSummaryData(_metaModel));
        }

        [Test]
        public void GetMemberData_WhenCall_ParametersPassedCorrectly()
        {
            // Arrange
            DataAccess.GetObjectDb(Arg.Is(GetMemberDataSp)).Returns(ObjectDb);

            // Action
            DataAccess.GetMemberData(_metaModel);

            // Assert
            AssertSpParameters();
        }

        [Test]
        public void GetMemberData_WhenCall_ReturnCorrectResult()
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
            var result = DataAccess.GetMemberData(_metaModel);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.DataList.Count, Is.EqualTo(1));
                Assert.That(result.DataList.ElementAt(0).Turnover, Is.EqualTo(100));
                Assert.That(result.DataList.ElementAt(0).CustomerWinLoss, Is.EqualTo(200));

                Assert.That(result.TotalData.Turnover, Is.EqualTo(100));
            });
        }

        [Test]
        public void GetMemberData_WhenReceiveObjectDbException_ThrowDataAccessException()
        {
            TestException_WhenObjectDbThrowsException_ThrowDataAccessException(
                GetMemberDataSp,
                () => DataAccess.GetMemberData(_metaModel));
        }

        private void AssertSpParameters()
        {
            Assert.Multiple(() =>
            {
                ObjectDb.Received(1).AddInParameter("PlatformType", DbType.Byte, _metaModel.PlatformType);
                ObjectDb.Received(1).AddInParameter("movebetsday", DbType.DateTime, _balance.MoveBetsDay);
                ObjectDb.Received(1).AddInParameter("DrawFilter", DbType.Int16, _metaModel.DrawFilter);
                ObjectDb.Received(1).AddInParameter("FromDate", DbType.DateTime, _metaModel.FromDate);
                ObjectDb.Received(1).AddInParameter("ToDate", DbType.DateTime, _metaModel.ToDate);
                ObjectDb.Received(1).AddInParameter("AgentList", DbType.String, _metaModel.AgentListXml);
            });
        }
    }
}