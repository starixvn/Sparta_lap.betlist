// -----------------------------------------------------------------------
// <copyright file="RacingService.cs" company="Nexcel">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Service.Implement.TotalBets
{
    using DataAccess.Implement.TotalBets;
    using Interface.TotalBets;
    using Models.DTO.TotalBets;
    using Models.MetaModel.TotalBets;

    /// <summary>
    /// Racing service class.
    /// </summary>
    public class RacingService : IRacingService
    {
        #region Field

        protected readonly RacingDataAccess RacingDataAccess;

        #endregion Field

        #region Constructor

        public RacingService()
        {
            RacingDataAccess = new RacingDataAccess();
        }

        #endregion Constructor

        public Racing GetRacingDataList(RacingMetaModel metaModel)
        {
            return RacingDataAccess.GetRacingDataList(metaModel);
        }
    }
}