namespace Fanex.Athena.Controllers.TotalBets.UnitTests
{
    using System.Collections.Generic;
    using System.Web.Mvc;
    using Base.UnitTests;
    using Model.Casino.DTO.RNGResultDetail;
    using Model.Casino.MetaModel;
    using Model.Casino.ViewModel;
    using NSubstitute;
    using NUnit.Framework;

    [TestFixture]
    public partial class HandHistoryControllerTest : BaseControllerHelperTest
    {
        [Test]
        public void RNGCasinoDetail_EmptyGroup_ReturnNoGameErrorView()
        {
            using (var controller = CreateController())
            {
                // Arrange
                var metaModel = new RNGCasinoResultDetailMetaModel
                {
                    CurrencyName = "uus"
                };

                // Action
                var result = controller.RNGCasinoDetail(metaModel) as ViewResult;

                // Assert
                Assert.Multiple(() =>
                {
                    Assert.That(metaModel.CurrencyName, Is.EqualTo("UUS"));
                    Assert.That(result.ViewName, Is.EqualTo("~/Views/RNGCasino/RNGCasinoDetail/NoGameErr.cshtml"));
                });
            }
        }

        [Test]
        public void RNGCasinoDetail_InvalidModel_ReturnNoInfoView()
        {
            using (var controller = CreateController())
            {
                // Arrange
                var metaModel = new RNGCasinoResultDetailMetaModel
                {
                    GameTypeId = (int)CasinoGameType.Baccarat,
                    CurrencyName = string.Empty
                };

                // Action
                var result = controller.RNGCasinoDetail(metaModel) as ViewResult;

                // Assert
                Assert.Multiple(() =>
                {
                    _rngCasinoService.DidNotReceiveWithAnyArgs().GetRNGCasinoResultDetail(metaModel);
                    Assert.That(result.ViewName, Is.EqualTo("~/Views/RNGCasino/RNGCasinoDetail/Noinfo.cshtml"));
                });
            }
        }

        [TestCase(CasinoGameType.Baccarat, "1")]
        [TestCase(CasinoGameType.TexasHoldEm, "1-1")]
        [TestCase(CasinoGameType.RedDog, "2")]
        [TestCase(CasinoGameType.MoneySlot, "3")]
        [TestCase(CasinoGameType.Champions, "4")]
        [TestCase(CasinoGameType.WesternZodiac, "4-1")]
        [TestCase(CasinoGameType.MoneyWheel, "5")]
        [TestCase(CasinoGameType.JourneytotheWest, "6")]
        [TestCase(CasinoGameType.ChineseZodiac, "6-1")]
        [TestCase(CasinoGameType.JacksorBetter, "7-0")]
        [TestCase(CasinoGameType.JungleWild, "7-1")]
        [TestCase(CasinoGameType.No4LineJacksorBetter, "7-2")]
        [TestCase(CasinoGameType.No10SOrBetter, "7-3")]
        [TestCase(CasinoGameType.MonkeyHilo, "7-4")]
        [TestCase(CasinoGameType.KenoChinese, "7-5")]
        [TestCase(CasinoGameType.TurfKing, "7-6")]
        [TestCase(CasinoGameType.MamakCorner, "7-8")]
        [TestCase(CasinoGameType.MayaPush, "8")]
        [TestCase(CasinoGameType.PenaltyHatTrick, "9")]
        public void RNGCasinoDetail_ValidModel_ReturnCorrectView(CasinoGameType gameType, string group)
        {
            using (var controller = CreateController())
            {
                // Arrange
                var metaModel = new RNGCasinoResultDetailMetaModel
                {
                    UserBetId = 1,
                    GameTypeId = (int)gameType,
                    CurrencyName = "UUS"
                };
                var data = new CasinoHandHistoryDetailViewModel
                {
                    CasinoDetailHeaderList = new List<CasinoWinLossDetailHeader>
                    {
                        new CasinoWinLossDetailHeader()
                    }
                };
                _rngCasinoService.GetRNGCasinoResultDetail(metaModel).Returns(data);

                // Action
                var result = controller.RNGCasinoDetail(metaModel) as ViewResult;
                var viewModel = result.Model as CasinoHandHistoryDetailViewModel;

                // Assert
                Assert.Multiple(() =>
                {
                    Assert.That(result.ViewName, Is.EqualTo($"~/Views/RNGCasino/RNGCasinoDetail/Group{group}.cshtml"));
                    Assert.That(viewModel, Is.EqualTo(data));
                });
            }
        }
    }
}