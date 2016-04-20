// -----------------------------------------------------------------------
// <copyright file="ICoreBetSettingService.cs" company="Nexcel">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Service.BetSettings
{
    using Fanex.Athena.Models.DTO.Common;
    using Fanex.Athena.Models.MetaModel.Casino;

    public interface IRngCasinoBetSettingService
    {
        IResultInfo UpdateRngCasinoSetting(RngCasinoBetSettingMetaModel metaModel);

        IResultInfo UpdateRngCasinoStatus(int userId, int custId, int status, string language, string ip, string username);
    }
}