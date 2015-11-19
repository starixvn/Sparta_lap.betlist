// -----------------------------------------------------------------------
// <copyright file="CasinoProgressiveWins.cs" company="Nexcel">
// Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Models.ViewModel.Report
{
    using System.Collections.Generic;
    using Fanex.Athena.Models.DTO.Report;
    using Fanex.Athena.Models.MetaModel.Report;

    public class CasinoProgressiveWinsViewModel : BaseViewModel
    {
        public IEnumerable<CasinoProgressiveWins> DataList { get; set; }

        public CasinoProgressiveWinsMetaModel Param { get; set; }
    }
}