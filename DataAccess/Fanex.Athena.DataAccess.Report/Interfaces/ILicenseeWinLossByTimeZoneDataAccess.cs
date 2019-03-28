namespace Fanex.Athena.DataAccess.Report.Interfaces
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Fanex.Athena.Model.Report.MetaModel;
    using Model.Core.ResultModel;
    using Model.Report.DTO;

    public interface ILicenseeWinLossByTimeZoneDataAccess
    {
        Task<ReportResultModel<LicenseeWinLossByTimeZone>> GetData(LicenseeWinLossByTimeZoneMetaModel metaModel, IDictionary<int, string> currencyList);
    }
}