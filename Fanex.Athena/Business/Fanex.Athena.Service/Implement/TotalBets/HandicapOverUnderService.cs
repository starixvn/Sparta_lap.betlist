// -----------------------------------------------------------------------
// <copyright file="HandicapOverUnderService.cs" company="Nexcel">
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
    /// TODO: Update summary.
    /// </summary>
    public class HandicapOverUnderService : IHandicapOverUnderService
    {
        #region Fields

        protected readonly HandicapOverUnderDataAccess HandicapOverUnderDataAccess;

        #endregion Fields

        #region Constructor

        public HandicapOverUnderService()
        {
            HandicapOverUnderDataAccess = new HandicapOverUnderDataAccess();
        }

        #endregion Constructor

        public IList<HandicapOverUnder> GetHandicapOverUnderDataList(HandicapOverUnderMetaModel metaModel)
        {
            var dataList = HandicapOverUnderDataAccess.GetHandicapOverUnderDataList(metaModel);
            return dataList;
        }
    }
}