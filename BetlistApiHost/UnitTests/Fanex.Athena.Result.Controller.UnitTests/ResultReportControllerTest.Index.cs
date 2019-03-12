namespace Fanex.Athena.Result.Controller.UnitTests
{
    using System.Collections.Generic;
    using System.Web.Mvc;
    using Fanex.Result.Core.Base;
    using Fanex.Result.Core.NumberGame.DTO;
    using Fanex.Result.Core.Racing.DTO;
    using Fanex.Result.Core.Sportsbook.DTO;
    using Model.MetaModel;
    using NSubstitute;
    using NUnit.Framework;

    [TestFixture]
    public partial class ResultReportControllerTest
    {
        [Test]
        public void Index_ValidModelState_ResultContentIsCorrect()
        {
            using (var controller = CreateController())
            {
                // Arrange
                _resultService.HtmlParser(Arg.Any<IList<Sportsbook>>(), null).ReturnsForAnyArgs(HtmlResult);

                // Action
                var result = controller.Index(new ResultMetaModel());

                // Assert
                Assert.That(result, Is.EqualTo(HtmlResult));
            }
        }

        [Test]
        public void Index_IsOutRight_ResultContentIsCorrect()
        {
            using (var controller = CreateController())
            {
                // Arrange
                const int SportId = 1;
                var metaModel = new ResultMetaModel
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
                var result = controller.Index(metaModel);

                // Assert
                Assert.That(result, Is.EqualTo(HtmlResult));
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
                var metaModel = new ResultMetaModel
                {
                    SportId = sportId,
                    IsOutRight = false
                };
                var dataList = new List<Racing> { new Racing() };
                _resultService.GetVirtualSportMatches(metaModel).Returns(dataList);
                _resultService.HtmlParser(Arg.Any<IList<Racing>>(), Arg.Any<ResultSetting>()).Returns(HtmlResult);

                // Action
                var result = controller.Index(metaModel);

                // Assert
                Assert.That(result, Is.EqualTo(HtmlResult));
            }
        }

        [Test]
        public void IndexPost_NumberGame_ResultContentIsCorrect()
        {
            using (var controller = CreateController())
            {
                // Arrange
                var metaModel = new ResultMetaModel
                {
                    SportId = (int)SportType.NumberGame,
                    IsOutRight = false
                };
                var dataList = new List<NumberGame> { new NumberGame() };
                _resultService.GetNumberGameMatches(metaModel).Returns(dataList);
                _resultService.HtmlParser(Arg.Any<IList<NumberGame>>(), Arg.Any<ResultSetting>()).Returns(HtmlResult);

                // Action
                var result = controller.Index(metaModel);

                // Assert
                Assert.That(result, Is.EqualTo(HtmlResult));
            }
        }
    }
}