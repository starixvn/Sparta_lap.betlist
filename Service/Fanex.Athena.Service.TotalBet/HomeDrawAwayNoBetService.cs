// -----------------------------------------------------------------------
// <copyright file="HomeDrawAwayNoBetService.cs" company="Nexcel">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Service.TotalBet
{
    using System.Collections.Generic;
    using DataAccess.TotalBet;
    using Interfaces;
    using Model.TotalBet.DTO;
    using Model.TotalBet.MetaModel;

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