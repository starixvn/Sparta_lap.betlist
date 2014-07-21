// -----------------------------------------------------------------------
// <copyright file="HomeDrawAwayNoBetService.cs" company="Nexcel">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Service.Implement.TotalBets
{
    using System.Collections.Generic;
    using DataAccess.Implement.TotalBets;
    using Interface.TotalBets;
    using Models.DTO.TotalBets;
    using Models.MetaModel.TotalBets;

    /// <summary>
    /// TODO: update class.
    /// </summary>
    public class HomeDrawAwayNoBetService : IHomeDrawAwayNoBetService
    {
        #region Fields

        private readonly HomeDrawAwayNoBetDataAccess _homeDrawAwayNoBetDataAccess;

        #endregion Fields

        #region Constructor

        public HomeDrawAwayNoBetService()
        {
            _homeDrawAwayNoBetDataAccess = new HomeDrawAwayNoBetDataAccess();
        }

        #endregion Constructor

        public IList<HomeDrawAwayNoBet> GetHomeDrawAwayNoBetDataList(HomeDrawAwayNoBetMetaModel metaModel)
        {
            return _homeDrawAwayNoBetDataAccess.GetHomeDrawAwayNoBetDataList(metaModel);
        }
    }
}