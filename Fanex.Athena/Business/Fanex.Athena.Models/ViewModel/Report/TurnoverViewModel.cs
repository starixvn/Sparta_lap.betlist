// -----------------------------------------------------------------------
// <copyright file="TurnoverViewModel.cs" company="Nexcel">
// warehouse winloss form parameters
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Models.ViewModel.Report
{
    using System.Collections.Generic;
    using Fanex.Athena.Models.DTO.Report;
    using Fanex.Athena.Models.MetaModel.Report;
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