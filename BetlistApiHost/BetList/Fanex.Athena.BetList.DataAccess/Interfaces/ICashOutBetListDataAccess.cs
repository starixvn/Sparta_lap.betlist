namespace Fanex.Athena.BetList.DataAccess.Interfaces
{
    using System.Collections.Generic;
    using Fanex.Athena.BetList.Model.MetaModel;
    using BetList.Model.ViewModel;

    public interface ICashOutBetListDataAccess
    {
        CashOutBetListDetailViewModel GetDetail(CashOutBetListDetailMetaModel metaModel, IDictionary<int, string> currencyList);
    }
}