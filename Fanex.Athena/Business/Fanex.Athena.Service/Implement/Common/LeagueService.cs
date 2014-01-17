// -----------------------------------------------------------------------
// <copyright file="LeagueService.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Service.Implement.Common
{
    using System.Collections.Generic;
    using Fanex.Athena.DataAccess.Interface.Common;
    using Fanex.Athena.Models.DTO.Common;
    using Fanex.Athena.Models.MetaModel.Report;
    using Fanex.Athena.Service.Interface.Common;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class LeagueService : ILeagueService
    {        
        private ILeagueDataAccess _dataAccess = new LeagueDataAccess();

        #region ILeagueService Members

        public IList<League> GetLeaguesBySport(LeagueMetaModel metaModel)
        {            
            return _dataAccess.GetLeaguesBySport(metaModel);            
        }

        #endregion ILeagueService Members
    }
}