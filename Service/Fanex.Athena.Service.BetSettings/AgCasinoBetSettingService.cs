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
    using System.Net;
    using System.Xml.Linq;
    using Common;
    using Core.Helpers;
    using DataAccess.BetSettings;
    using DataAccess.BetSettings.Interfaces;
    using Extension.Utility;
    using Interfaces;
    using Model.BetSettings.DTO;
    using Model.BetSettings.DTO.AgCasino;
    using Model.BetSettings.ViewModel;
    using Model.Core.DTO.Common;
    using Resources;

    public class AgCasinoBetSettingService : BetSettingBaseService, IAgCasinoBetSettingService
    {
        private readonly IAgCasinoBetSettingDataAccess _betSettingDataAccess;

        private readonly IAppSettings _appSettings;

        public AgCasinoBetSettingService()
        {
            _betSettingDataAccess = new AgCasinoBetSettingDataAccess();
            _appSettings = new AppSettings();
        }

        public AgCasinoBetSettingViewModel GetViewModel(int licenseeId, int custId, string licenseeName)
        {
            if (!HasPermissionOnData(licenseeId, custId))
            {
                return null;
            }

            var betSetting = _betSettingDataAccess.GetBetSetting(custId);

            var viewModel = new AgCasinoBetSettingViewModel();

            viewModel.BetSetting = betSetting;
            viewModel.Games = GetGroupBettings(custId, licenseeName, betSetting.CurrencyId);
            if (betSetting.CheckedGroups.Contains(betSetting.AgGroup) == false)
            {
                betSetting.CheckedGroups = betSetting.CheckedGroups.Concat(new string[] { betSetting.AgGroup })
                    .OrderBy(x => x).ToArray();
            }

            viewModel.IsEnabled = betSetting.IsEnabled;
            return viewModel;
        }

        public IResultInfo UpdateAgCasinoBetSetting(CommonBetSetting setting, string group)
        {
            if (setting == null)
            {
                return ErrorResult("Invalid parameter. AG Casino is null");
            }

            var agcasinoSetting = _betSettingDataAccess.GetBetSetting(setting.CustId);

            if (!agcasinoSetting.IsEnabled || !HasPermissionOnData(setting.LicenseeId, setting.CustId))
            {
                return ErrorResult(ViewResources.UpdateSpecialLevelMsg);
            }

            return _betSettingDataAccess.UpdateBetSetting(setting, group);
        }

        public IResultInfo UpdateStatus(int licenseeId, int userId, int custId, string language, string ip, bool isEnabled)
        {
            if (!HasPermissionOnData(licenseeId, custId))
            {
                return ErrorResult(ViewResources.UpdateSpecialLevelMsg);
            }

            return _betSettingDataAccess.UpdateStatus(userId, custId, language, ip, isEnabled);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes",
            Justification = "Reviewed. Suppression is OK here.")]
        private IList<AgCasinoGame> GetGroupBettings(int custId, string siteName, int currency)
        {
            var uri = new Uri(_appSettings.GetValue("AGCasinoApiUrl"));
            var parameters = string.Format("SiteName={0}&Currency={1}", siteName, currency);
            var apiResult = string.Empty;
            var games = new List<AgCasinoGame>();
            try
            {
                using (WebClient webClient = new WebClient())
                {
                    webClient.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";
                    apiResult = webClient.UploadString(uri, parameters);
                }

                var xml = XDocument.Parse(apiResult);

                foreach (var xmlGame in xml.Descendants("Game"))
                {
                    var game = new AgCasinoGame()
                    {
                        GameType = xmlGame.Attribute("GameType").Value,
                        Name = xmlGame.Attribute("Name").Value,
                        Groups = new List<AgCasinoGroup>()
                    };

                    foreach (var xmlGroup in xmlGame.Descendants("Group"))
                    {
                        game.Groups.Add(new AgCasinoGroup()
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
                LogHelper.WriteError(exception);
            }

            return games;
        }
    }
}