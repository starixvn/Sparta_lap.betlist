// -----------------------------------------------------------------------
// <copyright file="KenoBetSettingService.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Service.BetSettings
{
    using System.Collections.Generic;
    using System.Text;
    using System.Threading.Tasks;
    using DataAccess.BetSettings.Interfaces;
    using Fanex.Athena.Service.StatelessCore.Interfaces;
    using Interfaces;
    using Model.BetSettings.DTO;
    using Model.BetSettings.ViewModel;
    using Model.Core.DTO.Common;
    using Resources;

    public class KenoBetSettingService : BetSettingBaseService, IKenoBetSettingService
    {
        private readonly IKenoBetSettingDataAccess _betSettingDataAccess;

        public KenoBetSettingService(
            IAuthorizationService authorizationService,
            IKenoBetSettingDataAccess betSettingDataAccess)
            : base(authorizationService)
        {
            _betSettingDataAccess = betSettingDataAccess;
        }

        public async Task<BetSettingViewModel> GetViewModelAsync(int licenseeId, int custId)
        {
            if (!(await HasPermissionOnData(licenseeId, custId).ConfigureAwait(false)))
            {
                return new BetSettingViewModel
                {
                    MainData = new List<BetSetting>()
                };
            }

            var betSettings = await _betSettingDataAccess.GetBetSettingsAsync(custId).ConfigureAwait(false);

            return new BetSettingViewModel
            {
                MainData = betSettings
            };
        }

        public async Task<IResultInfo> UpdateBetSettingAsync(int licenseeId, int custId, int userId, string language, string ip, KenoBetSetting setting)
        {
            const string XMLPattern = "<settings>{0}</settings>";
            const string XMLElement = "<setting GameTypeID=\"{0}\" minbet=\"{1}\" maxbet=\"{2}\" maxpermatch=\"{3}\" ></setting>";
            if (!(await HasPermissionOnData(licenseeId, custId).ConfigureAwait(false)))
            {
                return ErrorResult(ViewResources.AccessDeniedMsg);
            }

            var xmlElementBuilder = new StringBuilder();

            xmlElementBuilder.Append(string.Format(XMLElement, setting.GameTypeID, setting.MinBet, setting.MaxBet, setting.MaxPerMatch));

            var xmlparam = string.Format(XMLPattern, xmlElementBuilder);

            return await _betSettingDataAccess.UpdateBetSettingAsync(
                custId,
                userId,
                language,
                ip,
                xmlparam);
        }
    }
}