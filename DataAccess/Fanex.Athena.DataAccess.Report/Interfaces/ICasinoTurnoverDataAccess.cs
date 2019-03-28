namespace Fanex.Athena.DataAccess.Report.Interfaces
{
    using System.Threading.Tasks;
    using Model.Core.ResultModel;
    using Model.Report.DTO;
    using Model.Report.MetaModel;

    public interface ICasinoTurnoverDataAccess
    {
        Task<ReportResultModel<CasinoTurnover>> GetData(CasinoTurnoverMetaModel metaModel);
    }
}