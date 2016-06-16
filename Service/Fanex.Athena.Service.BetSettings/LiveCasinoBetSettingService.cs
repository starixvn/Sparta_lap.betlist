// -----------------------------------------------------------------------
// <copyright file="LiveCasinoBetSettingService.cs" company="Nexcel">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Service.BetSettings
{
    using System.Collections.Generic;
    using System.Linq;
    using DataAccess.BetSettings;
    using DataAccess.BetSettings.Interfaces;
    using Model.BetSettings.DTO;
    using Model.BetSettings.ViewModel;
    using Model.Core.DTO.Common;
    using Resources;

    public class LiveCasinoBetSettingService : BetSettingBaseService, ILiveCasinoBetSettingService
    {
        private readonly ILiveCasinoBetSettingDataAccess _betSettingDataAccess;

        public LiveCasinoBetSettingService()
        {
            _betSettingDataAccess = new LiveCasinoBetSettingDataAccess();
        }

        public IResultInfo UpdateStatus(int licenseeId, int userId, int custId, int status, string language, string ip, bool isSyncLiveCasino)
        {
            if (!HasPermissionOnData(licenseeId, custId))
            {
                return ErrorResult(ViewResources.UpdateSpecialLevelMsg);
            }

            if (isSyncLiveCasino)
            {
                return _betSettingDataAccess.UpdateStatus(userId, custId, status, language, ip);
            }

            return new ResultInfo { Code = 1, Message = ViewResources.LCCasinoUpdateSettingUnAvailableMsg };
        }

        public LiveCasinoBetSettingViewModel GetViewModel(int licenseeId, int custId, string language)
        {
            var betSettings = HasPermissionOnData(licenseeId, custId)
                            ? _betSettingDataAccess.GetBetSetting(custId, language)
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

        public IResultInfo UpdateBetSetting(
            int licenseeId, 
            int userId, 
            int custId, 
            IList<LiveCasinoBetSetting> metaModel, 
            string ip, 
            string language, 
            bool isDisableLiveCasino)
        {
            if (isDisableLiveCasino || !HasPermissionOnData(licenseeId, custId))
            {
                return ErrorResult(ViewResources.UpdateSpecialLevelMsg);
            }

            var errCode = string.Empty;
            var errMsg = string.Empty;

            foreach (var curParam in metaModel)
            {
                var result = _betSettingDataAccess.UpdateBetSetting(userId, custId, curParam, ip, language);

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
    }
}