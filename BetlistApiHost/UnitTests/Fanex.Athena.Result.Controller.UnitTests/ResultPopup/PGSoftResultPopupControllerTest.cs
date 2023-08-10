using Fanex.Athena.BetList.Service.Interfaces;
using Fanex.Athena.Result.Controller.MetaModel;
using Fanex.Athena.Result.Controller.ResultPopup;
using Fanex.Athena.UnitTest.Common.Helpers;
using Fanex.Result.Core.ThirdParty;
using Fanex.Result.Core.ThirdParty.Models;
using NSubstitute;
using NUnit.Framework;

namespace Fanex.Athena.Result.Controller.UnitTests.ResultPopup
{
    [TestFixture]
    public class PGSoftResultPopupControllerTest
    {
        [Test]
        public void Index_WhenCall_ReturnResultFromApi()
        {
            // Arrange
            var metaModel = FakeHelper.Create<ThirdPartyMetaModel>();
            var result3RdApi = Substitute.For<IThirdPartyResult>();
            var infoService = Substitute.For<IIpInfoService>();

            var request = new ResultRequest(metaModel.CustId, metaModel.TransId3rd, metaModel.GameId3rd,
                                            metaModel.CurrencyId, metaModel.SiteId, metaModel.LanguageCode)
            {
                BetTypeId = metaModel.BetTypeId
            };

            using (var controller = new PGSoftResultPopupController(infoService, result3RdApi))
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
    }
}
