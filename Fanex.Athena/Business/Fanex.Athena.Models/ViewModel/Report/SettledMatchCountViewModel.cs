// -----------------------------------------------------------------------
// <copyright file="SettledMatchCountViewModel.cs" company="Nexcel">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Models.ViewModel.Report
{
    /// <summary>
    /// SettledMatchCountViewModel class.
    /// </summary>
    public class SettledMatchCountViewModel
    {
        public string SportName { get; set; }

        public int YesterdayEntered { get; set; }

        public int YesterdayNonEntered { get; set; }

        public int TodayEntered { get; set; }

        public int TodayNonEntered { get; set; }
    }
}