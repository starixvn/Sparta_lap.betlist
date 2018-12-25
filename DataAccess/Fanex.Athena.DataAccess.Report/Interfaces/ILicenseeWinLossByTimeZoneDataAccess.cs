namespace Fanex.Athena.DataAccess.Report.Interfaces
{
    using Fanex.Athena.Model.Report.MetaModel;
    using Model.Core.ResultModel;
    using Model.Report.DTO;
    using System.Collections.Generic;

    public interface ILicenseeWinLossByTimeZoneDataAccess
    {
        ReportResultModel<LicenseeWinLossByTimeZone> GetData(LicenseeWinLossByTimeZoneMetaModel metaModel, IDictionary<int, string> currencyList);
    }
}