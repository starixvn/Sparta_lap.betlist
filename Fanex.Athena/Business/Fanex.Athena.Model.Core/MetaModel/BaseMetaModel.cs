// -----------------------------------------------------------------------
// <copyright file="BaseMetaModel.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Model.Core.MetaModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using Fanex.Athena.Model.Core.DataDomain;

    /// <summary>
    /// BaseMetaModel class.
    /// </summary>
    [Serializable]
    public class BaseMetaModel
    {
        #region Private

        private string _baseCurrency = string.Empty;

        #endregion Private

        public BaseMetaModel()
        {
            CurrencyId = 0;
            CurrencyName = string.Empty;
        }

        public int LicenseeId { get; set; }

        public int UserId { get; set; }

        [DefaultValue("")]
        public string LicenseeType { get; set; }

        public int CurrencyId { get; set; }

        [DefaultValue("")]
        public string CurrencyName { get; set; }

        [DefaultValue("")]
        public string BaseCurrency
        {
            get
            {
                return _baseCurrency;
            }

            set
            {
                _baseCurrency = value == null
                    ? string.Empty
                    : value.ToLower();
            }
        }

        public int BaseCurrencyId
        {
            get
            {
                var baseCurrencyName = BaseCurrency.ToUpperInvariant();
                switch (baseCurrencyName)
                {
                    case "EUR":
                        return BaseCurrencyType.Eur;

                    case "USD":
                        return BaseCurrencyType.Usd;

                    case "GBP":
                        return BaseCurrencyType.Gbp;

                    default:
                        return BaseCurrencyType.All;
                }
            }
        }

        public bool IsConvert
        {
            get
            {
                return !string.IsNullOrEmpty(BaseCurrency);
            }
        }
    }
}