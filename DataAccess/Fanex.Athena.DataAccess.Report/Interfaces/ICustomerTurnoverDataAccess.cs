namespace Fanex.Athena.DataAccess.Report.Interfaces
{
    using Fanex.Athena.Model.Report.DTO;
    using Fanex.Athena.Model.Report.MetaModel;
    using Model.Core.ResultModel;

    public interface ICustomerTurnoverDataAccess
    {
        ReportResultModel<CustTurnover> GetDataList(CustomerTurnoverMetaModel metaModel, string languageCode);
    }
}