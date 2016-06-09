// -----------------------------------------------------------------------
// <copyright file="SettledMatchCount.cs" company="Nexcel">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Model.Report.DTO
{
    using System;

    /// <summary>
    /// SettledMatchCount class.
    /// </summary>
    public class SettledMatchCount
    {
        public int IsToday { get; set; }

        public DateTime EventDate { get; set; }

        public int SportType { get; set; }

        public int Entered { get; set; }

        public int NonEntered { get; set; }
    }
}