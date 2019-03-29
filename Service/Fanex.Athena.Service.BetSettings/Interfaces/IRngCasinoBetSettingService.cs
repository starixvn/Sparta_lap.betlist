// -----------------------------------------------------------------------
// <copyright file="IRngCasinoBetSettingService.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Service.BetSettings.Interfaces
{
    using System.Threading.Tasks;
    using Fanex.Athena.Model.Core.DTO.Common;
    using Model.BetSettings.MetaModel;
    using Model.BetSettings.ViewModel;
    using Model.Customer.DTO;

    public interface IRngCasinoBetSettingService
    {
        RngCasinoBetSettingViewModel GetViewModel(int custId, CustomerInfo customer, string language);

        Task<IResultInfo> UpdateStatusAsync(BetSettingCasinoStatusMetaModel metaModel);

        Task<IResultInfo> UpdateSetting(RngCasinoBetSettingMetaModel metaModel, bool isSyncCasino, bool isDisableCasino);
    }
}