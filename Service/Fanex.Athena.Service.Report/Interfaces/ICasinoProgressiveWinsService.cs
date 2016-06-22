// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ICasinoProgressiveWinsService.cs" company="">
// TODO: Update copyright text.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Fanex.Athena.Service.Report.Interfaces
{
    using System.Collections.Generic;
    using Fanex.Athena.Model.Report.DTO;
    using Fanex.Athena.Model.Report.MetaModel;

    public interface ICasinoProgressiveWinsService
    {
        IEnumerable<CasinoProgressiveWins> GetDataList(CasinoProgressiveWinsMetaModel metaModel);
    }
}