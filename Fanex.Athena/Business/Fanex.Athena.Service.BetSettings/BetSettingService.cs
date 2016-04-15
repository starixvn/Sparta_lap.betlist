// -----------------------------------------------------------------------
// <copyright file="BetSettingService.cs" company="Nexcel">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Service.BetSettings
{
    #region

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Text;
    using System.Xml.Linq;
    using CasinoLibrary.SBAgent;
    using DataAccess.BetSettings.Interfaces;
    using Fanex.Athena.DataAccess.BetSettings;
    using Fanex.Athena.Extension.Utility;
    using Fanex.Athena.Models.DataDomain;
    using Fanex.Athena.Models.DTO.BetSettings;
    using Fanex.Athena.Models.DTO.BetSettings.AgCasino;
    using Fanex.Athena.Models.DTO.Common;
    using Fanex.Athena.Models.MetaModel.Casino;
    using Fanex.Athena.Models.ViewModel;
    using Fanex.Athena.Models.ViewModel.BetSettings;
    using Fanex.Athena.Service.BetSettings.Interfaces;
    using Fanex.Athena.Service.Core.Exception;
    using Fanex.Athena.Service.Core.Helpers;
    using Fanex.Athena.Service.Implement.Casino;
    using Fanex.Common;
    using Implement.Customer;
    using Interface.Customer;

    #endregion

    /// <summary>
    ///     Bet setting service.
    /// </summary>
    public class BetSettingService : BetSettingBaseService, IBetSettingService
    {
        #region Constants

        private const int GoldDeluxeThirdPartyId = 5;

        private const string DefaultGoldDeluxeGroup = "default";

        #endregion Constants

        private readonly IBetSettingDataAccess _betSettingDataAccess;
        private readonly ICustomerService _customerService;
        private readonly CasinoSetting _casinoSetting;
        private readonly Customer _customer;

        public BetSettingService()
        {
            _betSettingDataAccess = new BetSettingDataAccess();
            _customerService = new CustomerService();
            _casinoSetting = new CasinoSetting();
            _customer = new Customer();
        }

        #region Gold Deluxe

        /// <summary>
        /// Get Gold Deluxe setting.
        /// </summary>
        /// <param name="custId">Customer Id.</param>
        /// <param name="siteName">Site name.</param>
        /// <returns>Gold Deluxe setting.</returns>
        public GoldDeluxeBetSettingViewModel GetGoldDeluxeViewModel(int custId, string siteName)
        {
            if (!HasPermissionOnData(custId))
            {
                return null;
            }

            var setting = _betSettingDataAccess.GetGoldDeluxeSetting(custId);
            setting.Group = GetGoldDeluxeUserGroup(siteName, custId, setting.CurrencyId);
            var games = GetGoldDeluxeGroupBettingLimit(custId, siteName, setting.CurrencyId);

            if (setting.IsLineEnabled && games.Count > 0)
            {
                setting.CheckedGroups = games.SelectMany(x => x.Groups).Select(x => x.GroupType).Distinct().ToArray();
                if (string.IsNullOrWhiteSpace(setting.Group) &&
                    setting.CheckedGroups.Any(x => x.Equals(DefaultGoldDeluxeGroup, StringComparison.OrdinalIgnoreCase)))
                {
                    setting.Group = DefaultGoldDeluxeGroup;
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

        /// <summary>
        /// Update Gold Deluxe setting.
        /// </summary>
        /// <param name="siteName">Site name.</param>
        /// <param name="custId">Customer id.</param>
        /// <param name="group">Group value..</param>
        /// <returns>Result info value.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes",
            Justification = "Reviewed. Suppression is OK here.")]
        public IResultInfo UpdateGoldDeluxeBetSetting(string siteName, int custId, string group)
        {
            var setting = _betSettingDataAccess.GetGoldDeluxeSetting(custId);
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

        public IResultInfo UpdateGoldDeluxeStatus(int userId, int custId, string language, string ip, bool isEnabled)
        {
            if (!HasPermissionOnData(custId))
            {
                return ErrorResult(ViewResources.UpdateSpecialLevelMsg);
            }

            return _betSettingDataAccess.UpdateGoldDeluxeStatus(userId, custId, language, ip, isEnabled);
        }

        #endregion Gold Deluxe

        #region AgCasino

        public AgCasinoBetSettingViewModel GetAgCasinoViewModel(int custId, string licenseeName)
        {
            if (!HasPermissionOnData(custId))
            {
                return null;
            }

            var betSetting = _betSettingDataAccess.GetAgCasinoSetting(custId);

            var viewModel = new AgCasinoBetSettingViewModel();

            viewModel.BetSetting = betSetting;
            viewModel.Games = GetAgCasinoGroupBettings(custId, licenseeName, betSetting.CurrencyId);
            if (betSetting.CheckedGroups.Contains(betSetting.AgGroup) == false)
            {
                betSetting.CheckedGroups = betSetting.CheckedGroups.Concat(new string[] { betSetting.AgGroup })
                    .OrderBy(x => x).ToArray();
            }

            viewModel.IsEnabled = betSetting.IsEnabled;
            return viewModel;
        }

        /// <summary>
        /// Get Ag Casino Group Betting Limit.
        /// </summary>
        /// <param name="custId">Customer id.</param>
        /// <param name="siteName">Site name.</param>
        /// <param name="currency">Currency id.</param>
        /// <returns>Ag Casino Group Betting Limit.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes",
            Justification = "Reviewed. Suppression is OK here.")]
        public IList<AgCasinoGame> GetAgCasinoGroupBettings(int custId, string siteName, int currency)
        {
            if (!HasPermissionOnData(custId))
            {
                return new List<AgCasinoGame>();
            }

            var uri = new Uri(AppSettings.GetValue("AGCasinoApiUrl"));
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

        public IResultInfo UpdateAgCasinoStatus(int userId, int custId, string language, string ip, bool isEnabled)
        {
            if (!HasPermissionOnData(custId))
            {
                return ErrorResult(ViewResources.UpdateSpecialLevelMsg);
            }

            return _betSettingDataAccess.UpdateAgCasinoStatus(userId, custId, language, ip, isEnabled);
        }

        #endregion AgCasino

        /// <summary>
        /// Enable or disable live casino.
        /// </summary>
        /// <param name="userId">The user id.</param>
        /// <param name="custId">The customer id.</param>
        /// <param name="status">Status value: 0 - enable, 1 - disable.</param>
        /// <param name="language">The language code.</param>
        /// <param name="ip">The host address.</param>
        /// <returns>The result info.</returns>
        public IResultInfo UpdateLiveCasinoStatus(int userId, int custId, int status, string language, string ip)
        {
            if (!HasPermissionOnData(custId))
            {
                return ErrorResult(ViewResources.UpdateSpecialLevelMsg);
            }

            var customerInfo = _customerService.GetCustomerInfo(custId);

            if (customerInfo.SyncLiveCasino)
            {
                return _betSettingDataAccess.UpdateLiveCasinoStatus(userId, custId, status, language, ip);
            }

            return new ResultInfo { Code = 1, Message = ViewResources.LCCasinoUpdateSettingUnAvailableMsg };
        }

        /// <summary>
        /// Update RNG Casino status.
        /// </summary>
        /// <param name="userId">User ID value.</param>
        /// <param name="custId">Customer ID value.</param>
        /// <param name="status">Status value: 0 - enable, 1 - disable.</param>
        /// <param name="language">The language value.</param>
        /// <param name="ip">The IP value.</param>
        /// <param name="username">The username value.</param>
        /// <returns>The result value.</returns>
        public IResultInfo UpdateRngCasinoStatus(int userId, int custId, int status, string language, string ip, string username)
        {
            if (!HasPermissionOnData(custId))
            {
                return ErrorResult(ViewResources.UpdateSpecialLevelMsg);
            }

            var customerInfo = _customerService.GetCustomerInfo(custId);
            if (customerInfo.SyncCasino && !AppSettings.IsDisableRngCasino)
            {
                if (!_customer.UpdateCustEnableCasino(custId, status == 0, false, ip, userId, username, 4))
                {
                    return ErrorResult(ViewResources.UpdateUnSuccess);
                }

                return _betSettingDataAccess.UpdateRngCasinoStatus(userId, custId, status, language, ip);
            }

            return new ResultInfo { Code = 1, Message = ViewResources.RNGCasinoUpdateSettingUnAvailableMsg };
        }

        /// <summary>
        /// Get SportsBook bet setting.
        /// </summary>
        /// <param name="custId">The customer id.</param>
        /// <returns>The  bet setting list.</returns>
        public BetSettingViewModel GetSportBookViewModel(int custId)
        {
            var esportSetting = EsportSettingType.NoneOffering;
            var betSettings = HasPermissionOnData(custId)
                        ? _betSettingDataAccess.GetSportBookBetSetting(custId, ref esportSetting)
                        : new List<BetSetting>();

            return new BetSettingViewModel
            {
                MainData = betSettings,
                EsportSetting = esportSetting
            };
        }

        /// <summary>
        ///     Get racing bet setting.
        /// </summary>
        /// <param name="custId">The customer id.</param>
        /// <param name="isAllowSaToreCurrency">Is allowed sa-tore currency.</param>
        /// <returns>The bet setting list.</returns>
        public BetSettingViewModel GetRacingViewModel(int custId, bool isAllowSaToreCurrency)
        {
            var betSettings = HasPermissionOnData(custId)
                        ? _betSettingDataAccess.GetRacingBetSetting(custId, isAllowSaToreCurrency)
                        : new List<BetSetting>();
            return new BetSettingViewModel
            {
                MainData = betSettings
            };
        }

        /// <summary>
        ///     Get virtual sports bet setting.
        /// </summary>
        /// <param name="custId">The customer id.</param>
        /// <returns>The bet setting list.</returns>
        public BetSettingViewModel GetVirtualSportsViewModel(int custId)
        {
            var betSettings = HasPermissionOnData(custId)
                        ? _betSettingDataAccess.GetVirtualSportsBetSetting(custId)
                        : new List<BetSetting>();
            return new BetSettingViewModel
            {
                MainData = betSettings
            };
        }

        /// <summary>
        ///     Get live casino bet setting.
        /// </summary>
        /// <param name="custId">The customer id.</param>
        /// <param name="language">Language of licensee.</param>
        /// <returns>The bet setting of live casino list.</returns>
        public LiveCasinoBetSettingViewModel GetLiveCasinoViewModel(int custId, string language)
        {
            var betSettings = HasPermissionOnData(custId)
                            ? _betSettingDataAccess.GetLiveCasinoBetSetting(custId, language)
                            : new List<LiveCasinoBetSetting>();

            var resultList = new List<LiveCasinoBetSettingGame>();

            var gameTypeIdList = betSettings.Select(x => x.GameTypeId).Distinct().ToList();

            foreach (var gameTypeId in gameTypeIdList)
            {
                var gameTypeName =
                    betSettings.Where(x => x.GameTypeId == gameTypeId).Select(i => i.GameType).FirstOrDefault();
                var bettingLimitList =
                    betSettings.Where(x => x.GameTypeId == gameTypeId).Select(i => new LiveCasinoBetSetting
                    {
                        StakeLevelId = i.StakeLevelId,
                        StakeLevelName = i.StakeLevelName,
                        IsCheck = i.IsCheck
                    }).ToList();

                resultList.Add(new LiveCasinoBetSettingGame
                {
                    GameTypeName = gameTypeName,
                    GameTypeId = gameTypeId,
                    BettingLimitList = bettingLimitList
                });
            }

            return new LiveCasinoBetSettingViewModel
            {
                LiveCasinoData = resultList
            };
        }

        public BetSettingViewModel GetKenoViewModel(int custId)
        {
            var betSettings = HasPermissionOnData(custId)
                        ? _betSettingDataAccess.GetKenoBetSettings(custId)
                        : new List<BetSetting>();

            return new BetSettingViewModel()
            {
                MainData = betSettings
            };
        }

        /// <summary>
        ///  Update SportsBook bet setting.
        /// </summary>
        /// <param name="setting">The common setting.</param>
        /// <param name="group">Bet setting group.</param>
        /// <returns>The information of result.</returns>
        public IResultInfo UpdateAgCasinoBetSetting(CommonBetSetting setting, string group)
        {
            if (setting == null)
            {
                return ErrorResult("Invalid parameter. AG Casino is null");
            }

            var agcasinoSetting = _betSettingDataAccess.GetAgCasinoSetting(setting.CustId);

            if (!agcasinoSetting.IsEnabled || !HasPermissionOnData(setting.CustId))
            {
                return ErrorResult(ViewResources.UpdateSpecialLevelMsg);
            }

            return _betSettingDataAccess.UpdateAgCasinoBetSetting(setting, group);
        }

        /// <summary>
        ///  Update SportsBook bet setting.
        /// </summary>
        /// <param name="setting">The sport-book setting.</param>
        /// <param name="hashParam">The metal model.</param>
        /// <returns>The information of result.</returns>
        public IResultInfo UpdateSportsBookBetSetting(CommonBetSetting setting, IDictionary<string, object> hashParam)
        {
            if (setting == null)
            {
                return ErrorResult("Invalid parameter. sportsBook is null");
            }

            if (!HasPermissionOnData(setting.CustId))
            {
                return ErrorResult(ViewResources.UpdateSpecialLevelMsg);
            }

            return _betSettingDataAccess.UpdateSportsBookBetSetting(setting, hashParam);
        }

        public IResultInfo UpdateEsportStatus(CommonBetSetting setting, bool isDisabled)
        {
            if (setting == null)
            {
                return ErrorResult("Invalid parameter. sportsBook is null");
            }

            return _betSettingDataAccess.UpdateEsportStatus(setting, isDisabled);
        }

        /// <summary>
        /// Update racing bet setting.
        /// </summary>
        /// <param name="racingParam">Dictionary of racing parameter.</param>
        /// <returns>The  bet setting racing.</returns>
        public IDictionary<string, string> UpdateRacingBetSetting(Dictionary<string, RacingBetSetting> racingParam)
        {
            if (racingParam == null || racingParam.Count == 0)
            {
                throw new ServiceException("Invalid parameter. racingParam is null", "BetSetting");
            }

            var custId = racingParam.First().Value.CustId;
            if (!HasPermissionOnData(custId))
            {
                throw new ServiceException(ViewResources.UpdateSpecialLevelMsg, "BetSetting");
            }

            return _betSettingDataAccess.UpdateRacingBetSetting(racingParam);
        }

        /// <summary>
        /// Update virtual sports bet setting.
        /// </summary>
        /// <param name="custId">The customer id.</param>
        /// <param name="userId">The user id.</param>
        /// <param name="language">The language id.</param>
        /// <param name="ip">The IP address.</param>
        /// <param name="betSettings">The virtual sport bet setting.</param>
        /// <returns>The result.</returns>
        public IResultInfo UpdateVirtualSportsBetSetting(int custId, int userId, string language, string ip, IList<VSBetSetting> betSettings)
        {
            const string XMLPattern = "<settings>{0}</settings>";
            const string XMLElement = "<setting sporttype=\"{0}\" minbet=\"{1}\" maxbet=\"{2}\" maxpermatch=\"{3}\" ></setting>";
            if (!HasPermissionOnData(custId))
            {
                return ErrorResult(ViewResources.UpdateSpecialLevelMsg);
            }

            var xmlElementBuilder = new StringBuilder();

            foreach (var setting in betSettings)
            {
                xmlElementBuilder.Append(string.Format(XMLElement, setting.SportId, setting.MinBet, setting.MaxBet, setting.MaxPerMatch));
            }

            var xmlparam = string.Format(XMLPattern, xmlElementBuilder.ToString());

            return _betSettingDataAccess.UpdateVirtualSportsBetSetting(
                custId,
                userId,
                language,
                ip,
                xmlparam);
        }

        /// <summary>
        /// Update live casino bet setting.
        /// </summary>
        /// <param name="userId">The user Id.</param>
        /// <param name="custId">The customer id.</param>
        /// <param name="metaModel">The metal model.</param>
        /// <param name="ip">The IP address.</param>
        /// <param name="language">The language code.</param>
        /// <returns>The result.</returns>
        public IResultInfo UpdateLiveCasinoBetSetting(int userId, int custId, List<LiveCasinoBetSetting> metaModel, string ip, string language)
        {
            var customerInfo = _customerService.GetCustomerInfo(custId);

            if (customerInfo.DisableLiveCasino || !HasPermissionOnData(custId))
            {
                return ErrorResult(ViewResources.UpdateSpecialLevelMsg);
            }

            var errCode = string.Empty;
            var errMsg = string.Empty;

            foreach (var curParam in metaModel)
            {
                var result = _betSettingDataAccess.UpdateLiveCasinoBetSetting(userId, custId, curParam, ip, language);

                if (result.Code == 0)
                {
                    continue;
                }

                errCode += result.Code + ", ";
                errMsg += result.Message + ", ";
            }

            return errCode.Length == 0
                ? new ResultInfo { Code = 0, Message = string.Empty }
                : new ResultInfo { Code = -1, Message = errMsg };
        }

        /// <summary>
        /// Update RNG casino setting.
        /// </summary>
        /// <param name="metaModel">The metal mode.</param>
        /// <returns>The result.</returns>
        public IResultInfo UpdateRngCasinoSetting(RngCasinoBetSettingMetaModel metaModel)
        {
            var customerInfo = _customerService.GetCustomerInfo(metaModel.CustId);

            if (customerInfo.DisableCasino || !HasPermissionOnData(metaModel.CustId))
            {
                return ErrorResult(ViewResources.UpdateSpecialLevelMsg);
            }

            var result = false;

            if (customerInfo.SyncCasino)
            {
                if (!AppSettings.IsDisableRngCasino)
                {
                    result = _casinoSetting.UpdateRngCasinoSetting(metaModel.TableParam, metaModel.LicenseeName);

                    return result ?
                        new ResultInfo { Code = 0, Message = ViewResources.MessageSuccess }
                        : new ResultInfo { Code = 1, Message = ViewResources.Fail };
                }
                else
                {
                    return new ResultInfo { Code = 1, Message = ViewResources.RNGCasinoUpdateSettingUnAvailableMsg };
                }
            }

            return new ResultInfo { Code = 1, Message = ViewResources.RNGCasinoUpdateSettingUnAvailableMsg };
        }

        /// <summary>
        /// Update Keno bet setting.
        /// </summary>
        /// <param name="custId">The customer id.</param>
        /// <param name="userId">The user id.</param>
        /// <param name="language">The language id.</param>
        /// <param name="ip">The IP address.</param>
        /// <param name="setting">The bet setting.</param>
        /// <returns>The result.</returns>
        public IResultInfo UpdateKenoBetSetting(int custId, int userId, string language, string ip, KenoBetSetting setting)
        {
            const string XMLPattern = "<settings>{0}</settings>";
            const string XMLElement = "<setting GameTypeID=\"{0}\" minbet=\"{1}\" maxbet=\"{2}\" maxpermatch=\"{3}\" ></setting>";
            if (!HasPermissionOnData(custId))
            {
                return ErrorResult(ViewResources.UpdateSpecialLevelMsg);
            }

            var xmlElementBuilder = new StringBuilder();

            xmlElementBuilder.Append(string.Format(XMLElement, setting.GameTypeID, setting.MinBet, setting.MaxBet, setting.MaxPerMatch));

            var xmlparam = string.Format(XMLPattern, xmlElementBuilder.ToString());

            return _betSettingDataAccess.UpdateKenoBetSetting(
                custId,
                userId,
                language,
                ip,
                xmlparam);
        }

        #region Private Method

        /// <summary>
        /// Get Gold Deluxe Group Betting Limit.
        /// </summary>
        /// <param name="custId">Customer id.</param>
        /// <param name="siteName">Site name.</param>
        /// <param name="currency">Currency id.</param>
        /// <returns>Gold Deluxe Group Betting Limit.</returns>
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

        /// <summary>
        /// Get gold deluxe user group.
        /// </summary>
        /// <param name="siteName">Site name.</param>
        /// <param name="custId">Customer id.</param>
        /// <param name="currencyId">Currency id.</param>
        /// <returns>Group name value.</returns>
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

        #endregion
    }
}