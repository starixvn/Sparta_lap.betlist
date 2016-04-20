// -----------------------------------------------------------------------
// <copyright file="GoldDeluxeBetSettingService.cs" company="Nexcel">
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
    using Model.Core.DataDomain;
    using Model.Core.DTO.Common;
    using Models.DTO.BetSettings.AgCasino;
    using Models.ViewModel.BetSettings;
    using Resources;

    public class GoldDeluxeBetSettingService : BetSettingBaseService, IGoldDeluxeBetSettingService
    {
        private const int GoldDeluxeThirdPartyId = 5;

        private const string DefaultGroup = "default";

        private readonly IThirdPartyBetSettingDataAccess _betSettingDataAccess;

        public GoldDeluxeBetSettingService()
        {
            _betSettingDataAccess = new ThirdPartyBetSettingDataAccess();
        }

        public GoldDeluxeBetSettingViewModel GetViewModel(int custId, string siteName)
        {
            if (!HasPermissionOnData(custId))
            {
                return null;
            }

            var thirdPartySetting = _betSettingDataAccess.GetBetSetting(custId, ProductTypes.GoldDeluxe);
            var setting = new GoldDeluxeBetSetting()
            {
                CurrencyId = thirdPartySetting.CurrencyId,
                IsEnabled = thirdPartySetting.IsEnabled,
                IsLineEnabled = thirdPartySetting.IsLineEnabled
            };

            setting.Group = GetGoldDeluxeUserGroup(siteName, custId, setting.CurrencyId);
            var games = GetGoldDeluxeGroupBettingLimit(custId, siteName, setting.CurrencyId);

            if (setting.IsLineEnabled && games.Count > 0)
            {
                setting.CheckedGroups = games.SelectMany(x => x.Groups).Select(x => x.GroupType).Distinct().ToArray();
                if (string.IsNullOrWhiteSpace(setting.Group) &&
                    setting.CheckedGroups.Any(x => x.Equals(DefaultGroup, StringComparison.OrdinalIgnoreCase)))
                {
                    setting.Group = DefaultGroup;
                }
                else if (setting.CheckedGroups.Contains(setting.Group) == false)
                {
                    setting.CheckedGroups =
                        setting.CheckedGroups.Concat(new string[] { setting.Group })
                        .OrderBy(x => x).ToArray();
                }
            }

            return new GoldDeluxeBetSettingViewModel()
            {
                BetSetting = setting,
                Games = games,
                IsEnabled = setting.IsEnabled
            };
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes",
            Justification = "Reviewed. Suppression is OK here.")]
        public IResultInfo UpdateBetSetting(string siteName, int custId, string group)
        {
            var setting = _betSettingDataAccess.GetBetSetting(custId, ProductTypes.GoldDeluxe);
            if (!setting.IsEnabled || !HasPermissionOnData(custId))
            {
                return ErrorResult(ViewResources.UpdateSpecialLevelMsg);
            }

            var uri = new Uri(AppSettings.GetValue("GoldDeluxeSetUserGroupApiUrl"));
            var parameters = string.Format("SiteName={0}&IBCCustID={1}&ThirdPartyId={2}&GroupType={3}", siteName, custId, GoldDeluxeThirdPartyId, group);
            var apiResult = string.Empty;

            try
            {
                using (WebClient webClient = new WebClient())
                {
                    webClient.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";
                    apiResult = webClient.UploadString(uri, parameters);
                }

                var xml = XDocument.Parse(apiResult);
                var rootElement = xml.Element("SetUserGroupBettingLimit");
                if (rootElement.Element("ReturnCode").Value.Equals("0"))
                {
                    return new ResultInfo();
                }
            }
            catch (Exception exception)
            {
                LogHelper.WriteError(exception);
            }

            return ErrorResult(ViewResources.UpdateSpecialLevelMsg);
        }

        public IResultInfo UpdateStatus(int userId, int custId, string language, string ip, bool isEnabled)
        {
            if (!HasPermissionOnData(custId))
            {
                return ErrorResult(ViewResources.UpdateSpecialLevelMsg);
            }

            var setting = _betSettingDataAccess.GetBetSetting(custId, ProductTypes.GoldDeluxe);
            if (!setting.IsLineEnabled)
            {
                return ErrorResult(ViewResources.UpdateSpecialLevelMsg);
            }

            return _betSettingDataAccess.UpdateStatus(ProductTypes.GoldDeluxe, userId, custId, language, ip, isEnabled);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes",
            Justification = "Reviewed. Suppression is OK here.")]
        private IList<GoldDeluxeGame> GetGoldDeluxeGroupBettingLimit(int custId, string siteName, int currency)
        {
            if (!HasPermissionOnData(custId))
            {
                return new List<GoldDeluxeGame>();
            }

            var uri = new Uri(AppSettings.GetValue("GoldDeluxeGroupBettingLimitApiUrl"));
            var parameters = string.Format("SiteName={0}&Currency={1}&ThirdPartyId={2}", siteName, currency, GoldDeluxeThirdPartyId);
            var apiResult = string.Empty;
            var games = new List<GoldDeluxeGame>();
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
                    var game = new GoldDeluxeGame()
                    {
                        GameType = xmlGame.Attribute("GameType").Value,
                        Name = xmlGame.Attribute("Name").Value,
                        Groups = new List<GoldDeluxeGroup>()
                    };

                    foreach (var xmlGroup in xmlGame.Descendants("Group"))
                    {
                        game.Groups.Add(new GoldDeluxeGroup()
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

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes",
            Justification = "Reviewed. Suppression is OK here.")]
        private string GetGoldDeluxeUserGroup(string siteName, int custId, int currencyId)
        {
            var uri = new Uri(AppSettings.GetValue("GoldDeluxeGetUserGroupApiUrl"));
            var parameters = string.Format("SiteName={0}&IBCCustID={1}&ThirdPartyId={2}&Currency={3}", siteName, custId, GoldDeluxeThirdPartyId, currencyId);
            var apiResult = string.Empty;

            try
            {
                using (WebClient webClient = new WebClient())
                {
                    webClient.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";
                    apiResult = webClient.UploadString(uri, parameters);
                }

                var xml = XDocument.Parse(apiResult);

                var rootElement = xml.Element("GetUserGroupBettingLimit");
                if (rootElement.Element("ReturnCode").Value.Equals("0"))
                {
                    return rootElement.Element("ReturnMessage").Value;
                }
            }
            catch (Exception exception)
            {
                LogHelper.WriteError(exception);
            }

            return string.Empty;
        }
    }
}