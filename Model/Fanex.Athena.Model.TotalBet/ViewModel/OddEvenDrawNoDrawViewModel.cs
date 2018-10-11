// -----------------------------------------------------------------------
// <copyright file="OddEvenDrawNoDrawViewModel.cs" company="">
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

        public IEnumerable<OddEvenDrawNoDraw> MainData { get; set; }

        public OddEvenDrawNoDraw TotalData { get; set; }
    }
}