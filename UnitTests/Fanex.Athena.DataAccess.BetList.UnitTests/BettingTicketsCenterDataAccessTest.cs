namespace Fanex.Athena.DataAccess.BetList.UnitTests
{
    using System.Collections;
    using System.Data;
    using Core.UnitTests.TestHelpers;
    using Model.BetList.MetaModel;
    using NSubstitute;
    using NUnit.Framework;
    using UnitTest.Common.Helpers;

    public class BettingTicketsCenterDataAccessTest : BaseDataAccessHelperTest<BettingTicketCenterDataAccess>
    {
        private const string BetListByRefnoUspKey = "Lrf_BetList_RefNo_Mem";
        private const string RunningBetListUspKey = "Lrf_BetList_Running";

        [Test]
        public void GetBetListByRefno_IsConvertAllIsTrue_BaseCurrencyParameterIsNotEmpty()
        {
            // Arrange
            DataAccess.GetObjectDb(Arg.Is(BetListByRefnoUspKey)).Returns(ObjectDb);
            ObjectDb.ExecuteReader().Returns(Reader);
            Reader.Read().Returns(false);

            var metaModel = new RefNoBetListMetaModel { IsConvertAll = true };

            // Action
            DataAccess.GetBetListByRefno(metaModel);

            // Assert
            Assert.Multiple(() =>
            {
                ObjectDb.Received(1).AddInParameter("AgentList", DbType.String, metaModel.AgentListXml);
                ObjectDb.Received(1).AddInParameter("AgentList", DbType.String, metaModel.AgentListXml);
                ObjectDb.Received(1).AddInParameter("custid", DbType.Int32, metaModel.UserName);
                ObjectDb.Received(1).AddInParameter("RefNo", DbType.Int64, metaModel.RefNo);
                ObjectDb.Received(1).AddInParameter("MoveBetsDay", DbType.DateTime, metaModel.MoveBetsDay);
                ObjectDb.Received(1).AddInParameter("BalanceUpDay", DbType.DateTime, metaModel.BalanceUpDay);
                ObjectDb.Received(1).AddInParameter("BaseCurrency", DbType.String, metaModel.BaseCurrency);
            });
        }

        [Test]
        public void GetBetListByRefno_IsConvertAllIsFalse_BaseCurrencyParameterIsEmpty()
        {
            // Arrange
            DataAccess.GetObjectDb(Arg.Is(BetListByRefnoUspKey)).Returns(ObjectDb);
            ObjectDb.ExecuteReader().Returns(Reader);
            Reader.Read().Returns(false);

            var metaModel = new RefNoBetListMetaModel { IsConvertAll = false };

            // Action
            DataAccess.GetBetListByRefno(metaModel);

            // Assert
            Assert.Multiple(() =>
            {
                ObjectDb.Received(1).AddInParameter("BaseCurrency", DbType.String, string.Empty);
            });
        }

        [Test]
        public void GetBetListByRefno_WhenObjectDbThrowsException_ThrowDataAccessException()
        {
            TestException_WhenObjectDbThrowsException_ThrowDataAccessException(
                BetListByRefnoUspKey,
                () => DataAccess.GetBetListByRefno(new RefNoBetListMetaModel()));
        }

        [TestCase("", false, false)]
        [TestCase("P1,SB,P2", true, false)]
        [TestCase("BA,P1,P2", false, true)]
        [TestCase("P1,BA,P2,SB", true, true)]
        public void GetRunningBetList_WhenCall_ParametersPassedCorrectly(string productCode, bool showSB, bool showBA)
        {
            // Arrange
            DataAccess.GetObjectDb(Arg.Is(RunningBetListUspKey)).Returns(ObjectDb);
            ObjectDb.ExecuteReader().Returns(Reader);
            Reader.Read().Returns(false);
            ObjectDb.GetOuputParams().Returns(new Hashtable());

            var metaModel = new BaseBettingTicketMetaModel { ProductCodeList = productCode };

            // Action
            DataAccess.GetRunningBetList(metaModel);

            // Assert
            Assert.Multiple(() =>
            {
                ObjectDb.Received(1).AddInParameter("AgentList", DbType.String, metaModel.AgentListXml);
                ObjectDb.Received(1).AddInParameter("custid", DbType.Int32, metaModel.UserName);
                ObjectDb.Received(1).AddInParameter("FromDate", DbType.DateTime, metaModel.FromDate);
                ObjectDb.Received(1).AddInParameter("ToDate", DbType.DateTime, metaModel.ToDate);
                ObjectDb.Received(1).AddInParameter("CurrencyId", DbType.Int32, metaModel.CurrencyId);
                ObjectDb.Received(1).AddInParameter("BaseCurrency", DbType.String, metaModel.BaseCurrency);
                ObjectDb.Received(1).AddInParameter("showSB", DbType.Boolean, showSB);
                ObjectDb.Received(1).AddInParameter("showBA", DbType.Boolean, showBA);
                ObjectDb.Received(1).AddInParameter("PageSize", DbType.Int32, metaModel.PageSize);
                ObjectDb.Received(1).AddInParameter("PageIndex", DbType.Int32, metaModel.PageIndex);
                ObjectDb.Received(1).AddOutParameter("TotalRows", DbType.Int32, 4);
                ObjectDb.Received(1).AddOutParameter("SumStake", DbType.Decimal, 8);
            });
        }

        [Test]
        public void GetRunningBetList_WhenCall_ReturnTotalDataCorrectly()
        {
            // Arrange
            DataAccess.GetObjectDb(Arg.Is(RunningBetListUspKey)).Returns(ObjectDb);
            ObjectDb.ExecuteReader().Returns(Reader);
            Reader.Read().Returns(false);

            var outPut = new Hashtable
            {
                { "SumStake", FakeHelper.Create<decimal>() },
                { "TotalRows", FakeHelper.Create<int>() }
            };
            ObjectDb.GetOuputParams().Returns(outPut);

            var metaModel = new BaseBettingTicketMetaModel { ProductCodeList = string.Empty };

            // Action
            var result = DataAccess.GetRunningBetList(metaModel);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.SumStake, Is.EqualTo(outPut["SumStake"]));
                Assert.That(result.TotalRows, Is.EqualTo(outPut["TotalRows"]));
            });
        }

        [Test]
        public void GetRunningBetList_WhenObjectDbThrowsException_ThrowDataAccessException()
        {
            TestException_WhenObjectDbThrowsException_ThrowDataAccessException(
                RunningBetListUspKey,
                () => DataAccess.GetRunningBetList(new BaseBettingTicketMetaModel()));
        }
    }
}