// -----------------------------------------------------------------------
// <copyright file="ICasinoSetting.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Service.Casino.Interfaces
{
    using System.Collections.Generic;
    using System.Data;
    using Fanex.Athena.Model.BetSettings.DTO.RngCasino;
    using Fanex.Athena.Model.Casino.MetaModel;

    public interface ICasinoSetting
    {
        bool UpdateCustSuspend(CasinoStatusMetaModel metaModel);

        bool UpdateCustClosed(CasinoStatusMetaModel metaModel);

        IList<GameKind> GetRngCasinoSettings(int custId, string licenseeName, string language);

        bool UpdateSetting(DataTable dataTableParam, string licenseeName);
    }
}