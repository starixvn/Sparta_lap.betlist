namespace Fanex.Athena.Result.Service
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Threading.Tasks;
    using Common.Model.DTO;
    using DataAccess;
    using Fanex.Result.Core;
    using Fanex.Result.Core.Base;
    using Fanex.Result.Core.Happy5.DTO;
    using Fanex.Result.Core.NumberGame.DTO;
    using Fanex.Result.Core.Racing.DTO;
    using Fanex.Result.Core.Sportsbook.DTO;
    using Interfaces;
    using Model.MetaModel;

    [CLSCompliant(false)]
    public class ResultService : IResultService
    {
        private readonly IResultDataAccess _dataAccess;

        private readonly IResultNumberGameDataAccess _numberGameDataAccess;

        public ResultService(IResultDataAccess dataAccess, IResultNumberGameDataAccess numberGameDataAccess)
        {
            this._dataAccess = dataAccess;
            _numberGameDataAccess = numberGameDataAccess;
        }

        public IList<Outright> GetOutRight(ResultMetaModel metal)
        {
            return this._dataAccess.GetOutRight(metal);
        }

        public async Task<IList<Sport>> GetSports(DateTime dateValue, int licenseeId)
        {
            return await this._dataAccess.GetSports(dateValue, licenseeId).ConfigureAwait(false);
        }

        public async Task<IList<League>> GetLeagues(DateTime dateValue, int sportId)
        {
            return await this._dataAccess.GetLeagues(dateValue, sportId).ConfigureAwait(false);
        }

        #region ResultEngine wrapping

        public MemoryStream ExcelParser<T>(IList<T> dataList, ResultSetting setting)
        {
            return ResultEngine.ExcelParser<T>(dataList, setting);
        }

        public string HtmlGameDetailParser(dynamic data, ResultSetting setting)
        {
            return ResultEngine.HtmlGameDetailParser(data, setting);
        }

        public string HtmlParser<T>(IList<T> dataList, ResultSetting setting)
        {
            return ResultEngine.HtmlParser(dataList, setting);
        }

        #endregion ResultEngine wrapping

        #region Report

        public async Task<IList<Sportsbook>> GetMatches(ResultMetaModel metalModel)
        {
            return await this._dataAccess.GetMatches<Sportsbook>(metalModel).ConfigureAwait(false);
        }

        public IList<NumberGame> GetNumberGameMatches(ResultMetaModel metalModel)
        {
            return this._numberGameDataAccess.GetNumberGameMatches(metalModel);
        }

        public IList<Happy5> GetHappy5Matches(ResultMetaModel metalModel)
        {
            return this._numberGameDataAccess.GetHappy5Matches(metalModel);
        }

        public IList<Racing> GetVirtualSportMatches(ResultMetaModel metalModel)
        {
            return this._dataAccess.GetVirtualSportMatches(metalModel);
        }

        #endregion Report

        #region Popup

        [System.CLSCompliant(false)]
        public Sportsbook GetSportsbookGameDetail(int matchId)
        {
            return this._dataAccess.GetSportsbookGameDetail(matchId);
        }

        public IList<Sportsbook> GetPopUpMatch(ResultPopupMetaModel metalModel)
        {
            metalModel.RaceNo = null;
            return this._dataAccess.GetPopUpMatches<Sportsbook>(metalModel);
        }

        public IList<Racing> GetPopUpVirtualSportMatch(ResultPopupMetaModel metalModel)
        {
            return this._dataAccess.GetPopUpMatches<Racing>(metalModel);
        }

        public IList<Parlay> GetPopUpParlayMatch(long refNo)
        {
            return this._dataAccess.GetPopUpParalayMatch(refNo);
        }

        public IList<NumberGame> GetPopUpNumberGameMatchs(int matchId)
        {
            return this._numberGameDataAccess.GetPopUpNumberGameMatch(matchId);
        }

        public IList<Happy5> GetPopUpHappy5Matches(int matchId)
        {
            return this._numberGameDataAccess.GetPopUpHappy5Match(matchId);
        }

        #endregion Popup
    }
}