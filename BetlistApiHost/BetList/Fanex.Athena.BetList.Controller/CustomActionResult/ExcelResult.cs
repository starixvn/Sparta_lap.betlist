namespace Fanex.Athena.BetList.Controller.CustomActionResult
{
    using BetListNPOI.SS.UserModel;
    using System.Web.Mvc;

    public class ExcelResult : ActionResult
    {
        private readonly IWorkbook _hssfWorkBook;
        private string _fileName;

        public ExcelResult(IWorkbook hssfWorkbook, string fileName = null)
        {
            _hssfWorkBook = hssfWorkbook;
            _fileName = fileName;
        }

        public override void ExecuteResult(ControllerContext context)
        {
            var response = context.HttpContext.Response;
            response.Clear();
            response.ContentType = "application/vnd.ms-excel";
            if (!string.IsNullOrWhiteSpace(_fileName))
            {
                response.AddHeader("Content-disposition", "attachment; filename=" + _fileName + ".xls");
            }
            _hssfWorkBook.Write(response.OutputStream, true);
        }
    }
}