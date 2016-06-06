// -----------------------------------------------------------------------
// <copyright file="IOutrightService.cs" company="Nexcel">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Service.TotalBet.Interfaces
{
    using System.Collections.Generic;
    using Model.TotalBet.DTO;
    using Model.TotalBet.MetaModel;

    public interface IOutrightService
    {
        IList<Outright> GetOutrightDataList(OutrightMetaModel metaModel);
    }
}