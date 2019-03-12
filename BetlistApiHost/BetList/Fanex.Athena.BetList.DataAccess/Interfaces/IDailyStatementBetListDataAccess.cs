namespace Fanex.Athena.BetList.DataAccess.Interfaces
{
    using System.Collections.Generic;
    using Fanex.Athena.BetList.Model.DTO;
    using Fanex.Athena.BetList.Model.MetaModel;

    public interface IDailyStatementBetListDataAccess
    {
        IEnumerable<DailyStatement> GetDailyStatementData(BetListMetaModel metaModel);
    }
}