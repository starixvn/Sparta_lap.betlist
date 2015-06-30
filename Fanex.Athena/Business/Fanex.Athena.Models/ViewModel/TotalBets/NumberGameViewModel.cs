// -----------------------------------------------------------------------
// <copyright file="NumberGameViewModel.cs" company="Nexcel">
// warehouse winloss form parameters
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Models.ViewModel.TotalBets
{
    using System.Collections.Generic;
    using DTO.TotalBets;
    using MetaModel.TotalBets;

    /// <summary>
    /// NumberGameViewModel class.
    /// </summary>
    public class NumberGameViewModel : TotalBetsBaseViewModel
    {
        public IList<NumberGame> MainData { get; set; }

        public NumberGameMetaModel Param { get; set; }

        public NumberGame TotalData { get; set; }
    }
}