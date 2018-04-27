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

    public interface ICasinoSettingService
    {
        IList<GameKind> GetRngCasinoSettings(int custId, int currencyId, string language);

        bool UpdateSetting(DataTable dataTableParam);
    }
}