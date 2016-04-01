// -----------------------------------------------------------------------
// <copyright file="MobileWinLossMetaModel.cs" company="Nexcel">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Models.MetaModel.Report
{
    using System;
    using Fanex.Athena.Models.DataDomain;

    /// <summary>
    /// MobileWinLossMetaModel class.
    /// </summary>
    public class MobileWinLossMetaModel : BaseReportMetaModel
    {
        public MobileWinLossMetaModel()
        {
            LicenseeId = 0;
            var today = DateTime.Now;
            Year = today.Year;
            Month = today.Month;
            FromDay = 1;
            ToDay = today.Day;
            CurrencyId = 0;
            DrawFilter = DrawFilterType.Normal;
            Platform = PlatformType.SmartSite;
            IsExcludeTest = true;
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

        public int Platform { get; set; }

        public bool IsExcludeTest { get; set; }

        public override bool IsValid
        {
            get
            {
                return base.IsValid && Platform <= PlatformType.SmartSite && Platform >= PlatformType.All;
            }
        }
    }
}