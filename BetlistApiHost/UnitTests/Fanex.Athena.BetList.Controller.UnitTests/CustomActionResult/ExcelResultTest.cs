namespace Fanex.Athena.BetList.Controller.UnitTests.CustomActionResult
{
    using System.Collections.Specialized;
    using System.IO;
    using System.Web;
    using System.Web.Mvc;
    using NPOI.SS.UserModel;
    using NSubstitute;
    using NUnit.Framework;
    using UnitTest.Common.Helpers;

    public class ExcelResultTest
    {
        [Test]
        public void ExcelResult_WhenCall_WriteWorkBookToOutputStream()
        {
            using (var controller = new FakeCustomerActionResultController(MvcHelper.CreateControllerContext()))
            {
                // Arrange
                var hssfWorkbook = Substitute.For<IWorkbook>();

                // Action
                controller.TestExcelResult(hssfWorkbook).ExecuteResult(controller.ControllerContext);

                // Assert
                hssfWorkbook.Received(1).Write(controller.ControllerContext.HttpContext.Response.OutputStream);
            }
        }
    }
}