// -----------------------------------------------------------------------
// <copyright file="NoteViewModel.cs" company="Nexcel">
// warehouse winloss form parameters
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Model.Report.ViewModel.Note
{
    using System;
    using System.Collections.Generic;
    using Fanex.Athena.Model.Core.DTO.Common;

    /// <summary>
    /// NoteViewModel class.
    /// </summary>
    public class NoteViewModel
    {
        public NoteViewModel()
        {
            IsHistory = false;
            ThreeMonthBefore = DateTime.Today.AddMonths(-3).AddDays(1 - DateTime.Today.Day);
        }

        public DateTime BalanceUpDate { get; set; }

        public DateTime MoveBetsDate { get; set; }

        public DateTime ThreeMonthBefore { get; set; }

        public IList<Product> ProductList { get; set; }

        public bool IsHistory { get; set; }
    }
}