namespace Fanex.Athena.DataAccess.Report.Interfaces
{
    using System.Collections.Generic;
    using Fanex.Athena.Model.Report.MetaModel;
    using Model.Core.ResultModel;
    using Model.Report.DTO;

    public interface ILicenseeWinLossByTimeZoneDataAccess
    {
        ReportResultModel<LicenseeWinLossByTimeZone> GetData(LicenseeWinLossByTimeZoneMetaModel metaModel, IDictionary<int, string> currencyList);
    }
}