// -----------------------------------------------------------------------
// <copyright file="ResultService.cs" company="Nexcel">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Service.Implement.Report
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using Fanex.Athena.DataAccess.Implement.Report;
    using Fanex.Athena.DataAccess.Interface.Report;
    using Fanex.Athena.Models.DTO.Common;
    using Fanex.Athena.Models.MetaModel.Report;
    using Fanex.Athena.Service.Interface.Report;
    using Fanex.Result.Core.DTO;

    /// <summary>
    /// Result service class.
    /// </summary>
    public class ResultService : IResultService
    {
        private readonly IResultDataAccess _dataAccess;

        public ResultService()
        {
            this._dataAccess = new ResultDataAccess();
        }

        /// <summary>
        /// Get out right data.
        /// </summary>
        /// <param name="metal">The metal model.</param>
        /// <returns>The out right data list.</returns>
        public IList<OutRight> GetOutRight(Models.MetaModel.Report.ResultMetalModel metal)
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

        public Sportsbook GetSportsbookGameDetail(int matchId)
        {
            return this._dataAccess.GetSportsbookGameDetail(matchId);
        }

        public IList<Sportsbook> GetPopUpMatch(int matchId, int licenseeId, int userId, int bettypeId, long? betId)
        {
            return this._dataAccess.GetPopUpMatches<Sportsbook>(matchId, -1, licenseeId, userId, bettypeId, betId);
        }

        public IList<LiveCasino> GetPopUpLiveCasinoMatch(int matchId, int licenseeId, int userId, int bettypeId, long? betId)
        {
            return this._dataAccess.GetPopUpMatches<LiveCasino>(matchId, -1, licenseeId, userId, bettypeId, betId);
        }

        public IList<Racing> GetPopUpRacingMatch(int matchId, int raceNo, int licenseeId, int userId, int bettypeId, long? betId)
        {
            return this._dataAccess.GetPopUpMatches<Racing>(matchId, raceNo, licenseeId, userId, bettypeId, betId);
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