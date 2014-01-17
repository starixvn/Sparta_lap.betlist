// -----------------------------------------------------------------------
// <copyright file="HomeDrawAwayNoBetViewModel.cs" company="Nexcel">
// warehouse winloss form parameters
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Models.ViewModel.TotalBets
{
    using System.Collections.Generic;
    using DTO.TotalBets;
    using MetaModel.TotalBets;

    /// <summary>
    /// HomeDrawAwayNoBetViewModel class.
    /// </summary>
    public class HomeDrawAwayNoBetViewModel : TotalBetsBaseViewModel
    {
        public HomeDrawAwayNoBetMetaModel Param { get; set; }

        public IList<HomeDrawAwayNoBet> MainData { get; set; }
    }
}