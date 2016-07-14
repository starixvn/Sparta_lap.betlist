// -----------------------------------------------------------------------
// <copyright file="ICasinoProgressiveWinsDataAccess.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.DataAccess.Report.Interfaces
{
    using System.Collections.Generic;
    using Fanex.Athena.Model.Report.DTO;
    using Fanex.Athena.Model.Report.MetaModel;

    public interface ICasinoProgressiveWinsDataAccess
    {
        IEnumerable<CasinoProgressiveWins> GetDataList(CasinoProgressiveWinsMetaModel metaModel);
    }
}