namespace Fanex.Athena.Exce.NPOI
{
    using System.IO;
    using System.Web;

    using NPOI.SS.UserModel;

    public interface IExcel
    {
        void AssignData();

        void AssignEmptyRow(ISheet sheet);

        void AssignHeader(ISheet sheet);

        void AssignSheetData(ISheet sheet, int rowFrom, int rowTo);

        void Export();

        void Export(HttpResponse response);

        void Export(Stream stream);

        void LoadTemplate();

        void RemoveTemplate();

        void Write(HttpResponse response);

        void Write(Stream stream);
    }
}