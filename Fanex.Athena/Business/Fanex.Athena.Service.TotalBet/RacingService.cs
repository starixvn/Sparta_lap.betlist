// -----------------------------------------------------------------------
// <copyright file="RacingService.cs" company="Nexcel">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Service.TotalBet
{
    using DataAccess.TotalBet;
    using Interfaces;
    using Model.TotalBet.DTO;
    using Model.TotalBet.MetaModel;

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