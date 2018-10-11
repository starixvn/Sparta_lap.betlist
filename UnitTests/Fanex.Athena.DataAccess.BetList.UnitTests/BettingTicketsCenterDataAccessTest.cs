namespace Fanex.Athena.DataAccess.BetList.UnitTests
{
    using System;
    using System.Collections;
    using System.Data;
    using Core.Configuration.Interfaces;
    using Core.UnitTests.TestHelpers;
    using Extension.Utility;
    using Model.BetList.MetaModel;
    using Model.Core.DTO.Configuration;
    using NSubstitute;
    using NUnit.Framework;
    using Session.Interfaces;
    using UnitTest.Common.Helpers;

    public class BettingTicketsCenterDataAccessTest : BaseDataAccessHelperTest<BettingTicketCenterDataAccess>
    {
        private const string BetListByRefnoUspKey = "Lrf_BetList_RefNo_Mem";
        private const string RunningBetListUspKey = "Lrf_BetList_Running";
        private const string BetListBySingleCustomerUspKey = "Lrf_BetList_Mem";

        private static readonly BalanceUp BalanceUp = new BalanceUp
        {
            MoveBetsDay = new DateTime(2016, 1, 1),
            BalanceUpDay = new DateTime(2016, 2, 1)
        };

        protected override void SetUpDataAccess()
        {
            var balanceUpDataAccess = Substitute.For<IBalanceUpDataAccess>();
            balanceUpDataAccess.GetBalanceUp().Returns(BalanceUp);
            DataAccess = Substitute.ForPartsOf<BettingTicketCenterDataAccess>(
                Substitute.For<IAppSettings>(),
                Substitute.For<ISessionStorage>(),
                balanceUpDataAccess);
        }

        [Test]
        public void GetBetListByRefno_WhenCall_ParameterIsPassesCorrectly()
        {
            // Arrange
            DataAccess.GetObjectDb(Arg.Is(BetListByRefnoUspKey)).Returns(ObjectDb);
            ObjectDb.ExecuteReader().Returns(Reader);
            Reader.Read().Returns(false);

            var metaModel = new RefNoBetListMetaModel();

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
                ObjectDb.Received(1).AddInParameter("ToDate", DbType.DateTime, metaModel.ToDate.AddDays(1).AddMilliseconds(-2));
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
        public void GetRunningBetList_Export_PagingParametersPassedCorrectly()
        {
            // Arrange
            DataAccess.GetObjectDb(Arg.Is(RunningBetListUspKey)).Returns(ObjectDb);
            ObjectDb.ExecuteReader().Returns(Reader);
            Reader.Read().Returns(false);
            ObjectDb.GetOuputParams().Returns(new Hashtable());

            var metaModel = new BaseBettingTicketMetaModel { ProductCodeList = string.Empty, IsExport = true };

            // Action
            DataAccess.GetRunningBetList(metaModel);

            // Assert
            Assert.Multiple(() =>
            {
                ObjectDb.Received(1).AddInParameter("PageSize", DbType.Int32, int.MaxValue - 1);
                ObjectDb.Received(1).AddInParameter("PageIndex", DbType.Int32, 1);
            });
        }

        [Test]
        public void GetRunningBetList_Sort_SortParametersPassedCorrectly()
        {
            // Arrange
            DataAccess.GetObjectDb(Arg.Is(RunningBetListUspKey)).Returns(ObjectDb);
            ObjectDb.ExecuteReader().Returns(Reader);
            Reader.Read().Returns(false);
            ObjectDb.GetOuputParams().Returns(new Hashtable());

            var metaModel = new BaseBettingTicketMetaModel { ProductCodeList = string.Empty, SortBy = "Colum1", SortOrder = "asc" };

            // Action
            DataAccess.GetRunningBetList(metaModel);

            // Assert
            ObjectDb.Received(1).AddInParameter("Sort", DbType.String, metaModel.SortBy + " " + metaModel.SortOrder);
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

        [TestCase("", "", false)]
        [TestCase("P1,SB,P2", "P1,SB,P2", false)]
        [TestCase("BA,P1,P2", "P1,P2", true)]
        [TestCase("P1,BA,P2,SB", "P1,P2,SB", true)]
        public void GetBetListForSingleCustomer_WhenCall_ParametersPassedCorrectly(string productCode, string productList, bool showBA)
        {
            // Arrange
            DataAccess.GetObjectDb(Arg.Is(BetListBySingleCustomerUspKey)).Returns(ObjectDb);
            ObjectDb.ExecuteReader().Returns(Reader);
            Reader.Read().Returns(false);
            ObjectDb.GetOuputParams().Returns(new Hashtable());

            var metaModel = new SingleCustomersBetListMetaModel { ProductCodeList = productCode };

            // Action
            DataAccess.GetBetListForSingleCustomer(metaModel);

            // Assert
            Assert.Multiple(() =>
            {
                ObjectDb.Received(1).AddInParameter("ShowBA", DbType.Boolean, showBA);
                ObjectDb.Received(1).AddInParameter("ProductList", DbType.String, productList);
                ObjectDb.Received(1).AddInParameter("FromDate", DbType.DateTime, metaModel.FromDate);
                ObjectDb.Received(1).AddInParameter("ToDate", DbType.DateTime, metaModel.ToDate);
                ObjectDb.Received(1).AddInParameter("AgentList", DbType.String, metaModel.AgentListXml);
                ObjectDb.Received(1).AddInParameter("UserName", DbType.String, metaModel.UserName);
                ObjectDb.Received(1).AddInParameter("StatusIdList", DbType.String, metaModel.Status);
                ObjectDb.Received(1).AddInParameter("LiveIndicator", DbType.Byte, metaModel.LiveStatus);
                ObjectDb.Received(1).AddInParameter("Platform", DbType.Byte, metaModel.Platform);
                ObjectDb.Received(1).AddInParameter("MoveBetsDay", DbType.DateTime, BalanceUp.MoveBetsDay);
                ObjectDb.Received(1).AddInParameter("BalanceUpDay", DbType.DateTime, BalanceUp.BalanceUpDay);
                ObjectDb.Received(1).AddInParameter("BaseCurrency", DbType.String, metaModel.BaseCurrency);
                ObjectDb.Received(1).AddInParameter("PageSize", DbType.Int32, metaModel.PageSize);
                ObjectDb.Received(1).AddInParameter("PageIndex", DbType.Int32, metaModel.PageIndex);
                ObjectDb.Received(1).AddOutParameter("TotalRows", DbType.Int32, 4);
                ObjectDb.Received(1).AddOutParameter("SumStake", DbType.Currency, 8);
                ObjectDb.Received(1).AddOutParameter("SumWinLost", DbType.Currency, 8);
                ObjectDb.Received(1).AddOutParameter("SumLicenseeWL", DbType.Currency, 8);
                ObjectDb.Received(1).AddOutParameter("SumComm", DbType.Currency, 8);
                ObjectDb.Received(1).AddOutParameter("SumPlayerComm", DbType.Currency, 8);
            });
        }

        [Test]
        public void GetBetListForSingleCustomer_Export_PagingParametersPassedCorrectly()
        {
            // Arrange
            DataAccess.GetObjectDb(Arg.Is(BetListBySingleCustomerUspKey)).Returns(ObjectDb);
            ObjectDb.ExecuteReader().Returns(Reader);
            Reader.Read().Returns(false);
            ObjectDb.GetOuputParams().Returns(new Hashtable());

            var metaModel = new SingleCustomersBetListMetaModel { ProductCodeList = string.Empty, IsExport = true };

            // Action
            DataAccess.GetBetListForSingleCustomer(metaModel);

            // Assert
            Assert.Multiple(() =>
            {
                ObjectDb.Received(1).AddInParameter("PageSize", DbType.Int32, int.MaxValue - 1);
                ObjectDb.Received(1).AddInParameter("PageIndex", DbType.Int32, 1);
            });
        }

        [Test]
        public void GetBetListForSingleCustomer_Sort_SortParametersPassedCorrectly()
        {
            // Arrange
            DataAccess.GetObjectDb(Arg.Is(BetListBySingleCustomerUspKey)).Returns(ObjectDb);
            ObjectDb.ExecuteReader().Returns(Reader);
            Reader.Read().Returns(false);
            ObjectDb.GetOuputParams().Returns(new Hashtable());

            var metaModel = new SingleCustomersBetListMetaModel { ProductCodeList = string.Empty, SortBy = "Colum1", SortOrder = "asc" };

            // Action
            DataAccess.GetBetListForSingleCustomer(metaModel);

            // Assert
            ObjectDb.Received(1).AddInParameter("Sort", DbType.String, metaModel.SortBy + " " + metaModel.SortOrder);
        }

        [Test]
        public void GetBetListForSingleCustomer_WhenCall_ReturnTotalDataCorrectly()
        {
            // Arrange
            DataAccess.GetObjectDb(Arg.Is(BetListBySingleCustomerUspKey)).Returns(ObjectDb);
            ObjectDb.ExecuteReader().Returns(Reader);
            Reader.Read().Returns(false);

            var outPut = new Hashtable
            {
                { "sumstake", FakeHelper.Create<decimal>() },
                { "totalrows", FakeHelper.Create<int>() },
                { "sumwinlost", FakeHelper.Create<decimal>() },
                { "sumlicenseewl", FakeHelper.Create<int>() },
                { "sumcomm", FakeHelper.Create<decimal>() },
                { "sumplayercomm", FakeHelper.Create<int>() }
            };
            ObjectDb.GetOuputParams().Returns(outPut);

            var metaModel = new SingleCustomersBetListMetaModel { ProductCodeList = string.Empty };

            // Action
            var result = DataAccess.GetBetListForSingleCustomer(metaModel);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.SumStake, Is.EqualTo(outPut["sumstake"]));
                Assert.That(result.TotalRows, Is.EqualTo(outPut["totalrows"]));
                Assert.That(result.SumWinLoss, Is.EqualTo(outPut["sumwinlost"]));
                Assert.That(result.SumLicenseeWL, Is.EqualTo(outPut["sumlicenseewl"]));
                Assert.That(result.SumComm, Is.EqualTo(outPut["sumcomm"]));
                Assert.That(result.SumPlayerComm, Is.EqualTo(outPut["sumplayercomm"]));
            });
        }

        [Test]
        public void GetBetListForSingleCustomer_WhenObjectDbThrowsException_ThrowDataAccessException()
        {
            TestException_WhenObjectDbThrowsException_ThrowDataAccessException(
                BetListBySingleCustomerUspKey,
                () => DataAccess.GetBetListForSingleCustomer(new SingleCustomersBetListMetaModel()));
        }
    }
}