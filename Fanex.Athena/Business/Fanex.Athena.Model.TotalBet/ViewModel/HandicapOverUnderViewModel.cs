// -----------------------------------------------------------------------
// <copyright file="HandicapOverUnderViewModel.cs" company="Nexcel">
// warehouse winloss form parameters
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

        public IList<HandicapOverUnder> MainData { get; set; }

        public HandicapOverUnder TotalData { get; set; }
    }
}