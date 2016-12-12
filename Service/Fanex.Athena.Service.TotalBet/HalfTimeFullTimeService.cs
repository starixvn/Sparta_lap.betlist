// -----------------------------------------------------------------------
// <copyright file="HalfTimeFullTimeService.cs" company="">
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

    public class HalfTimeFullTimeService : IHalfTimeFullTimeService
    {
        private readonly IHalfTimeFullTimeDataAccess _halfTimeFullTimeDataAccess;

        public HalfTimeFullTimeService()
        {
            _halfTimeFullTimeDataAccess = new HalfTimeFullTimeDataAccess();
        }

        public HalfTimeFullTimeService(IHalfTimeFullTimeDataAccess halfTimeFullTimeDataAccess)
        {
            _halfTimeFullTimeDataAccess = halfTimeFullTimeDataAccess;
        }

        public IList<HalfTimeFullTime> GetHalfTimeFullTimeDataList(HalfTimeFullTimeMetaModel metaModel, out HalfTimeFullTime totalData)
        {
            return _halfTimeFullTimeDataAccess.GetHalfTimeFullTimeDataList(metaModel, out totalData);
        }
    }
}