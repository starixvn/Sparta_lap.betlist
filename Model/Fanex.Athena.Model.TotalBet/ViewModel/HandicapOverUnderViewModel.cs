// -----------------------------------------------------------------------
// <copyright file="HandicapOverUnderViewModel.cs" company="">
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Model.TotalBet.ViewModel
{
    using System.Collections.Generic;
    using DTO;
    using MetaModel;

    /// <summary>
    /// HandicapOverUnderViewModel class.
    /// </summary>
    public class HandicapOverUnderViewModel : TotalBetsBaseViewModel
    {
        public HandicapOverUnderMetaModel Param { get; set; }

        public IEnumerable<HandicapOverUnder> MainData { get; set; }

        public HandicapOverUnder TotalData { get; set; }
    }
}