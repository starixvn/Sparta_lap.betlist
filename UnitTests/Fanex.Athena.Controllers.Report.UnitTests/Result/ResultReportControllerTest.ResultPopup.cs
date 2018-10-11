namespace Fanex.Athena.Controllers.TotalBets.UnitTests
{
    using System.Collections.Generic;
    using System.Web.Mvc;
    using Base.UnitTests;
    using Constant.DataDomain;
    using Model.Report.MetaModel;
    using NSubstitute;
    using NUnit.Framework;
    using Result.Core.Base;
    using Result.Core.NumberGame.DTO;
    using Result.Core.Racing.DTO;
    using Result.Core.Sportsbook.DTO;

    [TestFixture]
    public partial class ResultReportControllerTest : BaseControllerHelperTest
    {
        [Test]
        public void ResultPopup_InvalidModelState_RedirectToErrorPage()
        {
            using (var controller = CreateController())
            {
                // Arrange
                controller.ModelState.AddModelError("error", "hihi");

                // Action
                var result = controller.ResultPopup(new ResultPopupMetalModel()) as RedirectResult;

                // Assert
                Assert.That(result.Url, Is.EqualTo("~/Utility/Error?err=400"));
            }
        }

        [Test]
        public void ResultPopup_ValidModelState_ResultIsCorrect()
        {
            using (var controller = CreateController())
            {
                // Arrange
                _resultService.HtmlParser(Arg.Any<IList<Sportsbook>>(), null).ReturnsForAnyArgs(HtmlResult);

                // Action
                var viewResult = controller.ResultPopup(new ResultPopupMetalModel()) as ViewResult;

                // Assert
                Assert.Multiple(() =>
                {
                    Assert.That(viewResult.ViewName, Is.EqualTo("~/Views/Report/Result/Popup/Main.cshtml"));
                    Assert.That(viewResult.ViewBag.Data, Is.EqualTo(HtmlResult));
                });
            }
        }

        [TestCase(1, true, (int)BetType.VirtualSports2MixParlay, true)]
        [TestCase(0, false, 123, false)]
        [TestCase(null, false, BetType.MixParlay, true)]
        public void ResultPopup_ValidModelState_CreateCorrectResultSetting(
            int inputOutRight,
            bool expectedIsOutRight,
            int bettype,
            bool expectedIsParlayPopup)
        {
            using (var controller = CreateController())
            {
                // Arrange
                var metaModel = new ResultPopupMetalModel
                {
                    SportType = (int)Result.Core.Base.SportType.AGCasino,
                    IsOutRight = inputOutRight,
                    Bettype = bettype
                };

                // Action
                var viewResult = controller.ResultPopup(new ResultPopupMetalModel()) as ViewResult;

                // Assert
                _resultService.HtmlParser(
                        Arg.Any<IList<Sportsbook>>(),
                        Arg.Is<ResultSetting>(x =>
                            x.SportType == (Result.Core.Base.SportType)metaModel.SportType &&
                            x.IsOutright == expectedIsOutRight &&
                            x.IsParlayPopUp == expectedIsParlayPopup &&
                            x.IsPopUp &&
                            x.LanguageCode == SessionService.UserSession.Language &&
                            x.EnableCheckScoreCalculated));
            }
        }

        [TestCase(1, HtmlResult)]
        [TestCase(0, HtmlResult)]
        [TestCase(null, null)]
        public void ResultPopup_IsParlayPopup_ResultIsCorrect(long? refNo, string expectedHtmlResult)
        {
            using (var controller = CreateController())
            {
                // Arrange
                var metaModel = new ResultPopupMetalModel
                {
                    Bettype = (int)BetType.VirtualSports2MixParlay,
                    RefNo = refNo
                };
                var dataList = new List<Parlay> { new Parlay() };
                _resultService.GetPopUpParlayMatch(metaModel.RefNo.HasValue ? metaModel.RefNo.Value : 0).Returns(dataList);
                _resultService.HtmlParser(dataList, Arg.Any<ResultSetting>()).Returns(HtmlResult);

                // Action
                var viewResult = controller.ResultPopup(metaModel) as ViewResult;

                // Assert
                Assert.That(viewResult.ViewBag.Data, Is.EqualTo(expectedHtmlResult));
            }
        }

        [TestCase(Result.Core.Base.SportType.VirtualHorseRacing)]
        [TestCase(Result.Core.Base.SportType.VirtualMotorRacing)]
        [TestCase(Result.Core.Base.SportType.VirtualGreyhounds)]
        [TestCase(Result.Core.Base.SportType.VirtualSpeedway)]
        [TestCase(Result.Core.Base.SportType.VirtualCycling)]
        public void ResultPopup_VirtualSport_ResultIsCorrect(Result.Core.Base.SportType sportType)
        {
            using (var controller = CreateController())
            {
                // Arrange
                var metaModel = CreateResultPopupMetaModel(sportType);
                var dataList = new List<Racing> { new Racing() };
                _resultService.GetPopUpVirtualSportMatch(metaModel).Returns(dataList);
                _resultService.HtmlParser(dataList, Arg.Any<ResultSetting>()).Returns(HtmlResult);

                // Action
                var viewResult = controller.ResultPopup(metaModel) as ViewResult;

                // Assert
                Assert.That(viewResult.ViewBag.Data, Is.EqualTo(HtmlResult));
            }
        }

        [Test]
        public void ResultPopup_NumberGameSport_ResultIsCorrect()
        {
            using (var controller = CreateController())
            {
                // Arrange
                var metaModel = CreateResultPopupMetaModel(Result.Core.Base.SportType.NumberGame);
                var dataList = new List<NumberGame> { new NumberGame() };
                _resultService.GetPopUpNumberGameMatchs(metaModel.MatchId).Returns(dataList);
                _resultService.HtmlParser(dataList, Arg.Any<ResultSetting>()).Returns(HtmlResult);

                // Action
                var viewResult = controller.ResultPopup(metaModel) as ViewResult;

                // Assert
                Assert.That(viewResult.ViewBag.Data, Is.EqualTo(HtmlResult));
            }
        }

        [Test]
        public void ResultPopup_SportBookSports_ResultIsCorrect()
        {
            using (var controller = CreateController())
            {
                // Arrange
                var metaModel = CreateResultPopupMetaModel(Result.Core.Base.SportType.Soccer);
                var dataList = new List<Sportsbook> { new Sportsbook() };
                _resultService.GetPopUpMatch(metaModel).Returns(dataList);
                _resultService.HtmlParser(dataList, Arg.Any<ResultSetting>()).Returns(HtmlResult);

                // Action
                var viewResult = controller.ResultPopup(metaModel) as ViewResult;

                // Assert
                Assert.That(viewResult.ViewBag.Data, Is.EqualTo(HtmlResult));
            }
        }

        private static ResultPopupMetalModel CreateResultPopupMetaModel(Result.Core.Base.SportType sportType)
        {
            return new ResultPopupMetalModel
            {
                MatchId = 1,
                RaceNo = 2,
                Bettype = 3,
                BetId = 4,
                SportType = (int)sportType
            };
        }
    }
}