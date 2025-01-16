namespace Fanex.Athena.BetList.Controller.CustomActionResult
{
    using System.Web.Mvc;
    using BetListNPOI.SS.UserModel;

    public class ExcelResult : ActionResult
    {
        private readonly IWorkbook _hssfWorkBook;
        private readonly string _fileName;

        public ExcelResult(IWorkbook hssfWorkbook, string fileName = null)
        {
            _hssfWorkBook = hssfWorkbook;
            _fileName = fileName;
        }

        public override void ExecuteResult(ControllerContext context)
        {
            var response = context.HttpContext.Response;
            response.Clear();
            response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            if (!string.IsNullOrWhiteSpace(_fileName))
            {
                response.AddHeader("Content-disposition", "attachment; filename=" + _fileName + ".xlsx");
            }
            _hssfWorkBook.Write(response.OutputStream, true);
        }
    }
}