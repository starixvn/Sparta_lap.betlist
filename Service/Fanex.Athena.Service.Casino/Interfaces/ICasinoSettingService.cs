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
    using Model.Customer.MetaModel;

    public interface ICasinoSettingService
    {
        IList<GameKind> GetRngCasinoSettings(int custId, string language);

        bool UpdateSetting(DataTable dataTableParam);
    }
}