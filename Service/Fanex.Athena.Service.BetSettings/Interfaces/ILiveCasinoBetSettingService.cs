// -----------------------------------------------------------------------
// <copyright file="ILiveCasinoBetSettingService.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Service.BetSettings.Interfaces
{
    using System.Collections.Generic;
    using Fanex.Athena.Model.BetSettings.DTO;
    using Fanex.Athena.Model.BetSettings.ViewModel;
    using Fanex.Athena.Model.Core.DTO.Common;

    public interface ILiveCasinoBetSettingService
    {
        LiveCasinoBetSettingViewModel GetViewModel(int licenseeId, int custId, string language);

        IResultInfo UpdateBetSetting(
            int licenseeId, 
            int userId, 
            int custId, 
            IList<LiveCasinoBetSetting> metaModel, 
            string ip, 
            string language, 
            bool isDisableLiveCasino);

        IResultInfo UpdateStatus(int licenseeId, int userId, int custId, int status, string language, string ip, bool isSyncLiveCasino);
    }
}