// -----------------------------------------------------------------------
// <copyright file="FirstGoalLastGoalService.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Service.TotalBet
{
    using System.Collections.Generic;
    using DataAccess.TotalBet;
    using Model.TotalBet.DTO;
    using Model.TotalBet.MetaModel;
    using Service.TotalBet.Interfaces;

    /// <summary>
    /// The first goal and last goal service class.
    /// </summary>
    public class FirstGoalLastGoalService : IFirstGoalLastGoalService
    {
        #region Fields

        private readonly FirstGoalLastGoalDataAccess _firstGoalLastGoalDataAccess;

        #endregion Fields

        #region Constructor

        public FirstGoalLastGoalService()
        {
            _firstGoalLastGoalDataAccess = new FirstGoalLastGoalDataAccess();
        }

        #endregion Constructor

        public IList<FirstGoalLastGoal> GetFirstGoalLastGoalDataList(FirstGoalLastGoalMetaModel metaModel, out FirstGoalLastGoal totalData)
        {
            return _firstGoalLastGoalDataAccess.GetFirstGoalLastGoalDataList(metaModel, out totalData);
        }
    }
}