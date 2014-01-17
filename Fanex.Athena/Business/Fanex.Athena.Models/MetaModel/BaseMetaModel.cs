// -----------------------------------------------------------------------
// <copyright file="BaseMetaModel.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Models.MetaModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using Fanex.Athena.Extension.Utility;

    /// <summary>
    /// BaseMetaModel class.
    /// </summary>
    [Serializable]
    public class BaseMetaModel
    {
        #region Private

        private string _baseCurrency;

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

        public IList<string> BaseUsdLicensees
        {
            get
            {
                var items = AppSettings.GetValue("BaseUsdLicensees");
                if (!string.IsNullOrEmpty(items))
                {
                    return items.Split(',');
                }

                return new List<string>();
            }
        }

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

        public bool IsConvert
        {
            get
            {
                return !string.IsNullOrEmpty(BaseCurrency);
            }
        }
    }
}