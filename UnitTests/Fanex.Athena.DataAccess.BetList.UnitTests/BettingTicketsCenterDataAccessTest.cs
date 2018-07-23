namespace Fanex.Athena.DataAccess.BetList.UnitTests
{
    using System;
    using System.Collections;
    using System.Data;
    using Core.Configuration.Interfaces;
    using Core.UnitTests.TestHelpers;
    using Model.BetList.MetaModel;
    using Model.Core.DTO.Configuration;
    using NSubstitute;
    using NUnit.Framework;
    using UnitTest.Common.Helpers;

    public class BettingTicketsCenterDataAccessTest : BaseDataAccessHelperTest<BettingTicketCenterDataAccess>
    {
        private const string BetListByRefnoUspKey = "Lrf_BetList_RefNo_Mem";
        private const string RunningBetListUspKey = "Lrf_BetList_Running";

        private static readonly BalanceUp BalanceUp = new BalanceUp
        {
            MoveBetsDay = new DateTime(2016, 1, 1),
            BalanceUpDay = new DateTime(2016, 2, 1)
        };

        protected override void SetUpDataAccess()
        {
            var balanceUpDataAccess = Substitute.For<IBalanceUpDataAccess>();
            balanceUpDataAccess.GetBalanceUp().Returns(BalanceUp);
            DataAccess = Substitute.ForPartsOf<BettingTicketCenterDataAccess>(balanceUpDataAccess);
        }

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
                ObjectDb.Received(1).AddInParameter("UserName", DbType.String, metaModel.UserName);
                ObjectDb.Received(1).AddInParameter("RefNo", DbType.Int64, metaModel.RefNo);
                ObjectDb.Received(1).AddInParameter("MoveBetsDay", DbType.DateTime, BalanceUp.MoveBetsDay);
                ObjectDb.Received(1).AddInParameter("BalanceUpDay", DbType.DateTime, BalanceUp.BalanceUpDay);
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

        [TestCase("", "", false)]
        [TestCase("P1,SB,P2", "P1,SB,P2", false)]
        [TestCase("BA,P1,P2", "P1,P2", true)]
        [TestCase("P1,BA,P2,SB", "P1,P2,SB", true)]
        public void GetRunningBetList_WhenCall_ParametersPassedCorrectly(string productCode, string productList, bool showBA)
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
                ObjectDb.Received(1).AddInParameter("UserName", DbType.String, metaModel.UserName);
                ObjectDb.Received(1).AddInParameter("FromDate", DbType.DateTime, metaModel.FromDate);
                ObjectDb.Received(1).AddInParameter("ToDate", DbType.DateTime, metaModel.ToDate);
                ObjectDb.Received(1).AddInParameter("CurrencyId", DbType.Int32, metaModel.CurrencyId);
                ObjectDb.Received(1).AddInParameter("BaseCurrency", DbType.String, metaModel.BaseCurrency);
                ObjectDb.Received(1).AddInParameter("ProductList", DbType.String, productList);
                ObjectDb.Received(1).AddInParameter("showBA", DbType.Boolean, showBA);
                ObjectDb.Received(1).AddInParameter("PageSize", DbType.Int32, metaModel.PageSize);
                ObjectDb.Received(1).AddInParameter("PageIndex", DbType.Int32, metaModel.PageIndex);
                ObjectDb.Received(1).AddOutParameter("TotalRows", DbType.Int32, 4);
                ObjectDb.Received(1).AddOutParameter("SumStake", DbType.Currency, 8);
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
                { "sumstake", FakeHelper.Create<decimal>() },
                { "totalrows", FakeHelper.Create<int>() }
            };
            ObjectDb.GetOuputParams().Returns(outPut);

            var metaModel = new BaseBettingTicketMetaModel { ProductCodeList = string.Empty };

            // Action
            var result = DataAccess.GetRunningBetList(metaModel);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.SumStake, Is.EqualTo(outPut["sumstake"]));
                Assert.That(result.TotalRows, Is.EqualTo(outPut["totalrows"]));
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