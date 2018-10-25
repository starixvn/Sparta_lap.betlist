namespace Fanex.Athena.DataAccess.Report.Interfaces
{
    using Model.Core.ResultModel;
    using Model.Report.DTO;
    using Model.Report.MetaModel;

    public interface ICasinoTurnoverDataAccess
    {
        ReportResultModel<CasinoTurnover> GetData(CasinoTurnoverMetaModel metaModel);
    }
}