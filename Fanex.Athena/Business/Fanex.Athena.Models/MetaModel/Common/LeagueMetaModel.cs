// -----------------------------------------------------------------------
// <copyright file="LeagueMetaModel.cs" company="Nexcel">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Models.MetaModel.Report
{
    using System;

    /// <summary>
    /// LeagueMetaModel class.
    /// </summary>
    public class LeagueMetaModel
    {
        public LeagueMetaModel()
        {                           
        }

        public int CurrencyId { get; set; }

        public int UserId { get; set; }

        public int LicenseeId { get; set; }

        public int SportId { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }      
    }
}