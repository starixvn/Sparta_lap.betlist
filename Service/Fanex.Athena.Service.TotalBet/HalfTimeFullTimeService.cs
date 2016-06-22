// -----------------------------------------------------------------------
// <copyright file="HalfTimeFullTimeService.cs" company="">
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
    /// Half time and full time service class.
    /// </summary>
    public class HalfTimeFullTimeService : IHalfTimeFullTimeService
    {
        #region Fields

        private readonly HalfTimeFullTimeDataAccess _halfTimeFullTimeDataAccess;

        #endregion Fields

        #region Constructor

        public HalfTimeFullTimeService()
        {
            _halfTimeFullTimeDataAccess = new HalfTimeFullTimeDataAccess();
        }

        #endregion Constructor

        public IList<HalfTimeFullTime> GetHalfTimeFullTimeDataList(HalfTimeFullTimeMetaModel metaModel, out HalfTimeFullTime totalData)
        {
            return _halfTimeFullTimeDataAccess.GetHalfTimeFullTimeDataList(metaModel, out totalData);
        }
    }
}