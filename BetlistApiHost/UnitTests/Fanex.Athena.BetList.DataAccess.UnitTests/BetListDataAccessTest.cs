namespace Fanex.Athena.DataAccess.BetList.UnitTests
{
    using Athena.BetList.DataAccess;
    using Athena.BetList.Model.MetaModel;
    using Common.DataAcces.UnitTests;
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
            DataAccess.GetObjectDb(Arg.Is(GetMixParlayDetailTicketSp)).Returns(ObjectDb);
            var metaModel = new MixParlayDetailMetaModel
            {
                TransId = 123,
                CustId = 232,
                UserCurrencyList = UserCurrencyList
            };

            // Action
            DataAccess.GetMixParlayDetailTicket(metaModel);

            // Assert
            ObjectDb.Received(1).SetParameterValue("TransId", metaModel.TransId);
            ObjectDb.Received(1).SetParameterValue("CustId", metaModel.CustId);
        }
    }
}