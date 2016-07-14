// -----------------------------------------------------------------------
// <copyright file="HomeDrawAwayNoBetViewModel.cs" company="">
// warehouse winloss form parameters
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Model.TotalBet.ViewModel
{
    using System.Collections.Generic;
    using DTO;
    using MetaModel;

    /// <summary>
    /// HomeDrawAwayNoBetViewModel class.
    /// </summary>
    public class HomeDrawAwayNoBetViewModel : TotalBetsBaseViewModel
    {
        public HomeDrawAwayNoBetMetaModel Param { get; set; }

        public IList<HomeDrawAwayNoBet> MainData { get; set; }
    }
}