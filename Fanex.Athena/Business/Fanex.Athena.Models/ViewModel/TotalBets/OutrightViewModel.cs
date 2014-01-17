// -----------------------------------------------------------------------
// <copyright file="OutrightViewModel.cs" company="Nexcel">
// warehouse winloss form parameters
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Models.ViewModel.TotalBets
{
    using System.Collections.Generic;
    using DTO.TotalBets;
    using MetaModel.TotalBets;

    /// <summary>
    /// OutrightViewModel class.
    /// </summary>
    public class OutrightViewModel : TotalBetsBaseViewModel
    {
        public OutrightMetaModel Param { get; set; }

        public IList<Outright> MainData { get; set; }
    }
}