// -----------------------------------------------------------------------
// <copyright file="HomeDrawAwayNoBetService.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Service.TotalBet
{
    using System.Collections.Generic;
    using DataAccess.TotalBet;
    using DataAccess.TotalBet.Interfaces;
    using Interfaces;
    using Model.TotalBet.DTO;
    using Model.TotalBet.MetaModel;

    public class HomeDrawAwayNoBetService : IHomeDrawAwayNoBetService
    {
        private readonly IHomeDrawAwayNoBetDataAccess _homeDrawAwayNoBetDataAccess;

        public HomeDrawAwayNoBetService()
        {
            _homeDrawAwayNoBetDataAccess = new HomeDrawAwayNoBetDataAccess();
        }

        public HomeDrawAwayNoBetService(IHomeDrawAwayNoBetDataAccess homeDrawAwayNoBetDataAccess)
        {
            _homeDrawAwayNoBetDataAccess = homeDrawAwayNoBetDataAccess;
        }

        public IList<HomeDrawAwayNoBet> GetHomeDrawAwayNoBetDataList(HomeDrawAwayNoBetMetaModel metaModel)
        {
            return _homeDrawAwayNoBetDataAccess.GetHomeDrawAwayNoBetDataList(metaModel);
        }
    }
}