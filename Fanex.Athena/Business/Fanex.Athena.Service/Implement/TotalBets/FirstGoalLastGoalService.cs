// -----------------------------------------------------------------------
// <copyright file="FirstGoalLastGoalService.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Service.Implement.TotalBets
{
    using System.Collections.Generic;
    using DataAccess.Implement.TotalBets;
    using Models.DTO.TotalBets;
    using Models.MetaModel.TotalBets;
    using Service.Interface.TotalBets;

    /// <summary>
    /// The first goal and last goal service class.
    /// </summary>
    public class FirstGoalLastGoalService : IFirstGoalLastGoalService
    {
        #region Fields

        protected readonly FirstGoalLastGoalDataAccess FirstGoalLastGoalDataAccess;

        #endregion Fields

        #region Constructor

        public FirstGoalLastGoalService()
        {
            FirstGoalLastGoalDataAccess = new FirstGoalLastGoalDataAccess();
        }

        #endregion Constructor

        public IList<FirstGoalLastGoal> GetFirstGoalLastGoalDataList(FirstGoalLastGoalMetaModel metaModel)
        {
            return FirstGoalLastGoalDataAccess.GetFirstGoalLastGoalDataList(metaModel);
        }
    }
}