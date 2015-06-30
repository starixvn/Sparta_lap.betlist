// -----------------------------------------------------------------------
// <copyright file="TotalGoalService.cs" company="Nexcel">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Service.Implement.TotalBets
{
    using System.Collections.Generic;
    using DataAccess.Implement.TotalBets;
    using Interface.TotalBets;
    using Models.DTO.TotalBets;
    using Models.MetaModel.TotalBets;

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