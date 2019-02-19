namespace Fanex.Athena.Controllers.TotalBets.UnitTests
{
    using System.Collections.Generic;
    using System.Web.Mvc;
    using Base.UnitTests;
    using Model.Report.MetaModel.Result;
    using NSubstitute;
    using NUnit.Framework;

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
                var viewResult = controller.Index(new ResultMetaModel()) as ContentResult;

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
                var viewResult = controller.Index(new ResultMetaModel()) as ContentResult;

                // Assert
                Assert.That(viewResult.Content, Is.Empty);
            }
        }
    }
}