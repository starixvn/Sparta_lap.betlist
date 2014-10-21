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

        public IList<Sportbook> GetMatches(ResultMetalModel metalModel)
        {
            return this._dataAccess.GetMatches<Sportbook>(metalModel);
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

        public IList<Sportbook> GetPopUpMatch(int matchId, int licenseeId, int userId)
        {
            return this._dataAccess.GetPopUpMatches<Sportbook>(matchId, -1, licenseeId, userId);
        }

        public IList<LiveCasino> GetPopUpLiveCasinoMatch(int matchId, int licenseeId, int userId)
        {
            return this._dataAccess.GetPopUpMatches<LiveCasino>(matchId, -1, licenseeId, userId);
        }

        /// <summary>
        /// Get racing data for popup page.
        /// </summary>
        /// <param name="matchId">The match id.</param>
        /// <param name="raceNo">The reference number.</param>
        /// <param name="licenseeId">The licensee id.</param>
        /// <param name="userId">The user id.</param>
        /// <returns>The racing data list.</returns>
        public IList<Racing> GetPopUpRacingMatch(int matchId, int raceNo, int licenseeId, int userId)
        {
            return this._dataAccess.GetPopUpMatches<Racing>(matchId, raceNo, licenseeId, userId);
        }

        /// <summary>
        /// Get parlay match for pop up page.
        /// </summary>
        /// <param name="refNo">The reference no.</param>
        /// <returns>The parlay data list.</returns>        
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