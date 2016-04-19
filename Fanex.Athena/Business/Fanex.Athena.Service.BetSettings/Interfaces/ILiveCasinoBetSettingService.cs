// -----------------------------------------------------------------------
// <copyright file="ILiveCasinoBetSettingService.cs" company="Nexcel">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Service.BetSettings
{
    using System.Collections.Generic;
    using Fanex.Athena.Models.DTO.BetSettings;
    using Fanex.Athena.Models.DTO.Common;
    using Fanex.Athena.Models.ViewModel.BetSettings;

    public interface ILiveCasinoBetSettingService
    {
        LiveCasinoBetSettingViewModel GetViewModel(int custId, string language);

        IResultInfo UpdateBetSetting(int userId, int custId, List<LiveCasinoBetSetting> metaModel, string ip, string language, bool isDisableLiveCasino);

        IResultInfo UpdateStatus(int userId, int custId, int status, string language, string ip, bool isSyncLiveCasino);
    }
}