// -----------------------------------------------------------------------
// <copyright file="HalfTimeFullTimeViewModel.cs" company="Nexcel">
// warehouse winloss form parameters
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Models.ViewModel.TotalBets
{
    using System.Collections.Generic;
    using DTO.TotalBets;
    using MetaModel.TotalBets;

    /// <summary>
    /// HalfTimeFullTimeViewModel class.
    /// </summary>
    public class HalfTimeFullTimeViewModel : TotalBetsBaseViewModel
    {
        public HalfTimeFullTimeMetaModel Param { get; set; }

        public IList<HalfTimeFullTime> MainData { get; set; }

        public HalfTimeFullTime TotalData { get; set; }
    }
}