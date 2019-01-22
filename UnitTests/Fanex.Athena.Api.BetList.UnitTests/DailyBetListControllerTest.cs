namespace Fanex.Athena.Api.BetList.UnitTests
{
    using System;
    using Model.BetList.MetaModel;
    using NSubstitute;
    using NUnit.Framework;

    public class DailyBetListControllerTest : ApiBetListBaseControllerHelperTest
    {
        private const string UspKey = "Lrf_BetList_SelWinlossDaily";

        [TestCase(null, "")]
        [TestCase("test", "test")]
        public void Index_WhenCall_GetBetListResultWithCorrectParameter(string baseCurrency, string expectedBaseCurrency)
        {
            // Arrange
            var controller = Substitute.ForPartsOf<DailyBetListController>(
                SessionService,
                ProductService,
                LangService,
                BridgeClientService,
                BalanceUpDataAccess);

            const int Page = 1;
            const int Rows = 50;
            var date = new DateTime(2018, 1, 1);

            SessionService.GenerateAgentListXml(0).Returns("<Xml/>");

            controller.When(x => x.GetPagingBetListResult(Arg.Any<DailyBetListMetaModel>(), UspKey, Page, Rows)).DoNotCallBase();

            // Action
            controller.Index(Page, Rows, 0, string.Empty, baseCurrency, date, string.Empty, string.Empty);

            // Assert
            controller.Received().GetPagingBetListResult(
                Arg.Is<DailyBetListMetaModel>(x => x.BaseCurrency == expectedBaseCurrency && x.Date == date.Date && x.FromDate == date.Date && x.ToDate == date.Date),
                UspKey,
                Page,
                Rows);
        }
    }
}