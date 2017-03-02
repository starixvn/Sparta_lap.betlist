// -----------------------------------------------------------------------
// <copyright file="LeagueMetaModel.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Model.Report.MetaModel
{
    using System;

    public class LeagueMetaModel
    {
        public LeagueMetaModel()
        {
        }

        public string AgentListXml { get; set; }

        public int CurrencyId { get; set; }

        public int UserId { get; set; }

        public int LicenseeId { get; set; }

        public int SportId { get; set; }

        public DateTime? FromDate { get; set; }

        public DateTime? ToDate { get; set; }
    }
}