namespace Fanex.Athena.Api.BetList.UnitTests
{
    using Model.BetList.MetaModel;
    using NSubstitute;
    using NUnit.Framework;

    public class DailyRacingWinLossBetListControllerTest : ApiBetListBaseControllerHelperTest
    {
        private const string UspKey = "Lrf_BetList_SelWinlostRacing";

        [Test]
        public void Index_WhenCall_GetBetListResultWithCorrectParameter()
        {
            // Arrange
            var controller = Substitute.ForPartsOf<DailyRacingWinLossBetListController>(
                SessionService,
                ProductService,
                LangService,
                BetListService,
                BalanceUpDataAccess,
                TicketHelper,
                CacheRefAdapter);

            var metaModel = new BetListMetaModel();

            SessionService.GenerateAgentListXml(metaModel.CurrencyId).Returns("<Xml/>");

            controller.When(x => x.GetBetListResult(metaModel, UspKey)).DoNotCallBase();

            // Action
            controller.Index(metaModel);

            // Assert
            controller.Received().GetBetListResult(metaModel, UspKey);
        }
    }
}