namespace Fanex.Athena.DataAccess.Report.Interfaces
{
    using System.Collections.Generic;
    using Model.Core.ResultModel;
    using Model.Report;
    using Model.Report.MetaModel;

    public interface IPromotionPrizeDataAccess
    {
        ReportResultModel<PromotionPrize> GetData(PromotionPrizeMetaModel metaModel, IDictionary<int, string> currencyList);
    }
}