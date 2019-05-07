// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IWeeklyLiveMatchService.cs" company="">
//
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Fanex.Athena.Service.Report.Interfaces
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Fanex.Athena.Model.Report.DTO;
    using Fanex.Athena.Model.Report.MetaModel;
    using Model.Report.ResultModel;

    public interface IWeeklyLiveMatchService
    {
        Task<WeeklyLiveMatchResultModel> GetData(WeeklyLiveMatchMetaModel metaModel);

        IEnumerable<string> GetWeekHeaders(DateTime fromDate, DateTime toDate, int numberOfWeeks);
    }
}