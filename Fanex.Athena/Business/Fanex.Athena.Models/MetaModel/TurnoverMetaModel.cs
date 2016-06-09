// -----------------------------------------------------------------------
// <copyright file="TurnoverMetaModel.cs" company="Nexcel">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Model.Report.MetaModel
{
    using System;

    /// <summary>
    /// TurnoverMetaModel class.
    /// </summary>
    public class TurnoverMetaModel : BaseReportMetaModel
    {
        public TurnoverMetaModel()
        {               
            Year = DateTime.Today.Year;
            Month = DateTime.Today.Month;
            FromDay = 1;
            ToDay = DateTime.Today.Day;
            IsExcludeTest = true;
        }

        public int FromDay { get; set; }

        public int ToDay { get; set; }

        public int Month { get; set; }

        public int Year { get; set; }

        public new DateTime FromDate
        {
            get
            {
                return new DateTime(Year, Month, FromDay);
            }
        }

        public new DateTime ToDate
        {
            get
            {
                return new DateTime(Year, Month, ToDay);
            }
        }

        public bool IsExcludeTest { get; set; }

        public string Username { get; set; }

        public override bool IsValid
        {
            get
            {
                return FromDate <= ToDate;
            }
        }
    }
}