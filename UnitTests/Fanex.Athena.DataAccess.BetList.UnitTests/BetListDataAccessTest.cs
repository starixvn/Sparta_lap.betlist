namespace Fanex.Athena.DataAccess.BetList.UnitTests
{
    using System.Data;
    using Core.UnitTests.TestHelpers;
    using Extension.Utility;
    using Model.BetList.MetaModel;
    using NSubstitute;
    using NUnit.Framework;
    using UnitTest.Common.Helpers;

    [TestFixture]
    public partial class BetListDataAccessTest : BaseDataAccessHelperTest<BetListDataAccess>
    {
        private const string GetMixParlayDetailTicketSp = "Lrf_BetList_SelParlayDetail";

        private readonly BetListMetaModel _metaModel;

        public BetListDataAccessTest()
        {
            _metaModel = FakeHelper.Create<BetListMetaModel>(x => x.ProductCodeList, "SB,NG,BA");
        }

        protected override void SetUpDataAccess()
        {
            DataAccess = Substitute.ForPartsOf<BetListDataAccess>();
        }

        [Test]
        public void GetMixParlayDetailTicket_WhenCall_ParametersPassedCorrectly()
        {
            // Arrange
            const long TransId = 1;
            const int CustId = 2;

            DataAccess.GetObjectDb(Arg.Is(GetMixParlayDetailTicketSp)).Returns(ObjectDb);

            // Action
            DataAccess.GetMixParlayDetailTicket(UserCurrencyList, TransId, CustId);

            // Assert
            ObjectDb.Received(1).AddInParameter("TransId", DbType.Int64, TransId);
        }
    }
}