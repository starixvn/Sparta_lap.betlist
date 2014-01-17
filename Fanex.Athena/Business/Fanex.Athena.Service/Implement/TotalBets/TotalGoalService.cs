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

        protected readonly TotalGoalDataAccess TotalGoalDataAccess;

        #endregion Fields

        #region Constructor

        public TotalGoalService()
        {
            TotalGoalDataAccess = new TotalGoalDataAccess();
        }

        #endregion Constructor

        public IList<TotalGoal> GetTotalGoalDataList(TotalGoalMetaModel metaModel)
        {
            return TotalGoalDataAccess.GetTotalGoalDataList(metaModel);
        }
    }
}