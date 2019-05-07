namespace Fanex.Athena.DataAccess.Report.Interfaces
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Fanex.Athena.Model.Report.DTO;
    using Fanex.Athena.Model.Report.MetaModel;
    using Model.Core.ResultModel;

    public interface ICustomerTurnoverDataAccess
    {
        Task<ReportResultModel<CustTurnover>> GetDataList(CustomerTurnoverMetaModel metaModel, IDictionary<int, string> currencyList, string languageCode);
    }
}