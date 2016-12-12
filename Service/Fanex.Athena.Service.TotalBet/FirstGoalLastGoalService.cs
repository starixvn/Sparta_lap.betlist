// -----------------------------------------------------------------------
// <copyright file="FirstGoalLastGoalService.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Service.TotalBet
{
    using System.Collections.Generic;
    using DataAccess.TotalBet;
    using DataAccess.TotalBet.Interfaces;
    using Model.TotalBet.DTO;
    using Model.TotalBet.MetaModel;
    using Service.TotalBet.Interfaces;

    public class FirstGoalLastGoalService : IFirstGoalLastGoalService
    {
        private readonly IFirstGoalLastGoalDataAccess _firstGoalLastGoalDataAccess;

        public FirstGoalLastGoalService()
        {
            _firstGoalLastGoalDataAccess = new FirstGoalLastGoalDataAccess();
        }

        public FirstGoalLastGoalService(IFirstGoalLastGoalDataAccess firstGoalLastGoalDataAccess)
        {
            _firstGoalLastGoalDataAccess = firstGoalLastGoalDataAccess;
        }

        public IList<FirstGoalLastGoal> GetFirstGoalLastGoalDataList(FirstGoalLastGoalMetaModel metaModel, out FirstGoalLastGoal totalData)
        {
            return _firstGoalLastGoalDataAccess.GetFirstGoalLastGoalDataList(metaModel, out totalData);
        }
    }
}