namespace Fanex.Athena.DataAccess.BetList.Interfaces
{
    using System.Collections.Generic;
    using Fanex.Athena.Model.BetList.DTO;
    using Fanex.Athena.Model.BetList.MetaModel;

    public interface IDailyStatementBetListDataAccess
    {
        IEnumerable<DailyStatement> GetDailyStatementData(BetListMetaModel metaModel, bool allowCustComm);
    }
}