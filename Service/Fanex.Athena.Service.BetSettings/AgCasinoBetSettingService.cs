// -----------------------------------------------------------------------
// <copyright file="AgCasinoBetSettingService.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Service.BetSettings
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Xml.Linq;
    using Common;
    using Core.Utility.Interfaces;
    using DataAccess.BetSettings.Interfaces;
    using Extension.Utility;
    using Extension.Utility.Interfaces;
    using Fanex.Athena.Service.StatelessCore.Interfaces;
    using Interfaces;
    using Model.BetSettings.DTO;
    using Model.BetSettings.DTO.AgCasino;
    using Model.BetSettings.ViewModel;
    using Model.Core.DTO.Common;
    using Resources;

    public class AgCasinoBetSettingService : BetSettingBaseService, IAgCasinoBetSettingService
    {
        private readonly ICoreLogger _logger;

        private readonly IAgCasinoBetSettingDataAccess _betSettingDataAccess;

        private readonly IAppSettings _appSettings;

        private readonly IWebClientService _webClientService;
        
        public AgCasinoBetSettingService(
            IAuthorizationService authorizationService,
            ICoreLogger logger,
            IAgCasinoBetSettingDataAccess betSettingDataAccess,
            IAppSettings appSettings,
            IWebClientService webClientService)
            : base(authorizationService)
        {
            _logger = logger;
            _betSettingDataAccess = betSettingDataAccess;
            _appSettings = appSettings;
            _webClientService = webClientService;
        }

        public async Task<AgCasinoBetSettingViewModel> GetViewModelAsync(int licenseeId, int custId, string licenseeName)
        {
            if (!HasPermissionOnData(licenseeId, custId))
            {
                return null;
            }

            var betSetting = await _betSettingDataAccess.GetBetSettingAsync(custId);

            if (!betSetting.CheckedGroups.Contains(betSetting.AgGroup))
            {
                betSetting.CheckedGroups = betSetting.CheckedGroups.Concat(new[] { betSetting.AgGroup })
                    .OrderBy(x => x).ToArray();
            }

            var games = await GetGroupBettingsAsync(custId, licenseeName, betSetting.CurrencyId).ConfigureAwait(false);

            return new AgCasinoBetSettingViewModel
            {
                BetSetting = betSetting,
                Games = games,
                IsEnabled = betSetting.IsEnabled
            };
        }

        public async Task<IResultInfo> UpdateAgCasinoBetSettingAsync(CommonBetSetting setting, string group)
        {
            if (setting == null)
            {
                return ErrorResult("Invalid parameter. AG Casino is null");
            }

            var agcasinoSetting = await _betSettingDataAccess.GetBetSettingAsync(setting.CustId).ConfigureAwait(false);

            if (!agcasinoSetting.IsEnabled || !HasPermissionOnData(setting.LicenseeId, setting.CustId))
            {
                return ErrorResult(ViewResources.AccessDeniedMsg);
            }

            return await _betSettingDataAccess.UpdateBetSettingAsync(setting, group);
        }

        public async Task<IResultInfo> UpdateStatusAsync(int licenseeId, int userId, int custId, string language, string ip, bool isEnabled)
        {
            if (!HasPermissionOnData(licenseeId, custId))
            {
                return ErrorResult(ViewResources.AccessDeniedMsg);
            }

            return await _betSettingDataAccess.UpdateStatusAsync(userId, custId, language, ip, isEnabled);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes",
            Justification = "Reviewed. Suppression is OK here.")]
        private async Task<IList<AgCasinoGame>> GetGroupBettingsAsync(int custId, string siteName, int currency)
        {
            var uri = new Uri(_appSettings.GetValue("AGCasinoApiUrl"));
            var parameters = string.Format("SiteName={0}&Currency={1}", siteName, currency);
            var games = new List<AgCasinoGame>();
            try
            {
                var apiResult = await _webClientService.UploadStringAsync(uri, parameters).ConfigureAwait(false);

                var xml = XDocument.Parse(apiResult);

                foreach (var xmlGame in xml.Descendants("Game"))
                {
                    var game = new AgCasinoGame
                    {
                        GameType = xmlGame.Attribute("GameType").Value,
                        Name = xmlGame.Attribute("Name").Value,
                        Groups = new List<AgCasinoGroup>()
                    };

                    foreach (var xmlGroup in xmlGame.Descendants("Group"))
                    {
                        game.Groups.Add(new AgCasinoGroup
                        {
                            GroupType = xmlGroup.Attribute("GroupType").Value,
                            Min = Converter.ToInt(xmlGroup.Element("Min").Value),
                            Max = Converter.ToInt(xmlGroup.Element("Max").Value)
                        });
                    }

                    games.Add(game);
                }
            }
            catch (Exception exception)
            {
                _logger.Error(exception);
            }

            return games;
        }
    }
}