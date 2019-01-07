namespace Fanex.Athena.DataAccess.BetList.UnitTests
{
    using System.Collections;
    using System.Data;
    using Model.BetList.MetaModel;
    using Model.Core.DTO.Configuration;
    using NSubstitute;
    using NUnit.Framework;
    using UnitTest.Common.Helpers;

    public partial class BettingTicketsCenterDataAccessTest
    {
        [TestCase("", "", false)]
        [TestCase("P1,SB,P2", "P1,SB,P2", false)]
        [TestCase("BA,P1,P2", "P1,P2", true)]
        [TestCase("P1,BA,P2,SB", "P1,P2,SB", true)]
        public void GetBetListForMultipleCustomers_WhenCall_ParametersPassedCorrectly(string productCode, string productList, bool showBA)
        {
            // Arrange
            DataAccess.GetObjectDb(Arg.Is(BetListByMultiCustomerUspKey)).Returns(ObjectDb);
            ObjectDb.ExecuteReader().Returns(Reader);
            Reader.Read().Returns(false);
            ObjectDb.GetOuputParams().Returns(new Hashtable());

            var metaModel = FakeHelper.Create<MultipleCustomersBetListMetaModel>(x => x.ProductCodeList, productCode);
            metaModel.IsExport = false;

            // Action
            DataAccess.GetBetListForMultipleCustomers(metaModel, UserCurrencyList);

            // Assert
            Assert.Multiple(() =>
            {
                ObjectDb.Received(1).AddInParameter("AgentList", DbType.String, metaModel.AgentListXml);
                ObjectDb.Received(1).AddInParameter("FromDate", DbType.DateTime, metaModel.FromDate);
                ObjectDb.Received(1).AddInParameter("ToDate", DbType.DateTime, metaModel.ToDate);
                ObjectDb.Received(1).AddInParameter("MoveBetsDay", DbType.DateTime, BalanceUp.MoveBetsDay);
                ObjectDb.Received(1).AddInParameter("SportType", DbType.Int32, metaModel.SportId);
                ObjectDb.Received(1).AddInParameter("BetType", DbType.Int32, metaModel.BetTypeId);
                ObjectDb.Received(1).AddInParameter("LeagueId", DbType.Int32, metaModel.LeagueId);
                ObjectDb.Received(1).AddInParameter("MatchId", DbType.Int32, metaModel.MatchId);
                ObjectDb.Received(1).AddInParameter("BaseCurrency", DbType.String, metaModel.BaseCurrency);
                ObjectDb.Received(1).AddInParameter("StatusIdList", DbType.String, metaModel.StatusList);
                ObjectDb.Received(1).AddInParameter("LiveIndicator", DbType.Byte, metaModel.LiveStatus);
                ObjectDb.Received(1).AddInParameter("PageSize", DbType.Int32, metaModel.PageSize);
                ObjectDb.Received(1).AddInParameter("PageIndex", DbType.Int32, metaModel.PageIndex);
                ObjectDb.Received(1).AddInParameter("ShowBA", DbType.Boolean, showBA);
                ObjectDb.Received(1).AddInParameter("ProductList", DbType.String, productList);

                ObjectDb.Received(1).AddOutParameter("TotalRows", DbType.Int32, 4);
                ObjectDb.Received(1).AddOutParameter("SumStake", DbType.Currency, 8);
                ObjectDb.Received(1).AddOutParameter("SumWinLost", DbType.Currency, 8);
                ObjectDb.Received(1).AddOutParameter("SumLicenseeWL", DbType.Currency, 8);
                ObjectDb.Received(1).AddOutParameter("SumComm", DbType.Currency, 8);
                ObjectDb.Received(1).AddOutParameter("SumPlayerComm", DbType.Currency, 8);
            });
        }
    }
}