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

        protected readonly HomeDrawAwayNoBetDataAccess HomeDrawAwayNoBetDataAccess;

        #endregion Fields

        #region Constructor

        public HomeDrawAwayNoBetService()
        {
            HomeDrawAwayNoBetDataAccess = new HomeDrawAwayNoBetDataAccess();
        }

        #endregion Constructor

        public IList<HomeDrawAwayNoBet> GetHomeDrawAwayNoBetDataList(HomeDrawAwayNoBetMetaModel metaModel)
        {
            return HomeDrawAwayNoBetDataAccess.GetHomeDrawAwayNoBetDataList(metaModel);
        }
    }
}