// -----------------------------------------------------------------------
// <copyright file="DailyRacingWinLossViewModel.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Model.Report.ViewModel
{
    using Fanex.Athena.Model.Report.DTO;
    using Fanex.Athena.Model.Report.MetaModel;
    using Model.Core.ViewModel;

    /// <summary>
    /// DailyRacingWinLossViewModel class.
    /// </summary>
    public class DailyRacingWinLossViewModel : BaseViewModel
    {
        public DailyRacingWinLossMetaModel Param { get; set; }

        public DailyRacingWinLossData DailyRacingWinLossData { get; set; }
    }
}