// -----------------------------------------------------------------------
// <copyright file="LeagueService.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Service.Core.Common
{
    using System.Collections.Generic;
    using Fanex.Athena.DataAccess.Core.Common.Interfaces;
    using Fanex.Athena.Models.DTO.Common;
    using Fanex.Athena.Models.MetaModel.Report;
    using Fanex.Athena.Service.Core.Common.Interfaces;

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