// -----------------------------------------------------------------------
// <copyright file="OddEvenDrawNoDrawViewModel.cs" company="Nexcel">
// warehouse winloss form parameters
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Model.TotalBet.ViewModel
{
    using System.Collections.Generic;
    using DTO;
    using MetaModel;

    /// <summary>
    /// OddEvenDrawNoDrawViewModel class.
    /// </summary>
    public class OddEvenDrawNoDrawViewModel : TotalBetsBaseViewModel
    {
        public OddEvenDrawNoDrawMetaModel Param { get; set; }

        public IList<OddEvenDrawNoDraw> MainData { get; set; }

        public OddEvenDrawNoDraw TotalData { get; set; }
    }
}