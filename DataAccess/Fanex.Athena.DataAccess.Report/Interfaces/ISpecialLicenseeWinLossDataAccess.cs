namespace Fanex.Athena.DataAccess.Report.Interfaces
{
    using System.Collections.Generic;
    using Model.Core.ResultModel;
    using Model.Report.DTO;
    using Model.Report.MetaModel;

    public interface ISpecialLicenseeWinLossDataAccess
    {
        ReportResultModel<LicenseeWinLoss> GetData(SpecialLicenseeWinLossMetaModel metaModel, IDictionary<int, string> currencyList);
    }
}