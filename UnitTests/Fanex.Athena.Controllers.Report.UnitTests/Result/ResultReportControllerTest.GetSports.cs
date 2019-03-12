namespace Fanex.Athena.Controllers.TotalBets.UnitTests
{
    using System.Web.Mvc;
    using System.Web.Script.Serialization;
    using Base.UnitTests;
    using Model.Report.MetaModel.Result;
    using NSubstitute;
    using NUnit.Framework;

    [TestFixture]
    public partial class ResultReportControllerTest : BaseControllerHelperTest
    {
        [Test]
        public void GetSports_InvalidModelState_ReturnEmptyResult()
        {
            using (var controller = CreateController())
            {
                // Arrange
                controller.ModelState.AddModelError("error", "hihi");

                // Action
                var result = controller.GetSports(new GetSportResultMetaModel()).Result as JsonResult;

                // Assert
                Assert.That(result.Data, Is.EqualTo(string.Empty));
            }
        }

        [Test]
        public void GetSports_ValidModel_ReturnOutrightResult()
        {
            using (var controller = CreateController())
            {
                // Arrange
                const string ExpectResult = "sssssssssssss";
                const string Url = "/ResultReport/GetSports";
                var metaModel = new GetSportResultMetaModel();
                _betListApiClient.PostAsyncString(Url, metaModel).Returns(ExpectResult);

                // Action
                var result = controller.GetSports(metaModel).Result as ContentResult;

                // Assert
                Assert.That(result.Content, Is.EqualTo(ExpectResult));
            }
        }
    }
}