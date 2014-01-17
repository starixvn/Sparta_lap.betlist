// -----------------------------------------------------------------------
// <copyright file="ICasinoSetting.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Service.Interface.Casino
{
    using System.Data;
    using Fanex.Athena.Models.DTO.Customer.BetSetting.RngCasino;
    using Fanex.Athena.Models.MetaModel.Casino;

    public interface ICasinoSetting
    {
        bool UpdateCustSuspend(CasinoStatusMetaModel metaModel);

        bool UpdateCustClosed(CasinoStatusMetaModel metaModel);

        RngCasinoBetSetting GetRngCasinoSetting(int custId, string licenseeName);

        bool UpdateRngCasinoSetting(DataTable dataTableParam, string licenseeName);
    }
}