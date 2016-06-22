// -----------------------------------------------------------------------
// <copyright file="TotalGoalService.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Service.TotalBet
{
    using System.Collections.Generic;
    using DataAccess.TotalBet;
    using Interfaces;
    using Model.TotalBet.DTO;
    using Model.TotalBet.MetaModel;

    /// <summary>
    /// Total Goal Service class.
    /// </summary>
    public class TotalGoalService : ITotalGoalService
    {
        #region Fields

        private readonly TotalGoalDataAccess _totalGoalDataAccess;

        #endregion Fields

        #region Constructor

        public TotalGoalService()
        {
            _totalGoalDataAccess = new TotalGoalDataAccess();
        }

        #endregion Constructor

        public IList<TotalGoal> GetTotalGoalDataList(TotalGoalMetaModel metaModel, out TotalGoal totalData)
        {
            return _totalGoalDataAccess.GetTotalGoalDataList(metaModel, out totalData);
        }
    }
}