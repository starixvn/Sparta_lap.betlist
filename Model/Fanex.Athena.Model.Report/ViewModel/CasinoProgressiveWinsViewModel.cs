// -----------------------------------------------------------------------
// <copyright file="CasinoProgressiveWins.cs" company="">
// Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Model.Report.ViewModel
{
    using System.Collections.Generic;
    using Fanex.Athena.Model.Report.DTO;
    using Fanex.Athena.Model.Report.MetaModel;
    using Model.Core.ViewModel;

    public class CasinoProgressiveWinsViewModel : BaseViewModel
    {
        public IEnumerable<CasinoProgressiveWins> DataList { get; set; }

        public CasinoProgressiveWinsMetaModel Param { get; set; }
    }
}