namespace Fanex.Athena.Controllers.TotalBets.UnitTests
{
    using System.Web.Mvc;
    using Base.UnitTests;
    using Model.Report.MetaModel;
    using NSubstitute;
    using NUnit.Framework;

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
                var result = controller.ResultPopup(new ResultPopupMetalModel()).Result as RedirectResult;

                // Assert
                Assert.That(result.Url, Is.EqualTo("~/Utility/Error?err=400"));
            }
        }

        [Test]
        public void ResultPopup_WhenCall_ReturnCorrectResult()
        {
            using (var controller = CreateController())
            {
                // Arrange
                const string ExpectedData = "Abc";
                const string Url = "/ResultReport/ResultPopup";
                _betListApiClient.PostAsyncString(Url, Arg.Any<object>(), Arg.Any<string>()).Returns(ExpectedData);
                var metaModel = new ResultPopupMetalModel();

                // Action
                var result = controller.ResultPopup(metaModel).Result as ViewResult;

                // Assert
                Assert.Multiple(() =>
                {
                    _betListApiClient.Received(1).PostAsyncString(Url, metaModel, Arg.Any<string>());

                    Assert.That(result.ViewBag.Data, Is.EqualTo(ExpectedData));
                    Assert.That(result.ViewName, Is.EqualTo("~/Views/Desktop/Report/Result/Popup/Main.cshtml"));
                });
            }
        }
    }
}