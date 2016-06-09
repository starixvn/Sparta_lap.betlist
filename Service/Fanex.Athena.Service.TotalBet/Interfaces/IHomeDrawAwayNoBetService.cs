// -----------------------------------------------------------------------
// <copyright file="IHomeDrawAwayNoBetService.cs" company="Nexcel">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Service.TotalBet.Interfaces
{
    using System.Collections.Generic;
    using Model.TotalBet.DTO;
    using Model.TotalBet.MetaModel;

    public interface IHomeDrawAwayNoBetService
    {
        IList<HomeDrawAwayNoBet> GetHomeDrawAwayNoBetDataList(HomeDrawAwayNoBetMetaModel metaModel);
    }
}