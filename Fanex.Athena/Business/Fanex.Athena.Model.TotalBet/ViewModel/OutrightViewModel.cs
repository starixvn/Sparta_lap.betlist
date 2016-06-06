// -----------------------------------------------------------------------
// <copyright file="OutrightViewModel.cs" company="Nexcel">
// warehouse winloss form parameters
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Model.TotalBet.ViewModel
{
    using System.Collections.Generic;
    using DTO;
    using MetaModel;

    /// <summary>
    /// OutrightViewModel class.
    /// </summary>
    public class OutrightViewModel : TotalBetsBaseViewModel
    {
        public OutrightMetaModel Param { get; set; }

        public IList<Outright> MainData { get; set; }
    }
}