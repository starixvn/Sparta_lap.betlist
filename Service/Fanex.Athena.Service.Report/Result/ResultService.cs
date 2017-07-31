namespace Fanex.Athena.Service.Report.Result
{
    using System;
    using System.Collections.Generic;
    using Core.Configuration;
    using Core.Configuration.Interfaces;
    using Fanex.Athena.DataAccess.Report;
    using Fanex.Athena.DataAccess.Report.Interfaces;
    using Fanex.Athena.Model.Core.DTO.Common;
    using Fanex.Athena.Model.Report.MetaModel;
    using Fanex.Result.Core.LiveCasino.DTO;
    using Fanex.Result.Core.NumberGame.DTO;
    using Fanex.Result.Core.Racing.DTO;
    using Fanex.Result.Core.Sportsbook.DTO;
    using Service.Result.Interfaces;

    [System.CLSCompliant(false)]
    public class ResultService : IResultService
    {
        private readonly IBalanceUpService _balanceUpService;

        private readonly IResultDataAccess _dataAccess;

        public ResultService()
        {
            _balanceUpService = new BalanceUpService();
            this._dataAccess = new ResultDataAccess(_balanceUpService.BalanceUpInfo);
        }

        public IList<Outright> GetOutRight(ResultMetalModel metal)
        {
            return this._dataAccess.GetOutRight(metal);
        }

        public IList<Sport> GetSports(DateTime dateValue, int licenseeId)
        {
            return this._dataAccess.GetSports(dateValue, licenseeId);
        }

        public IList<League> GetLeagues(DateTime dateValue, int sportId)
        {
            return this._dataAccess.GetLeagues(dateValue, sportId);
        }

        #region Report

        public IList<Sportsbook> GetMatches(ResultMetalModel metalModel)
        {
            return this._dataAccess.GetMatches<Sportsbook>(metalModel);
        }

        public IList<LiveCasino> GetLiveCasinoMatches(ResultMetalModel metalModel)
        {
            return this._dataAccess.GetMatches<LiveCasino>(metalModel);
        }

        public IList<NumberGame> GetNumberGameMatches(ResultMetalModel metalModel)
        {
            return this._dataAccess.GetNumberGameMatches(metalModel);
        }

        public IList<Racing> GetRacingMatches(ResultMetalModel metalModel)
        {
            return this._dataAccess.GetRacingMatches(metalModel);
        }

        #endregion Report

        #region Popup

        [System.CLSCompliant(false)]
        public Sportsbook GetSportsbookGameDetail(int matchId)
        {
            return this._dataAccess.GetSportsbookGameDetail(matchId);
        }

        public IList<Sportsbook> GetPopUpMatch(int matchId, int bettypeId, long? betId)
        {
            return this._dataAccess.GetPopUpMatches<Sportsbook>(matchId, -1, bettypeId, betId);
        }

        public IList<LiveCasino> GetPopUpLiveCasinoMatch(int matchId, int bettypeId, long? betId)
        {
            return this._dataAccess.GetPopUpMatches<LiveCasino>(matchId, -1, bettypeId, betId);
        }

        public IList<Racing> GetPopUpRacingMatch(int matchId, int raceNo, int bettypeId, long? betId)
        {
            return this._dataAccess.GetPopUpMatches<Racing>(matchId, raceNo, bettypeId, betId);
        }

        public IList<Parlay> GetPopUpParlayMatch(long refNo)
        {
            return this._dataAccess.GetPopUpParalayMatch(refNo);
        }

        public IList<NumberGame> GetPopUpNumberGameMatchs(int matchId)
        {
            return this._dataAccess.GetPopUpNumberGameMatch(matchId);
        }

        #endregion Popup
    }
}