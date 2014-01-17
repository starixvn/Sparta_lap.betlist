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

        protected readonly HalfTimeFullTimeDataAccess HalfTimeFullTimeDataAccess;

        #endregion Fields

        #region Constructor

        public HalfTimeFullTimeService()
        {
            HalfTimeFullTimeDataAccess = new HalfTimeFullTimeDataAccess();
        }

        #endregion Constructor

        public IList<HalfTimeFullTime> GetHalfTimeFullTimeDataList(HalfTimeFullTimeMetaModel metaModel)
        {
            return HalfTimeFullTimeDataAccess.GetHalfTimeFullTimeDataList(metaModel);
        }
    }
}