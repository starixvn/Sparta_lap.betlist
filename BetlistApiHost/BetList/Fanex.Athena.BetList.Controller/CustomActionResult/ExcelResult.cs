namespace Fanex.Athena.BetList.Controller.CustomActionResult
{
    using BetListNPOI.SS.UserModel;
    using System.Web.Mvc;

    public class ExcelResult : ActionResult
    {
        private readonly IWorkbook _hssfWorkBook;

        public ExcelResult(IWorkbook hssfWorkbook)
        {
            this._hssfWorkBook = hssfWorkbook;
        }

        public override void ExecuteResult(ControllerContext context)
        {
            var response = context.HttpContext.Response;
            response.Clear();
            this._hssfWorkBook.Write(response.OutputStream, true);
        }
    }
}