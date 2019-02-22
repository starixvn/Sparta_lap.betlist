namespace Fanex.Athena.BetList.DataAccess.UnitTests
{
    using System.Collections;
    using Athena.BetList.Model.MetaModel;
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
                ObjectDb.Received(1).SetParameterValue("AgentList", metaModel.AgentListXml);
                ObjectDb.Received(1).SetParameterValue("FromDate", metaModel.FromDate);
                ObjectDb.Received(1).SetParameterValue("ToDate", metaModel.ToDate);
                ObjectDb.Received(1).SetParameterValue("MoveBetsDay", BalanceUp.MoveBetsDay);
                ObjectDb.Received(1).SetParameterValue("SportType", metaModel.SportId);
                ObjectDb.Received(1).SetParameterValue("BetType", metaModel.BetTypeId);
                ObjectDb.Received(1).SetParameterValue("LeagueId", metaModel.LeagueId);
                ObjectDb.Received(1).SetParameterValue("MatchId", metaModel.MatchId);
                ObjectDb.Received(1).SetParameterValue("BaseCurrency", metaModel.BaseCurrency);
                ObjectDb.Received(1).SetParameterValue("StatusIdList", metaModel.StatusList);
                ObjectDb.Received(1).SetParameterValue("LiveIndicator", metaModel.LiveStatus);
                ObjectDb.Received(1).SetParameterValue("PageSize", metaModel.PageSize);
                ObjectDb.Received(1).SetParameterValue("PageIndex", metaModel.PageIndex);
                ObjectDb.Received(1).SetParameterValue("ShowBA", showBA);
                ObjectDb.Received(1).SetParameterValue("ProductList", productList);

                ObjectDb.Received(1).SetParameterValue("TotalRows", null);
                ObjectDb.Received(1).SetParameterValue("SumStake", null);
                ObjectDb.Received(1).SetParameterValue("SumWinLost", null);
                ObjectDb.Received(1).SetParameterValue("SumLicenseeWL", null);
                ObjectDb.Received(1).SetParameterValue("SumComm", null);
                ObjectDb.Received(1).SetParameterValue("SumPlayerComm", null);
            });
        }
    }
}