// -----------------------------------------------------------------------
// <copyright file="ICasinoSetting.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Service.Core.Casino.Interfaces
{
    using System.Collections.Generic;
    using System.Data;
    using Fanex.Athena.Models.DTO.BetSettings.RngCasino;
    using Fanex.Athena.Models.MetaModel.Casino;

    public interface ICasinoSetting
    {
        bool UpdateCustSuspend(CasinoStatusMetaModel metaModel);

        bool UpdateCustClosed(CasinoStatusMetaModel metaModel);

        IList<GameKind> GetRngCasinoSettings(int custId, string licenseeName, string language);

        bool UpdateRngCasinoSetting(DataTable dataTableParam, string licenseeName);
    }
}