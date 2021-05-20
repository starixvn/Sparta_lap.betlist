namespace Fanex.Athena.Result.Controller.UnitTests
{
    using Fanex.Result.Core.KenoLotteryTG;
    using Model.Const;
    using Model.MetaModel.Keno;
    using NSubstitute;
    using NUnit.Framework;
    using Service.Interfaces;
    using System;
    using System.Globalization;
    using UnitTest.Common.Helpers;

    [TestFixture]
    public class ResultKenoReportControllerTest
    {
        private const int VendorProductId = 34343;
        private const string UserId = "987343";
        private const string LanguageCode = "en-US";

        IKenoLotteryTGResultFactory _kenoApi;
        IKenoLotteryTGResult _resultApi;

        [SetUp]
        public void SetUp()
        {
            _kenoApi = Substitute.For<IKenoLotteryTGResultFactory>();
            _resultApi = Substitute.For<IKenoLotteryTGResult>();
        }

        [TearDown]
        public void TearDown()
        {
            _kenoApi.ClearReceivedCalls();
            _resultApi.ClearReceivedCalls();
        }

        [Test]
        public void GetResultPage_WhenCall_ReturnCorrectResult()
        {
            // Arrange
            const string ExpectedUrl = "http://abc";
            _resultApi.GetResult(Arg.Any<string>(), Arg.Any<bool>(), Arg.Any<DateTime>(), Arg.Any<string>()).Returns(ExpectedUrl);
            _kenoApi.CreateResultApi(Arg.Any<int>()).Returns(_resultApi);

            using (var controller = CreateController())
            {
                // Action
                var result = controller.GetResultPage(VendorProductId, UserId, LanguageCode);

                // Assert
                Assert.That(result, Is.EqualTo(ExpectedUrl));
            }
        }

        private ResultKenoReportController CreateController()
        {
            var controller = new ResultKenoReportController(_kenoApi);
            controller.ControllerContext = MvcHelper.CreateControllerContext();
            controller.Url = MvcHelper.CreateUrlHelper(controller.ControllerContext.HttpContext);
            return controller;
        }
    }
}