// -----------------------------------------------------------------------
// <copyright file="BetSettingBaseViewModel.cs" company="Nexcel">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Models.ViewModel.BetSettings
{
    using System.Collections.Generic;
    using Fanex.Athena.Models.DTO.BetSettings;
    using Fanex.Athena.Models.MetaModel.BetSettings;

    public class BetSettingBaseViewModel
    {
        public BetSettingMetaModel Param { get; set; }

        public bool? IsEnabled { get; set; }

        public bool Editable { get; set; }

        public IEnumerable<BetSettingProduct> BetSettingProducts { get; set; }
    }
}