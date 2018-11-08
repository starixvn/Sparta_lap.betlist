namespace Fanex.Athena.Controllers.TotalBets.UnitTests
{
    using System.Collections.Generic;
    using System.Web.Mvc;
    using Base.UnitTests;
    using Model.Report.MetaModel;
    using Model.Report.ViewModel;
    using NSubstitute;
    using NUnit.Framework;
    using Result.Core.Base;
    using Result.Core.NumberGame.DTO;
    using Result.Core.Racing.DTO;
    using Result.Core.Sportsbook.DTO;

    [TestFixture]
    public partial class ResultReportControllerTest : BaseControllerHelperTest
    {
        [TestCase("CS", true, false, false)]
        [TestCase("KN", false, true, false)]
        [TestCase("LT", false, false, true)]
        public void Index_WhenGet_ReturnCorrectViewResult(
            string productCodeList,
            bool isAllowRng,
            bool isAllowKeno,
            bool isAllowLottery)
        {
            using (var controller = CreateController(productCodeList))
            {
                // Action
                var viewResult = controller.Index() as ViewResult;
                var viewModel = viewResult.Model as ResultReportViewModel;

                // Assert
                Assert.Multiple(() =>
                {
                    Assert.That(viewResult.ViewName, Is.EqualTo("~/Views/Desktop/Report/Result/Main.cshtml"));
                    Assert.That(viewResult.ViewBag.IsAllowedRNG, Is.EqualTo(isAllowRng));
                    Assert.That(viewResult.ViewBag.IsAllowedKeno, Is.EqualTo(isAllowKeno));
                    Assert.That(viewResult.ViewBag.IsAllowedLottery, Is.EqualTo(isAllowLottery));
                    Assert.That(viewModel.Param.BalanceUpDay, Is.EqualTo(BalanceUpDate));
                });
            }
        }

        [Test]
        public void IndexPost_ValidModelState_ResultContentIsCorrect()
        {
            using (var controller = CreateController())
            {
                // Arrange
                _resultService.HtmlParser(Arg.Any<IList<Sportsbook>>(), null).ReturnsForAnyArgs(HtmlResult);

                // Action
                var viewResult = controller.Index(new ResultMetalModel()) as ContentResult;

                // Assert
                Assert.That(viewResult.Content, Is.EqualTo(HtmlResult));
            }
        }

        [Test]
        public void IndexPost_InvalidModelState_ResultContentIsEmpty()
        {
            using (var controller = CreateController())
            {
                // Arrange
                controller.ModelState.AddModelError("error", "hihi");

                // Action
                var viewResult = controller.Index(new ResultMetalModel()) as ContentResult;

                // Assert
                Assert.That(viewResult.Content, Is.Empty);
            }
        }

        [Test]
        public void IndexPost_IsOutRight_ResultContentIsCorrect()
        {
            using (var controller = CreateController())
            {
                // Arrange
                const int SportId = 1;
                var metaModel = new ResultMetalModel
                {
                    SportId = SportId,
                    IsOutRight = true
                };
                var dataList = new List<Outright>
                {
                    new Outright
                    {
                        SportType = SportId
                    }
                };
                _resultService.GetOutRight(metaModel).Returns(dataList);
                _resultService.HtmlParser(Arg.Any<IList<Outright>>(), Arg.Any<ResultSetting>()).Returns(HtmlResult);

                // Action
                var viewResult = controller.Index(metaModel) as ContentResult;

                // Assert
                Assert.That(viewResult.Content, Is.EqualTo(HtmlResult));
            }
        }

        [TestCase((int)SportType.VirtualHorseRacing)]
        [TestCase((int)SportType.VirtualGreyhounds)]
        [TestCase((int)SportType.VirtualMotorRacing)]
        [TestCase((int)SportType.VirtualCycling)]
        [TestCase((int)SportType.VirtualSpeedway)]
        public void IndexPost_VirtualSport_ResultContentIsCorrect(int sportId)
        {
            using (var controller = CreateController())
            {
                // Arrange
                var metaModel = new ResultMetalModel
                {
                    SportId = sportId,
                    IsOutRight = false
                };
                var dataList = new List<Racing> { new Racing() };
                _resultService.GetVirtualSportMatches(metaModel).Returns(dataList);
                _resultService.HtmlParser(Arg.Any<IList<Racing>>(), Arg.Any<ResultSetting>()).Returns(HtmlResult);

                // Action
                var viewResult = controller.Index(metaModel) as ContentResult;

                // Assert
                Assert.That(viewResult.Content, Is.EqualTo(HtmlResult));
            }
        }

        [Test]
        public void IndexPost_NumberGame_ResultContentIsCorrect()
        {
            using (var controller = CreateController())
            {
                // Arrange
                var metaModel = new ResultMetalModel
                {
                    SportId = (int)SportType.NumberGame,
                    IsOutRight = false
                };
                var dataList = new List<NumberGame> { new NumberGame() };
                _resultService.GetNumberGameMatches(metaModel).Returns(dataList);
                _resultService.HtmlParser(Arg.Any<IList<NumberGame>>(), Arg.Any<ResultSetting>()).Returns(HtmlResult);

                // Action
                var viewResult = controller.Index(metaModel) as ContentResult;

                // Assert
                Assert.That(viewResult.Content, Is.EqualTo(HtmlResult));
            }
        }
    }
}