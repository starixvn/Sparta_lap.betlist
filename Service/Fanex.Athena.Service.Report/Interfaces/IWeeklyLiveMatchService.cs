// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IWeeklyLiveMatchService.cs" company="">
//
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Fanex.Athena.Service.Report.Interfaces
{
    using System;
    using System.Collections.Generic;
    using Fanex.Athena.Model.Report.DTO;
    using Fanex.Athena.Model.Report.MetaModel;

    public interface IWeeklyLiveMatchService
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1021:AvoidOutParameters",
            Justification = "Reviewed. Suppression is OK here.")]
        IEnumerable<WeeklyLiveMatch> GetData(WeeklyLiveMatchMetaModel metaModel, out int[] total);

        IEnumerable<string> GetWeekHeaders(DateTime fromDate, DateTime toDate, int numberOfWeeks);
    }
}