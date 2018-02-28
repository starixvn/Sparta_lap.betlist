namespace Fanex.Athena.Service.BetList.Interfaces
{
    using System.Collections.Generic;
    using Fanex.Athena.Model.BetList.MetaModel;
    using Model.BetList.DTO;

    public interface IExchangeWinLossBetListService
    {
        IEnumerable<ExchangeTicket> GetBetListData(BetListMetaModel metaModel);
    }
}