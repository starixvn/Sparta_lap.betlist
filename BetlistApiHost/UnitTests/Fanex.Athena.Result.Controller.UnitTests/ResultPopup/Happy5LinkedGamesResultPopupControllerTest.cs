using Fanex.Athena.Result.Controller.MetaModel;
using Fanex.Athena.Result.Controller.ResultPopup;
using Fanex.Athena.UnitTest.Common.Helpers;
using Fanex.Result.Core.LinkedGames;
using Fanex.Result.Core.ThirdParty;
using Fanex.Result.Core.ThirdParty.Models;
using NSubstitute;
using NUnit.Framework;

namespace Fanex.Athena.Result.Controller.UnitTests.ResultPopup
{
    [TestFixture]
    public class Happy5LinkedGamesResultPopupControllerTest
    {
        [Test]
        public void Index_WhenCall_ReturnResultFromApi()
        {
            // Arrange
            var metaModel = FakeHelper.Create<ThirdPartyMetaModel>();
            var result3RdApi = Substitute.For<IThirdPartyResult>();
            var resultLinkedGameApi = Substitute.For<ILinkedGamesResult>();

            var request = new ResultRequest(metaModel.CustId, metaModel.TransId3rd, metaModel.GameId3rd,
                                            metaModel.CurrencyId, metaModel.SiteId, metaModel.LanguageCode)
            {
                BetTypeId = metaModel.BetTypeId
            };

            using (var controller = new Happy5LinkedGamesResultPopupController(resultLinkedGameApi, result3RdApi))
            {
                // Action
                controller.Index(metaModel);

                // Assert
                result3RdApi.Received().GetResultGET(Arg.Is<ResultRequest>(rq => rq.CustId == request.CustId
                                                                            && rq.TranId3rd == request.TranId3rd
                                                                            && rq.GameId3rd == request.GameId3rd
                                                                            && rq.SiteId == request.SiteId));
            }
        }

        private const string DesktopPlatform = "d";

        [Test]
        public void Soccer5_WhenCallWithDesktop_ReturnResultFromApi()
        {
            // Arrange
            var matchId = FakeHelper.Create<string>();
            var balls = FakeHelper.Create<string>();
            var platform = DesktopPlatform;
            var languageCode = FakeHelper.Create<string>();
            var result3RdApi = Substitute.For<IThirdPartyResult>();
            var resultLinkedGameApi = Substitute.For<ILinkedGamesResult>();

            using (var controller = new Happy5LinkedGamesResultPopupController(resultLinkedGameApi, result3RdApi))
            {
                // Action
                controller.Soccer5(matchId, balls, platform, languageCode);

                // Assert
                resultLinkedGameApi.Received().GetSoccer5Result(matchId, balls, true, language: languageCode);
            }
        }

        [Test]
        public void Soccer5_WhenCallWithoutDesktop_ReturnResultFromApi()
        {
            // Arrange
            var matchId = FakeHelper.Create<string>();
            var balls = FakeHelper.Create<string>();
            var platform = FakeHelper.Create<string>();
            var languageCode = FakeHelper.Create<string>();
            var result3RdApi = Substitute.For<IThirdPartyResult>();
            var resultLinkedGameApi = Substitute.For<ILinkedGamesResult>();

            using (var controller = new Happy5LinkedGamesResultPopupController(resultLinkedGameApi, result3RdApi))
            {
                // Action
                controller.Soccer5(matchId, balls, platform, languageCode);

                // Assert
                resultLinkedGameApi.Received().GetSoccer5Result(matchId, balls, false, language: languageCode);
            }
        }
    }
}
