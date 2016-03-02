// --------------------------------------------------------------------------------------------------------------------
// <copyright file="WeeklyLiveMatchService.cs" company="Nexcel">
// The licensee win loss service.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Fanex.Athena.Service.Implement.Report
{
    using System.Collections.Generic;
    using System.Linq;
    using Fanex.Athena.DataAccess.Implement.Report;
    using Fanex.Athena.DataAccess.Interface.Report;
    using Fanex.Athena.Extension.Utility;
    using Fanex.Athena.Models.DTO.Report;
    using Fanex.Athena.Models.MetaModel.Report;
    using Fanex.Athena.Service.Implement.Configuration;
    using Fanex.Athena.Service.Interface.Configuration;
    using Fanex.Athena.Service.Interface.Report;

    /// <summary>
    ///     The active customer service.
    /// </summary>
    public class WeeklyLiveMatchService : IWeeklyLiveMatchService
    {
        private readonly IWeeklyLiveMatchDataAccess _weeklyLiveMatchDataAccess;

        public WeeklyLiveMatchService()
        {
            this._weeklyLiveMatchDataAccess = new WeeklyLiveMatchDataAccess();
        }

        public IEnumerable<WeeklyLiveMatch> GetData(WeeklyLiveMatchMetaModel metaModel, out WeeklyLiveMatch total)
        {
            if (metaModel.IsValid)
            {
                return this._weeklyLiveMatchDataAccess.GetDataList(metaModel, out total);
            }
            else
            {
                total = new WeeklyLiveMatch();
                return new SortableList<WeeklyLiveMatch>();
            }
        }
    }
}