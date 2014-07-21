// -----------------------------------------------------------------------
// <copyright file="HalfTimeFullTimeService.cs" company="Nexcel">
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

        public IList<HalfTimeFullTime> GetHalfTimeFullTimeDataList(HalfTimeFullTimeMetaModel metaModel)
        {
            return _halfTimeFullTimeDataAccess.GetHalfTimeFullTimeDataList(metaModel);
        }
    }
}