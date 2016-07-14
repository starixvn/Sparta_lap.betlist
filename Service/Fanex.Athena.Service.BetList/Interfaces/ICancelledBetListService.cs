// -----------------------------------------------------------------------
// <copyright file="ICancelledBetListService.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Service.BetList.Interfaces
{
    using System.Collections.Generic;
    using Fanex.Athena.Model.BetList.DTO;
    using Fanex.Athena.Model.BetList.MetaModel;

    public interface ICancelledBetListService
    {
        IList<CustomerCancelBetItem> GetCancelBetCustList(BetListMetaModel metaModel);
    }
}