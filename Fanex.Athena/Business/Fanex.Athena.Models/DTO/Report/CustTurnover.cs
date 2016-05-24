// -----------------------------------------------------------------------
// <copyright file="CustTurnover.cs" company="Nexcel">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Models.DTO.Report
{
    using System;
    using Fanex.Athena.Model.Core.Helper;

    /// <summary>
    /// Customer Turnover class.
    /// </summary>
    public class CustTurnover : ICustomerWinLoss
    {
        private int _sportId;
        private string _sportType;

        public int Index { get; set; }

        public DateTime ReportBy { get; set; }

        public string UserName { get; set; }

        public int RowSpan { get; set; }

        public int RowSpan2 { get; set; }

        public string Currency { get; set; }

        public int SportId
        {
            get { return this._sportId; }
            set { this._sportId = value; }
        }

        public string SportType
        {
            get
            {
                if (string.IsNullOrEmpty(this._sportType))
                {
                    this._sportType = CacheRef.GetSportName(this._sportId);
                }

                return this._sportType;
            }
        }

        public int TotalTransactions { get; set; }

        public string Bettype { get; set; }

        public decimal EffectiveBettingValue { get; set; }

        public decimal Turnover { get; set; }

        public decimal TotalTurnover { get; set; }

        public decimal CustomerWinLoss { get; set; }

        public decimal CustomerComm { get; set; }

        public decimal CustomerTotal
        {
            get { return CustomerWinLoss + CustomerComm; }
        }
    }
}