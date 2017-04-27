// -----------------------------------------------------------------------
// <copyright file="LeagueService.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Service.Core.Common
{
    using System.Collections.Generic;
    using DataAccess.Core.Common;
    using Fanex.Athena.DataAccess.Core.Common.Interfaces;
    using Fanex.Athena.Model.Core.DTO.Common;
    using Fanex.Athena.Service.Core.Common.Interfaces;
    using Model.Core.MetaModel.Common;

    public class LeagueService : ILeagueService
    {
        private readonly ILeagueDataAccess _dataAccess;

        public LeagueService()
        {
            _dataAccess = new LeagueDataAccess();
        }

        public LeagueService(ILeagueDataAccess dataAccess)
        {
            _dataAccess = dataAccess;
        }

        public IEnumerable<League> GetLeaguesBySport(LeagueMetaModel metaModel)
        {
            return _dataAccess.GetLeaguesBySport(metaModel);
        }
    }
}