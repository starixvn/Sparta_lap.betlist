// -----------------------------------------------------------------------
// <copyright file="CustomerWinLossHistoryMetaModel.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Model.Report.MetaModel
{
    using System;

    /// <summary>
    /// CustomerWinLossHistoryMetaModel class.
    /// </summary>
    public class CustomerWinLossHistoryMetaModel : CustomerWinLossMetaModel
    {
        public CustomerWinLossHistoryMetaModel()
        {
            LicenseeType = string.Empty;
            PageIndex = 1;
            PageSize = 1000;
            FromDay = 1;
            ToDay = DateTime.Today.Day;
            Month = DateTime.Today.Month;
            Year = DateTime.Today.Year;
            FromDate = new DateTime(Year, Month, FromDay);
            ToDate = new DateTime(Year, Month, ToDay);
        }

        public int FromDay { get; set; }

        public int ToDay { get; set; }

        public int Month { get; set; }

        public int Year { get; set; }
    }
}