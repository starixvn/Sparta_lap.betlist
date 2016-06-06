// -----------------------------------------------------------------------
// <copyright file="MoneyLineViewModel.cs" company="Nexcel">
// warehouse winloss form parameters
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Model.TotalBet.ViewModel
{
    using System.Collections.Generic;
    using DTO;
    using MetaModel;

    /// <summary>
    /// MoneyLineViewModel class.
    /// </summary>
    public class MoneyLineViewModel : TotalBetsBaseViewModel
    {
        public MoneyLineMetaModel Param { get; set; }

        public IList<MoneyLine> MainData { get; set; }

        public MoneyLine TotalData { get; set; }
    }
}