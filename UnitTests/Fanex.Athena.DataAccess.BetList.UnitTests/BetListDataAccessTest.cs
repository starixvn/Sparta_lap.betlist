namespace Fanex.Athena.DataAccess.BetList.UnitTests
{
    using System;
    using System.Data;
    using Core.UnitTests.TestHelpers;
    using Model.BetList.MetaModel;
    using NSubstitute;
    using NUnit.Framework;

    [TestFixture]
    public partial class BetListDataAccessTest : BaseDataAccessHelperTest<BetListDataAccess>
    {
        private const string GetMixParlayDetailTicketSp = "Lrf_BetList_SelParlayDetail";

        private readonly BetListMetaModel _metaModel;

        public BetListDataAccessTest()
        {
            _metaModel = GetBetListMetaModel();
        }

        [Test]
        public void GetMixParlayDetailTicket_WhenCall_ParametersPassedCorrectly()
        {
            // Arrange
            const long TransId = 1;

            DataAccess.GetObjectDb(Arg.Is(GetMixParlayDetailTicketSp)).Returns(ObjectDb);            

            // Action
            DataAccess.GetMixParlayDetailTicket(TransId);

            // Assert
            ObjectDb.Received().AddInParameter("TransId", DbType.Int64, TransId);
        }

        [Test]
        public void GetMixParlayDetailTicket_WhenObjectDbThrowsException_ThrowDataAccessException()
        {
            TestException_WhenObjectDbThrowsException_ThrowDataAccessException(
                GetMixParlayDetailTicketSp,
                () => DataAccess.GetMixParlayDetailTicket(1));
        }

        private static BetListMetaModel GetBetListMetaModel()
        {
            return new BetListMetaModel
            {
                BaseCurrency = "BaseCurrencyTest",
                CustId = 1,
                FromDate = new DateTime(2016, 8, 10),
                ToDate = new DateTime(2016, 8, 19),
                MoveBetsDay = new DateTime(2016, 1, 1),
                BalanceUpDay = new DateTime(2016, 2, 1),
                PageSize = 10,
                PageIndex = 1,
                ExternalId = "ExternalIdTest",
                SystemId = "SystemIdTest",
                LicenseeId = 2,
                UserId = 3,
                CurrencyId = 4,
                ProductCodeList = "SB,NG",
                SortBy = "SortByTest",
                SortOrder = "SortOrderTest",
                LeagueId = 5,
                RaceNo = "RaceNoTest",
                DrawFilter = 6,
                MultipleBettype = "MultipleBettypeTest",
                LiveIndicator = 7,
                PlatformType = 9
            };
        }
    }
}