// -----------------------------------------------------------------------
// <copyright file="NumberGameViewModel.cs" company="">
// warehouse winloss form parameters
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Model.TotalBet.ViewModel
{
    using System.Collections.Generic;
    using DTO;
    using MetaModel;

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