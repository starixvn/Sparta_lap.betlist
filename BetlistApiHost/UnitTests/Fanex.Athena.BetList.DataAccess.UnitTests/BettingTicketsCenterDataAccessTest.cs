namespace Fanex.Athena.DataAccess.BetList.UnitTests
{
    using System;
    using System.Data;
    using Core.Configuration.Interfaces;
    using Core.UnitTests.TestHelpers;
    using Extension.Utility.Interfaces;
    using Model.BetList.MetaModel;
    using Model.Core.DTO.Configuration;
    using NSubstitute;
    using NUnit.Framework;

    public partial class BettingTicketsCenterDataAccessTest : BaseDataAccessHelperTest<BettingTicketCenterDataAccess>
    {
        private const string BetListByRefnoUspKey = "Lrf_BetList_RefNo_Mem";
        private const string RunningBetListUspKey = "Lrf_BetList_Running";
        private const string BetListBySingleCustomerUspKey = "Lrf_BetList_Mem";
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
            DataAccess = Substitute.ForPartsOf<BettingTicketCenterDataAccess>(
                balanceUpDataAccess,
                Substitute.For<ITempFileService>());
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
            DataAccess.GetBetListByRefno(metaModel, UserCurrencyList);

            // Assert
            Assert.Multiple(() =>
            {
                ObjectDb.Received(1).SetParameterValue("AgentList", metaModel.AgentListXml);
                ObjectDb.Received(1).SetParameterValue("UserName", metaModel.UserName);
                ObjectDb.Received(1).SetParameterValue("RefNo", metaModel.RefNo);
                ObjectDb.Received(1).SetParameterValue("MoveBetsDay", BalanceUp.MoveBetsDay);
                ObjectDb.Received(1).SetParameterValue("BalanceUpDay", BalanceUp.BalanceUpDay);
                ObjectDb.Received(1).SetParameterValue("BaseCurrency", metaModel.BaseCurrency);
            });
        }
    }
}