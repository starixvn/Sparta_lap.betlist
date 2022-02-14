namespace Fanex.Athena.Result.Controller.UnitTests
{
    using Fanex.Athena.Result.Controller.MetaModel;
    using Fanex.Athena.Result.Controller.ResultPopup;
    using Fanex.Result.Core.SportsLottery;
    using Fanex.Result.Core.ThirdParty;
    using Fanex.Result.Core.ThirdParty.Models;
    using NSubstitute;
    using NUnit.Framework;
    using UnitTest.Common.Helpers;

    [TestFixture]
    public class SportsLotteryResultPopupControllerTest
    {
        [Test]
        public void Index_WhenCall_ReturnResultFromApi()
        {
            // Arrange
            var metaModel = FakeHelper.Create<ThirdPartyMetaModel>();
            var resultApi = Substitute.For<ISportsLotteryResult>();
            var result3rdApi = Substitute.For<IThirdPartyResult>();
            var request = new ResultRequest(metaModel.CustId, metaModel.TransId3rd, metaModel.GameId3rd, metaModel.CurrencyId, metaModel.SiteId, metaModel.LanguageCode)
            {
                BetTypeId = metaModel.BetTypeId
            };

            using (var controller = new SportsLotteryResultPopupController(resultApi, result3rdApi))
            {
                // Action
                var result = controller.Index(metaModel);

                // Assert
                result3rdApi.Received().GetResultGET(Arg.Is<ResultRequest>(rq => rq.CustId == request.CustId
                    && rq.TranId3rd == request.TranId3rd
                    && rq.GameId3rd == request.GameId3rd
                    && rq.SiteId == request.SiteId));
            }
        }

        [Test]
        public void ResultPage_WhenCall_ReturnResultPageFromApi()
        {
            // Arrange
            var platform = "d";
            var languageCode = FakeHelper.Create<string>();
            var isDesktop = true;

            var resultApi = Substitute.For<ISportsLotteryResult>();

            using (var controller = new SportsLotteryResultPopupController(resultApi))
            {
                // Action
                var result = controller.ResultPage(platform, languageCode);

                // Assert
                resultApi.Received().GetResultPage(isDesktop, languageCode);
            }
        }
    }
}