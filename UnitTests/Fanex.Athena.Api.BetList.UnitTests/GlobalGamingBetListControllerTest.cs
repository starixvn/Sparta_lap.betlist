namespace Fanex.Athena.Api.BetList.UnitTests
{
    using Model.BetList.MetaModel;
    using NSubstitute;
    using NUnit.Framework;

    public class GlobalGamingBetListControllerTest : ApiBetListBaseControllerHelperTest
    {
        private const string UspKey = "Lrf_BetList_RNG_Detail";

        [Test]
        public void Detail_WhenCall_GetBetListResultWithCorrectParameter()
        {
            // Arrange
            var controller = Substitute.ForPartsOf<GlobalGamingBetListController>(
                SessionService,
                ProductService,
                LangService,
                BridgeClientService,
                BalanceUpDataAccess);

            var metaModel = new BetListMetaModel();

            SessionService.GenerateAgentListXml(metaModel.CurrencyId).Returns("<Xml/>");

            controller.When(x => x.GetBetListResult(metaModel, UspKey)).DoNotCallBase();

            // Action
            controller.Detail(metaModel);

            // Assert
            controller.Received().GetBetListResult(metaModel, UspKey);
        }
    }
}