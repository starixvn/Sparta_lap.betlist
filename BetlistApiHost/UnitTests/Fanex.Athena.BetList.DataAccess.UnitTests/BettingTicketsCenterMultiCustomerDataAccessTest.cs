namespace Fanex.Athena.BetList.DataAccess.UnitTests
{
    using System;
    using System.Collections;
    using Athena.BetList.Model.MetaModel;
    using Common.DataAccess.Interfaces;
    using Common.DataAccess.UnitTests;
    using Common.Model.DTO;
    using Common.Utility.Services.Interfaces;
    using NSubstitute;
    using NUnit.Framework;
    using UnitTest.Common.Helpers;

    public class BettingTicketsCenterMultiCustomerDataAccessTest : BaseDataAccessHelperTest<BettingTicketCenterMultiCustomerDataAccess>
    {
        private const string BetListByMultiCustomerUspKey = "Lrf_BetList_Licensee";

        private static readonly BalanceUp BalanceUp = new BalanceUp
        {
            MoveBetsDay = new DateTime(2016, 1, 1),
            BalanceUpDay = new DateTime(2016, 2, 1)
        };

        protected override void SetUpDataAccess()
        {
            var balanceUpDataAccess = Substitute.For<IBalanceUpDataAccess>();
            balanceUpDataAccess.GetBalanceUp().Returns(BalanceUp);
            DataAccess = Substitute.ForPartsOf<BettingTicketCenterMultiCustomerDataAccess>(
                balanceUpDataAccess,
                Substitute.For<ITempFileService>());
        }

        [Test]
        public void GetBetListForMultipleCustomers_WhenCall_ParametersPassedCorrectly()
        {
            // Arrange
            DataAccess.GetObjectDb(Arg.Is(BetListByMultiCustomerUspKey)).Returns(ObjectDb);
            ObjectDb.ExecuteReader().Returns(Reader);
            Reader.Read().Returns(false);
            ObjectDb.GetOuputParams().Returns(new Hashtable());

            var metaModel = FakeHelper.Create<MultipleCustomersBetListMetaModel>();
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
                ObjectDb.Received(1).SetParameterValue("StatusIdList", metaModel.Status);
                ObjectDb.Received(1).SetParameterValue("Username", metaModel.UserName);
                ObjectDb.Received(1).SetParameterValue("LiveIndicator", metaModel.LiveStatus);
                ObjectDb.Received(1).SetParameterValue("PageSize", metaModel.PageSize);
                ObjectDb.Received(1).SetParameterValue("PageIndex", metaModel.PageIndex);
                ObjectDb.Received(1).SetParameterValue("Product", metaModel.ProductCodeList);

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