// -----------------------------------------------------------------------
// <copyright file="ICoreBetSettingService.cs" company="Nexcel">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Service.BetSettings
{
    using Fanex.Athena.Model.Core.DTO.Common;
    using Fanex.Athena.Models.MetaModel.Casino;
    using Models.DTO.Customer;
    using Models.ViewModel.BetSettings;

    public interface IRngCasinoBetSettingService
    {
        RngCasinoBetSettingViewModel GetViewModel(int custId, CustomerInfo customer, string language, string licenseeName);

        IResultInfo UpdateStatus(int userId, int custId, int status, string language, string ip, string username, bool isSyncCasino);

        IResultInfo UpdateSetting(RngCasinoBetSettingMetaModel metaModel, bool isSyncCasino, bool isDisableCasino);
    }
}