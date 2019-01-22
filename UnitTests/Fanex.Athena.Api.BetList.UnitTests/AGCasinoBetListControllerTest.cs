namespace Fanex.Athena.Api.BetList.UnitTests
{
    using Model.BetList.MetaModel;
    using NSubstitute;
    using NUnit.Framework;

    [TestFixture]
    public class AGCasinoBetListControllerTest : ApiBetListBaseControllerHelperTest
    {
        [SetUp]
        public void SetUp()
        {
            MockSessionData();
        }

        [Test]
        public void Detail_WhenCall_GetBetListResultWithCorrectParameter()
        {
            // Arrange
            const string UspKey = "Lrf_BetList_LC_Detail";

            var controller = Substitute.ForPartsOf<AGCasinoBetListController>(
                SessionService,
                ProductService,
                LangService,
                BridgeClientService,
                BalanceUpDataAccess);

            var metaModel = new BetListMetaModel();

            controller.When(x => x.GetBetListResult(metaModel, UspKey)).DoNotCallBase();

            // Action
            controller.Detail(metaModel);

            // Assert
            controller.Received().GetBetListResult(metaModel, UspKey);
        }
    }
}