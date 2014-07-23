// -----------------------------------------------------------------------
// <copyright file="RNGCasinoResultMetalModel.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Models.MetaModel.Casino
{
    using System;

    /// <summary>
    /// RGNCasinoResultMetalModel class.
    /// </summary>
    [Serializable]
    public class RNGCasinoResultMetalModel
    {
        public long RefNo { get; set; }

        public string BetType
        {
            get
            {
                return "Casino";
            }
        }

        public bool IsShowPic { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public string ExternalId { get; set; }

        public string SystemId { get; set; }

        public string UserName
        {
            get
            {
                return SystemId;
            }
        }

        public int CustId { get; set; }

        public int CurrencyId { get; set; }

        public string CurrencyName { get; set; }

        public string BaseCurrency { get; set; }

        public bool Isvalid()
        {
            return RefNo > 0
                && FromDate <= ToDate && CustId > 0
                && !string.IsNullOrEmpty(UserName);
        }
    }
}