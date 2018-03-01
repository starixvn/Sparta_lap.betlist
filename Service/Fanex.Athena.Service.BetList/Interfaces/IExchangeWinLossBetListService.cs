namespace Fanex.Athena.Service.BetList.Interfaces
{
    using Fanex.Athena.Model.BetList.MetaModel;
    using Model.BetList.ViewModel;

    public interface IExchangeWinLossBetListService
    {
        ExchangeBetListSummaryViewModel GetBetListData(BetListMetaModel metaModel);
    }
}