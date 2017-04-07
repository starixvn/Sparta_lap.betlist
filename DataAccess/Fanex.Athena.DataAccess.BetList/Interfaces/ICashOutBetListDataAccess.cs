namespace Fanex.Athena.DataAccess.BetList.Interfaces
{
    using Fanex.Athena.Model.BetList.MetaModel;
    using Model.BetList.ViewModel;

    public interface ICashOutBetListDataAccess
    {
        CashOutBetListDetailViewModel GetDetail(CashOutBetListDetailMetaModel metaModel);
    }
}