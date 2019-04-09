namespace Fanex.Athena.BetList.Controller.UnitTests.CustomActionResult
{
    using System.Text;
    using System.Web.Mvc;
    using Controller.CustomActionResult;
    using NPOI.SS.UserModel;

    internal class FakeCustomerActionResultController : Controller
    {
        public FakeCustomerActionResultController(ControllerContext context)
        {
            this.ControllerContext = context;
        }

        internal ActionResult TestExcelResult(IWorkbook hssfWorkbook)
        {
            return new ExcelResult(hssfWorkbook);
        }

        internal ActionResult TestJsonNetResult(object data, Encoding contentEncoding = null)
        {
            return new JsonNetResult
            {
                Data = data,
                ContentEncoding = contentEncoding
            };
        }
    }
}