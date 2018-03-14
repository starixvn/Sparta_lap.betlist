namespace Fanex.Athena.Controllers.Forecast.UnitTests
{
    using System.Collections.Specialized;
    using System.Web.Mvc;
    using Base.UnitTests;
    using Constant.DataDomain;
    using Model.Forecast.DTO.ForecastAH;
    using Model.Forecast.MetaModel;
    using Model.Forecast.ViewModel;
    using NSubstitute;
    using NUnit.Framework;
    using Resources;
    using Service.Forecast.Interfaces;

    public class AhForecastControllerTest : BaseControllerHelperTest
    {
        private IForecastService _forecastService;

        [OneTimeSetUp]
        public override void FixtureSetUp()
        {
            base.FixtureSetUp();

            SetUpDefaultFakeData();
        }

        [Test]
        public void Index_WhenCall_ReturnCorrectViewName()
        {
            using (var controller = CreateController())
            {
                // Arrange
                var metaModel = new ForecastAHMetaModel { MatchId = 2, BetType = 3 };
                _forecastService.GetForecastTopOddsByMatch(metaModel.MatchId, metaModel.BetType).Returns(x => { return null; });

                // Action
                var result = controller.Index(metaModel) as ViewResult;

                // Assert
                Assert.That(result.ViewName, Is.EqualTo("~/Views/Forecast/ForecastAH/Main.cshtml"));
            }
        }

        [Test]
        public void Index_HeaderParamIsNull_ReturnNullForecastHeaderParamAndValue()
        {
            using (var controller = CreateController())
            {
                // Arrange
                var metaModel = new ForecastAHMetaModel { MatchId = 2, BetType = 3 };
                _forecastService.GetForecastTopOddsByMatch(metaModel.MatchId, metaModel.BetType).Returns(x => { return null; });

                // Action
                var result = controller.Index(metaModel) as ViewResult;
                var viewModel = (ForecastAHViewModel)result.Model;

                // Assert
                Assert.Multiple(() =>
                {
                    Assert.That(viewModel.ForecastHeaderParam, Is.Null);
                    Assert.That(viewModel.ForecastValue, Is.Null);
                });
            }
        }

        [TestCase((int)BetType.OverUnder, 1, 2, 3)]
        [TestCase((int)BetType.FirstHalfOverUnder, 1, 2, 3)]
        [TestCase((int)BetType.Handicap, 3, 2, 1)]
        public void Index_CurrentScoreRequestParamIsNull_CalculateCurrenctCoreCorrectly(int betType, int liveHomeScore, int liveAwayScore, int expectedCurrentCore)
        {
            using (var controller = CreateController())
            {
                // Arrange
                var metaModel = new ForecastAHMetaModel { MatchId = 2, BetType = betType };
                _forecastService.GetForecastTopOddsByMatch(metaModel.MatchId, metaModel.BetType)
                    .Returns(new ForecastAH { LiveHomeScore = liveHomeScore, LiveAwayScore = liveAwayScore });

                var queryString = new NameValueCollection();
                queryString.Add("CurrentScore", null);
                controller.HttpContext.Request.QueryString.Returns(queryString);

                _forecastService.GetForecastHandicap(metaModel, DefaultUserSession.UserId).Returns(new ForecastValue());
                _forecastService.GetForecastOverUnder(metaModel, DefaultUserSession.UserId).Returns(new ForecastValue());

                // Action
                var result = controller.Index(metaModel) as ViewResult;
                var viewModel = (ForecastAHViewModel)result.Model;

                // Assert
                Assert.That(viewModel.Param.CurrentScore, Is.EqualTo(expectedCurrentCore));
            }
        }

        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        [TestCase(4)]
        public void Index_CurrentScoreRequestParamIsNotNull_CurrenctCoreIsNotChanged(int expectedCurrentCore)
        {
            using (var controller = CreateController())
            {
                // Arrange
                var metaModel = new ForecastAHMetaModel { MatchId = 2, BetType = 3, CurrentScore = expectedCurrentCore };
                _forecastService.GetForecastTopOddsByMatch(metaModel.MatchId, metaModel.BetType)
                    .Returns(new ForecastAH());

                var queryString = new NameValueCollection();
                queryString.Add("CurrentScore", string.Empty);
                controller.HttpContext.Request.QueryString.Returns(queryString);

                _forecastService.GetForecastOverUnder(metaModel, DefaultUserSession.UserId).Returns(new ForecastValue());

                // Action
                var result = controller.Index(metaModel) as ViewResult;
                var viewModel = (ForecastAHViewModel)result.Model;

                // Assert
                Assert.That(viewModel.Param.CurrentScore, Is.EqualTo(expectedCurrentCore));
            }
        }

        [TestCase((int)BetType.Handicap, nameof(ViewResources.Handicap))]
        [TestCase((int)BetType.FirstHalfHandicap, nameof(ViewResources.lbl1stHandicap))]
        [TestCase((int)BetType.OverUnder, nameof(ViewResources.OverUnder))]
        [TestCase((int)BetType.FirstHalfOverUnder, nameof(ViewResources.lbl1stOver_Under))]
        public void Index_AsignForeCastName_GetCorrectNameByBetType(int betType, string expectedName)
        {
            using (var controller = CreateController())
            {
                // Arrange
                var metaModel = new ForecastAHMetaModel { MatchId = 2, BetType = betType };
                _forecastService.GetForecastTopOddsByMatch(metaModel.MatchId, metaModel.BetType)
                    .Returns(new ForecastAH());

                var queryString = new NameValueCollection();
                queryString.Add("CurrentScore", string.Empty);
                controller.HttpContext.Request.QueryString.Returns(queryString);

                _forecastService.GetForecastHandicap(metaModel, DefaultUserSession.UserId).Returns(new ForecastValue());
                _forecastService.GetForecastOverUnder(metaModel, DefaultUserSession.UserId).Returns(new ForecastValue());

                // Action
                var result = controller.Index(metaModel) as ViewResult;
                var viewModel = (ForecastAHViewModel)result.Model;

                // Assert
                Assert.That(viewModel.ForecastHeaderParam.ForecastName, Is.EqualTo(ViewResources.ResourceManager.GetString(expectedName)));
            }
        }

        [TestCase((int)BetType.Handicap)]
        [TestCase((int)BetType.FirstHalfHandicap)]
        public void Index_HandicapBetType_GetCorrectValue(int betType)
        {
            using (var controller = CreateController())
            {
                // Arrange
                var metaModel = new ForecastAHMetaModel { MatchId = 2, BetType = betType };
                _forecastService.GetForecastTopOddsByMatch(metaModel.MatchId, metaModel.BetType)
                    .Returns(new ForecastAH());

                var queryString = new NameValueCollection();
                queryString.Add("CurrentScore", string.Empty);
                controller.HttpContext.Request.QueryString.Returns(queryString);

                var foreCastValue = new ForecastValue();
                _forecastService.GetForecastHandicap(metaModel, DefaultUserSession.UserId).Returns(foreCastValue);

                // Action
                var result = controller.Index(metaModel) as ViewResult;
                var viewModel = (ForecastAHViewModel)result.Model;

                // Assert
                Assert.That(viewModel.ForecastValue, Is.EqualTo(foreCastValue));
            }
        }

        [TestCase((int)BetType.OverUnder)]
        [TestCase((int)BetType.FirstHalfOverUnder)]
        public void Index_OverUnderBetType_GetCorrectValue(int betType)
        {
            using (var controller = CreateController())
            {
                // Arrange
                var metaModel = new ForecastAHMetaModel { MatchId = 2, BetType = betType };
                _forecastService.GetForecastTopOddsByMatch(metaModel.MatchId, metaModel.BetType)
                    .Returns(new ForecastAH());

                var queryString = new NameValueCollection();
                queryString.Add("CurrentScore", string.Empty);
                controller.HttpContext.Request.QueryString.Returns(queryString);

                var foreCastValue = new ForecastValue();
                _forecastService.GetForecastOverUnder(metaModel, DefaultUserSession.UserId).Returns(foreCastValue);

                // Action
                var result = controller.Index(metaModel) as ViewResult;
                var viewModel = (ForecastAHViewModel)result.Model;

                // Assert
                Assert.That(viewModel.ForecastValue, Is.EqualTo(foreCastValue));
            }
        }

        [TestCase((int)BetType.OverUnder, -1, new[] { "5", "4", "3", "2", "1", "0" })]
        [TestCase((int)BetType.FirstHalfOverUnder, 6, new[] { "11", "10", "9", "8", "7", "6", "5", "4", "3", "2", "1" })]
        [TestCase((int)BetType.Handicap, -6, new[] { "-1", "-2", "-3", "-4", "-5", "-6", "-7", "-8", "-9", "-10", "-11" })]
        [TestCase((int)BetType.Handicap, 1, new[] { "6", "5", "4", "3", "2", "1", "0", "-1", "-2", "-3", "-4" })]
        public void Index_When_RenderCorrectForecastLink(int betType, int currentScore, string[] expectedLink)
        {
            using (var controller = CreateController())
            {
                // Arrange
                var metaModel = new ForecastAHMetaModel { MatchId = 2, BetType = betType, CurrentScore = currentScore };
                _forecastService.GetForecastTopOddsByMatch(metaModel.MatchId, metaModel.BetType)
                    .Returns(new ForecastAH());

                var queryString = new NameValueCollection();
                queryString.Add("CurrentScore", string.Empty);
                controller.HttpContext.Request.QueryString.Returns(queryString);

                var foreCastValue = new ForecastValue();
                _forecastService.GetForecastHandicap(metaModel, DefaultUserSession.UserId).Returns(foreCastValue);
                _forecastService.GetForecastOverUnder(metaModel, DefaultUserSession.UserId).Returns(foreCastValue);

                // Action
                var result = controller.Index(metaModel) as ViewResult;
                var viewModel = (ForecastAHViewModel)result.Model;

                // Assert
                Assert.That(viewModel.ForecastValue.ForeCastLink, Is.EqualTo(expectedLink));
            }
        }

        [TestCase((int)BetType.OverUnder, -1, new[] { 0, 1, 2, 3, 4 })]
        [TestCase((int)BetType.FirstHalfOverUnder, 3, new[] { 3, 4, 5, 6, 7 })]
        [TestCase((int)BetType.Handicap, -1, new[] { -3, -2, -1, 0, 1 })]
        [TestCase((int)BetType.FirstHalfHandicap, 0, new[] { -2, -1, 0, 1, 2 })]
        public void Index_When_RenderCorrectForecastHeader(int betType, int currentScore, int[] expectedHeader)
        {
            using (var controller = CreateController())
            {
                // Arrange
                var metaModel = new ForecastAHMetaModel { MatchId = 2, BetType = betType, CurrentScore = currentScore };
                _forecastService.GetForecastTopOddsByMatch(metaModel.MatchId, metaModel.BetType)
                    .Returns(new ForecastAH());

                var queryString = new NameValueCollection();
                queryString.Add("CurrentScore", string.Empty);
                controller.HttpContext.Request.QueryString.Returns(queryString);

                var foreCastValue = new ForecastValue();
                _forecastService.GetForecastHandicap(metaModel, DefaultUserSession.UserId).Returns(foreCastValue);
                _forecastService.GetForecastOverUnder(metaModel, DefaultUserSession.UserId).Returns(foreCastValue);

                // Action
                var result = controller.Index(metaModel) as ViewResult;
                var viewModel = (ForecastAHViewModel)result.Model;

                // Assert
                Assert.That(viewModel.ForecastValue.ForecastHeader, Is.EqualTo(expectedHeader));
            }
        }

        private AhForecastController CreateController()
        {
            _forecastService = Substitute.For<IForecastService>();

            var controller = new AhForecastController(
                LangService,
                SessionService,
                AppSettings,
                ClientViewHelper,
                _forecastService)
            {
                ControllerContext = ControllerContext
            };

            return controller;
        }
    }
}