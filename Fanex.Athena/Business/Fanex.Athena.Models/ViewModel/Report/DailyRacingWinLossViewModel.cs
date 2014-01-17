// -----------------------------------------------------------------------
// <copyright file="DailyRacingWinLossViewModel.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Models.ViewModel.Report
{
    using Fanex.Athena.Models.DTO.Report;
    using Fanex.Athena.Models.MetaModel.Report;

    /// <summary>
    /// DailyRacingWinLossViewModel class.
    /// </summary>
    public class DailyRacingWinLossViewModel : BaseViewModel
    {
        public DailyRacingWinLossMetaModel Param { get; set; }

        public DailyRacingWinLossData DailyRacingWinLossData { get; set; }
    }
}