// -----------------------------------------------------------------------
// <copyright file="TotalGoalService.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Service.TotalBet
{
    using System.Collections.Generic;
    using DataAccess.TotalBet;
    using DataAccess.TotalBet.Interfaces;
    using Interfaces;
    using Model.TotalBet.DTO;
    using Model.TotalBet.MetaModel;

    public class TotalGoalService : ITotalGoalService
    {
        private readonly ITotalGoalDataAccess _totalGoalDataAccess;

        public TotalGoalService()
        {
            _totalGoalDataAccess = new TotalGoalDataAccess();
        }

        public TotalGoalService(ITotalGoalDataAccess totalGoalDataAccess)
        {
            _totalGoalDataAccess = totalGoalDataAccess;
        }

        public IList<TotalGoal> GetTotalGoalDataList(TotalGoalMetaModel metaModel, out TotalGoal totalData)
        {
            return _totalGoalDataAccess.GetTotalGoalDataList(metaModel, out totalData);
        }
    }
}