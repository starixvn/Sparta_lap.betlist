// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ICasinoProgressiveWinsService.cs" company="Nexcel">
// TODO: Update copyright text.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Fanex.Athena.Service.Interface.Report
{
    using System.Collections.Generic;
    using Fanex.Athena.Model.Report.DTO;
    using Fanex.Athena.Model.Report.MetaModel;

    public interface ICasinoProgressiveWinsService
    {
        IEnumerable<CasinoProgressiveWins> GetDataList(CasinoProgressiveWinsMetaModel metaModel);
    }
}