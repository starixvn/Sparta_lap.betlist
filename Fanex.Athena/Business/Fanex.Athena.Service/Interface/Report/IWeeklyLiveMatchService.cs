// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IWeeklyLiveMatchService.cs" company="Nexcel">
// The active customer service.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Fanex.Athena.Service.Interface.Report
{
    using System.Collections.Generic;
    using Fanex.Athena.Extension.Utility;
    using Fanex.Athena.Models.DTO.Report;
    using Fanex.Athena.Models.MetaModel.Report;

    public interface IWeeklyLiveMatchService
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1021:AvoidOutParameters",
            Justification = "Reviewed. Suppression is OK here.")]
        IEnumerable<WeeklyLiveMatch> GetData(WeeklyLiveMatchMetaModel metaModel, out WeeklyLiveMatch total);
    }
}