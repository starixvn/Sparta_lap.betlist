namespace Fanex.Athena.BetList.DataAccess.Interfaces
{
    using System.Collections.Generic;
    using BetList.Model.ViewModel;
    using Fanex.Athena.BetList.Model.MetaModel;

    public interface ICashOutBetListDataAccess
    {
        CashOutBetListDetailViewModel GetDetail(CashOutBetListDetailMetaModel metaModel, IDictionary<int, string> currencyList);
    }
}