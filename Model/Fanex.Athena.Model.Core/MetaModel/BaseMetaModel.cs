// -----------------------------------------------------------------------
// <copyright file="BaseMetaModel.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Model.Core.MetaModel
{
    using System;
    using System.ComponentModel;
    using Fanex.Athena.Model.Core.DataDomain;

    [Serializable]
    public class BaseMetaModel
    {
        private string _baseCurrency = string.Empty;

        public BaseMetaModel()
        {
            CurrencyId = 0;
            CurrencyName = string.Empty;
        }

        public string AgentListXml { get; set; }

        public int LicenseeId { get; set; }

        public int UserId { get; set; }

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