// -----------------------------------------------------------------------
// <copyright file="HandicapOverUnderService.cs" company="">
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

    public class HandicapOverUnderService : IHandicapOverUnderService
    {
        private readonly IHandicapOverUnderDataAccess _handicapOverUnderDataAccess;

        public HandicapOverUnderService()
        {
            _handicapOverUnderDataAccess = new HandicapOverUnderDataAccess();
        }

        public HandicapOverUnderService(IHandicapOverUnderDataAccess handicapOverUnderDataAccess)
        {
            _handicapOverUnderDataAccess = handicapOverUnderDataAccess;
        }

        public IList<HandicapOverUnder> GetHandicapOverUnderDataList(HandicapOverUnderMetaModel metaModel, out HandicapOverUnder totalData)
        {
            return _handicapOverUnderDataAccess.GetHandicapOverUnderDataList(metaModel, out totalData);
        }
    }
}