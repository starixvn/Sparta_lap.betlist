// -----------------------------------------------------------------------
// <copyright file="OddEvenDrawNoDrawViewModel.cs" company="Nexcel">
// warehouse winloss form parameters
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Models.ViewModel.TotalBets
{
    using System.Collections.Generic;
    using DTO.TotalBets;
    using MetaModel.TotalBets;

    /// <summary>
    /// OddEvenDrawNoDrawViewModel class.
    /// </summary>
    public class OddEvenDrawNoDrawViewModel : TotalBetsBaseViewModel
    {
        public OddEvenDrawNoDrawMetaModel Param { get; set; }

        public IList<OddEvenDrawNoDraw> MainData { get; set; }
    }
}