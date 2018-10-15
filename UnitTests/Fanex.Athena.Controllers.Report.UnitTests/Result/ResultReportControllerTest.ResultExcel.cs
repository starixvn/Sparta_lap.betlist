namespace Fanex.Athena.Controllers.TotalBets.UnitTests
{
    using System.Collections.Generic;
    using System.IO;
    using System.Net;
    using System.Text;
    using Base.UnitTests;
    using Model.Report.MetaModel;
    using NSubstitute;
    using NUnit.Framework;
    using Resources;
    using Result.Core.Sportsbook.DTO;

    [TestFixture]
    public partial class ResultReportControllerTest : BaseControllerHelperTest
    {
        [Test]
        public void ResultExcel_InvalidModelState_ReturnBadRequestResponse()
        {
            using (var controller = CreateController())
            {
                // Arrange
                controller.ModelState.AddModelError("error", "hihi");
                var response = controller.HttpContext.Response;

                // Action
                controller.ResultExcel(new ResultMetalModel());

                // Assert
                Assert.Multiple(() =>
                {
                    Assert.That(response.StatusCode, Is.EqualTo((int)HttpStatusCode.BadRequest));
                    Assert.That(response.StatusDescription, Is.EqualTo(ViewResources.InvalidParams));
                });
            }
        }

        [Test]
        public void ResultExcel_ValidModelState_ReturnCorrectResponse()
        {
            // Arrange
            const string ExpectedStreamData = "Hello everyone";
            using (var controller = CreateController())
            using (var excelStream = new MemoryStream(Encoding.UTF8.GetBytes(ExpectedStreamData)))
            using (var outputStream = new MemoryStream())
            {
                var response = controller.HttpContext.Response;
                response.OutputStream.Returns(outputStream);
                _resultService.ExcelParser(new List<Sportsbook>(), null).ReturnsForAnyArgs(excelStream);

                // Action
                controller.ResultExcel(new ResultMetalModel());

                // Assert
                Assert.Multiple(() =>
                {
                    response.Received(1).AddHeader("Content-disposition", "attachment; filename=LRF-Result.xls");
                    Assert.That(response.ContentType, Is.EqualTo("application/vnd.ms-excel"));
                    Assert.That(Encoding.UTF8.GetString(((MemoryStream)response.OutputStream).ToArray()), Is.EqualTo(ExpectedStreamData));
                });
            }
        }
    }
}