using System;

namespace Fanex.Athena.Model.Report.ViewModel
{
    public class ResultAmendmentViewModel
    {
        public DateTime FromDate { get; set; } = DateTime.Today;

        public DateTime ToDate { get; set; } = DateTime.Today;
    }
}