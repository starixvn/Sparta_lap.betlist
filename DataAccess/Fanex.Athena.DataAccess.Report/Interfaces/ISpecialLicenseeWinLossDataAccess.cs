namespace Fanex.Athena.DataAccess.Report.Interfaces
{
    using Model.Core.ResultModel;
    using Model.Report.DTO;
    using Model.Report.MetaModel;
    using System.Collections.Generic;

    public interface ISpecialLicenseeWinLossDataAccess
    {
        ReportResultModel<LicenseeWinLoss> GetData(SpecialLicenseeWinLossMetaModel metaModel, IDictionary<int, string> currencyList);
    }
}