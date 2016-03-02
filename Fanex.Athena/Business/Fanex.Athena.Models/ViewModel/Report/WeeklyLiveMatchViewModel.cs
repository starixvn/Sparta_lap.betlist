// -----------------------------------------------------------------------
// <copyright file="WeeklyLiveMatchViewModel.cs" company="Nexcel">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Models.ViewModel.Report
{
    using System.Collections.Generic;
    using DTO.Report;
    using Fanex.Athena.Models.MetaModel.Report;

    public class WeeklyLiveMatchViewModel : BaseViewModel
    {
        public WeeklyLiveMatchMetaModel Param { get; set; }

        public IEnumerable<WeeklyLiveMatch> Data { get; set; }

        public WeeklyLiveMatch TotalData { get; set; }

        public int TotalRows { get; set; }
    }
}