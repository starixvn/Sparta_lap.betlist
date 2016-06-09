// -----------------------------------------------------------------------
// <copyright file="RNGCasinoResultMetaModel.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Model.Casino.MetaModel
{
    using System;

    /// <summary>
    /// RNGCasinoResultMetaModel class.
    /// </summary>
    [Serializable]
    public class RNGCasinoResultMetaModel
    {
        public bool IsExportExcel { get; set; }

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

        public bool IsValid()
        {
            return RefNo > 0
                && FromDate <= ToDate && CustId > 0
                && !string.IsNullOrEmpty(UserName);
        }
    }
}