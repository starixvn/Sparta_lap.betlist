// -----------------------------------------------------------------------
// <copyright file="RacingCommissionViewModel.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Model.Report.ViewModel
{
    using System.Collections.Generic;
    using Fanex.Athena.Model.Report.DTO;
    using Fanex.Athena.Model.Report.MetaModel;
    using Model.Core.ViewModel;

    /// <summary>
    /// RacingCommissionViewModel class.
    /// </summary>
    public class RacingCommissionViewModel : BaseViewModel
    {
        public RacingCommissionMetaModel Param { get; set; }

        public IList<RacingCommission> RacingCommissionList { get; set; }

        public RacingCommission Total { get; set; }

        public int TotalRows { get; set; }
    }
}