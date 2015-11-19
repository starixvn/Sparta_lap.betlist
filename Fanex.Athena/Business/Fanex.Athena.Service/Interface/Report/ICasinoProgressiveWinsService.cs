// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ICasinoProgressiveWinsService.cs" company="Nexcel">
// TODO: Update copyright text.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Fanex.Athena.Service.Interface.Report
{
    using System.Collections.Generic;
    using Fanex.Athena.Models.DTO.Report;
    using Fanex.Athena.Models.MetaModel.Report;

    public interface ICasinoProgressiveWinsService
    {
        IEnumerable<CasinoProgressiveWins> GetDataList(CasinoProgressiveWinsMetaModel metaModel);
    }
}