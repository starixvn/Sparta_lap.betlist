// -----------------------------------------------------------------------
// <copyright file="BetSettingBaseViewModel.cs" company="Nexcel">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Model.BetSettings.ViewModel
{
    using System.Collections.Generic;
    using Fanex.Athena.Model.BetSettings.DTO;
    using Fanex.Athena.Model.BetSettings.MetaModel;

    public class BetSettingBaseViewModel
    {
        public BetSettingMetaModel Param { get; set; }

        public bool? IsEnabled { get; set; }

        public bool Editable { get; set; }

        public IEnumerable<BetSettingProduct> BetSettingProducts { get; set; }
    }
}