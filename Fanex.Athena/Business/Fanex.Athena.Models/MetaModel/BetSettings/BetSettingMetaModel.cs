// -----------------------------------------------------------------------
// <copyright file="BetSettingMetaModel.cs" company="Nexcel">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Models.MetaModel.BetSettings
{
    using System.Collections.Generic;
    using Fanex.Athena.Models.DataDomain;
    using Fanex.Athena.Models.DTO.BetSettings;
    using Fanex.Athena.Models.ViewModel;

    /// <summary>
    /// BetSettingMetaModel class.
    /// </summary>
    public class BetSettingMetaModel
    {
        public BetSettingMetaModel()
        {
            ProductId = ProductTypes.Sportsbook;
            BetSettingName = ViewResources.Sportsbook;
        }

        public int ProductId { get; set; }

        public int CustId { get; set; }

        public int CurrencyId { get; set; }

        public string UserName { get; set; }

        public string NickName { get; set; }

        public string BetSettingName { get; set; }
    }
}