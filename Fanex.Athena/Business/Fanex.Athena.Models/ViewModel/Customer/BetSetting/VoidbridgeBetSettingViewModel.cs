// -----------------------------------------------------------------------
// <copyright file="VoidbridgeBetSettingViewModel.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Models.ViewModel.Customer.BetSetting
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Fanex.Athena.Models.DTO.Customer.BetSetting.Voidbridge;
    using Fanex.Athena.Models.DTO.Customer.BetSetting.AgCasino;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class VoidbridgeBetSettingViewModel
    {
        public VoidbridgeBetSetting BetSetting { get; set; }

        public IList<GoldDeluxeGame> Games { get; set; }
    }
}
