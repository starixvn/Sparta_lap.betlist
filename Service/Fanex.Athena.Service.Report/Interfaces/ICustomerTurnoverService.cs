namespace Fanex.Athena.Service.Report.Interfaces
{
    using System.Collections.Generic;
    using Fanex.Athena.Model.Report.DTO;
    using Fanex.Athena.Model.Report.MetaModel;
    using Model.Core.ResultModel;

    public interface ICustomerTurnoverService
    {
        ReportResultModel<CustTurnover> GetDataList(CustomerTurnoverMetaModel metaModel, IDictionary<int, string> currencyList, string languageCode);
    }
}