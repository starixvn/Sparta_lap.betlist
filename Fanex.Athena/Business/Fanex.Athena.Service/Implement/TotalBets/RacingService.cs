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

        private readonly RacingDataAccess _racingDataAccess;

        #endregion Field

        #region Constructor

        public RacingService()
        {
            _racingDataAccess = new RacingDataAccess();
        }

        #endregion Constructor

        public Racing GetRacingDataList(RacingMetaModel metaModel)
        {
            return _racingDataAccess.GetRacingDataList(metaModel);
        }
    }
}