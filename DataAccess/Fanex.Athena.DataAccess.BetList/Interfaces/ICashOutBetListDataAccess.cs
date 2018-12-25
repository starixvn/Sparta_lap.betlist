namespace Fanex.Athena.DataAccess.BetList.Interfaces
{
    using Fanex.Athena.Model.BetList.MetaModel;
    using Model.BetList.ViewModel;
    using System.Collections.Generic;

    public interface ICashOutBetListDataAccess
    {
        CashOutBetListDetailViewModel GetDetail(CashOutBetListDetailMetaModel metaModel, IDictionary<int, string> currencyList);
    }
}