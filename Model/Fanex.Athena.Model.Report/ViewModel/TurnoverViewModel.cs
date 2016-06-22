// -----------------------------------------------------------------------
// <copyright file="TurnoverViewModel.cs" company="">
// warehouse winloss form parameters
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Model.Report.ViewModel
{
    using System.Collections.Generic;
    using Fanex.Athena.Model.Report.DTO;
    using Fanex.Athena.Model.Report.MetaModel;
    using Model.Core.ViewModel;

    /// <summary>
    /// TurnoverViewModel class.
    /// </summary>
    public class TurnoverViewModel : BaseViewModel
    {
        public IList<TurnoverDTO> DataList { get; set; }

        public TurnoverMetaModel Param { get; set; }

        public TurnoverDTO TotalData { get; set; }

        public bool IsAllowedCasinoTransfer { get; set; }
    }
}