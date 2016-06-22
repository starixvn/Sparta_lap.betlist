// -----------------------------------------------------------------------
// <copyright file="HandicapOverUnderService.cs" company="">
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
    /// TODO: Update summary.
    /// </summary>
    public class HandicapOverUnderService : IHandicapOverUnderService
    {
        #region Fields

        private readonly HandicapOverUnderDataAccess _handicapOverUnderDataAccess;

        #endregion Fields

        #region Constructor

        public HandicapOverUnderService()
        {
            _handicapOverUnderDataAccess = new HandicapOverUnderDataAccess();
        }

        #endregion Constructor

        public IList<HandicapOverUnder> GetHandicapOverUnderDataList(HandicapOverUnderMetaModel metaModel, out HandicapOverUnder totalData)
        {
            return _handicapOverUnderDataAccess.GetHandicapOverUnderDataList(metaModel, out totalData);
        }
    }
}