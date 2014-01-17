// -----------------------------------------------------------------------
// <copyright file="MoneyLineViewModel.cs" company="Nexcel">
// warehouse winloss form parameters
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Models.ViewModel.TotalBets
{
    using System.Collections.Generic;
    using DTO.TotalBets;
    using MetaModel.TotalBets;

    /// <summary>
    /// MoneyLineViewModel class.
    /// </summary>
    public class MoneyLineViewModel : TotalBetsBaseViewModel
    {
        public MoneyLineMetaModel Param { get; set; }

        public IList<MoneyLine> MainData { get; set; }
    }
}