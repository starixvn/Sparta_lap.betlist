// -----------------------------------------------------------------------
// <copyright file="PlatformWinLossMetaModel.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Model.Report.MetaModel
{
    using System;
    using DataDomain = Model.Core.DataDomain;

    public class PlatformWinLossMetaModel : BaseReportMetaModel
    {
        public PlatformWinLossMetaModel()
        {
            LicenseeId = 0;
            var today = DateTime.Now;
            Year = today.Year;
            Month = today.Month;
            FromDay = 1;
            ToDay = today.Day;
            CurrencyId = 0;
            DrawFilter = DataDomain.DrawFilterType.Normal;
            PlatformType = DataDomain.PlatformType.All;
            IsExcludedTestData = true;
        }

        public int Level { get; set; }

        public int Year { get; set; }

        public int Month { get; set; }

        public int FromDay { get; set; }

        public int ToDay { get; set; }

        public new DateTime FromDate
        {
            get { return new DateTime(Year, Month, FromDay); }
        }

        public new DateTime ToDate
        {
            get { return new DateTime(Year, Month, ToDay); }
        }

        public int DrawFilter { get; set; }

        public int PlatformType { get; set; }

        public bool IsExcludedTestData { get; set; }

        public override bool IsValid
        {
            get
            {
                return base.IsValid && PlatformType <= DataDomain.PlatformType.Tablet && PlatformType >= DataDomain.PlatformType.All;
            }
        }
    }
}