// -----------------------------------------------------------------------
// <copyright file="RacingCommissionViewModel.cs" company="Nexcel">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Models.ViewModel.Report
{
    using System.Collections.Generic;
    using Fanex.Athena.Models.DTO.Report;
    using Fanex.Athena.Models.MetaModel.Report;

    /// <summary>
    /// RacingCommissionViewModel class.
    /// </summary>
    public class RacingCommissionViewModel : BaseViewModel
    {
        public RacingCommissionMetaModel Param { get; set; }

        public IList<RacingCommission> RacingCommissionList { get; set; }

        public RacingCommission Total { get; set; }
    }
}