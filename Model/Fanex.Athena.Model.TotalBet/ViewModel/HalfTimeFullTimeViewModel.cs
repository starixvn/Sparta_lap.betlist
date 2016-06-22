// -----------------------------------------------------------------------
// <copyright file="HalfTimeFullTimeViewModel.cs" company="">
// warehouse winloss form parameters
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Model.TotalBet.ViewModel
{
    using System.Collections.Generic;
    using DTO;
    using MetaModel;

    /// <summary>
    /// HalfTimeFullTimeViewModel class.
    /// </summary>
    public class HalfTimeFullTimeViewModel : TotalBetsBaseViewModel
    {
        public HalfTimeFullTimeMetaModel Param { get; set; }

        public IList<HalfTimeFullTime> MainData { get; set; }

        public decimal TotalRunningStake { get; set; }
    }
}