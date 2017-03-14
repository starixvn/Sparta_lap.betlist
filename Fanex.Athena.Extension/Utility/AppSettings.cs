// -----------------------------------------------------------------------
// <copyright file="AppSettings.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Extension.Utility
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Linq;

    /// <summary>
    /// Setting class.
    /// </summary>
    public class AppSettings : IAppSettings
    {
        internal const string HttpSlashSlash = "http://";
        internal const char UrlSlash = '/';

        #region Configurations

        public string Environment
        {
            get { return GetValue("Environment"); }
        }

        public DateTime? MoveBetDayStart
        {
            get
            {
                var value = GetValue("MoveBetDayStart");
                DateTime date;
                if (DateTime.TryParse(value, out date))
                {
                    return date;
                }

                return null;
            }
        }

        public DateTime? MoveBetDayEnd
        {
            get
            {
                var value = GetValue("MoveBetDayEnd");
                DateTime date;
                if (DateTime.TryParse(value, out date))
                {
                    return date;
                }

                return null;
            }
        }

        public bool EnableSessionValidatingService
        {
            get { return int.Parse(GetValue("EnableSessionValidatingService")) != 0; }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design",
            "CA1056:UriPropertiesShouldNotBeStrings",
            Justification = "Reviewed. Suppression is OK here.")]
        public string RemoteRefUrl
        {
            get { return GetValue("RemoteRefURL"); }
        }

        public bool LocalTest
        {
            get { return int.Parse(GetValue("LocalTest")) != 0; }
        }

        public bool IsTesting
        {
            get { return int.Parse(GetValue("IsTesting")) != 0; }
        }

        public bool IsShowMixedSport
        {
            get { return int.Parse(GetValue("IsShowMixedSport")) == 1; }
        }

        public DateTime HistoryDate
        {
            get { return DateTime.Parse(GetValue("HistoryDate")); }
        }

        public DateTime MaxDate
        {
            get
            {
                var dateString = GetValue("MaxDate");
                return string.IsNullOrWhiteSpace(dateString) ? DateTime.Now : DateTime.Parse(dateString);
            }
        }

        public string[] BasedCurrency
        {
            get { return GetValue("BasedCurrency").Split(new[] { ';' }); }
        }

        public IEnumerable<int> LiveReportNoDetailLinkSports
        {
            get { return GetValue("LiveReportNoDetailLinkSports").Split(',').Select(n => Convert.ToInt32(n)); }
        }

        #endregion Configurations

        #region Racing Propertise

        public bool EnableRacing
        {
            get
            {
                return GetValue("EnableRacing").Equals("1");
            }
        }

        public bool EnableRacingHRUk
        {
            get
            {
                return GetValue("EnableRacingHRUK").Equals("1");
            }
        }

        public bool EnableRacingHRSa
        {
            get
            {
                return GetValue("EnableRacingHRSA").Equals("1");
            }
        }

        public bool EnableRacingHRAust
        {
            get
            {
                return GetValue("EnableRacingHRAust").Equals("1");
            }
        }

        public bool EnableRacingGreyhoundUk
        {
            get
            {
                return GetValue("EnableRacingGreyhoundUK").Equals("1");
            }
        }

        public bool EnableRacingGreyhoundAust
        {
            get
            {
                return GetValue("EnableRacingGreyhoundAust").Equals("1");
            }
        }

        public bool EnableRacingHarnessAust
        {
            get
            {
                return GetValue("EnableRacingHarnessAust").Equals("1");
            }
        }

        #endregion Racing Propertise

        #region Casino

        public bool IsDisableRngCasino
        {
            get { return bool.Parse(GetValue("IsDisableRngCasino")); }
        }

        #endregion Casino

        #region NumberGame

        public bool EnableNumberGame
        {
            get { return GetValue("EnableNumberGame").Equals("1"); }
        }

        #endregion NumberGame

        #region ExcludeFunctionIdList

        public IList<int> ExcludeFunctionIdList
        {
            get
            {
                return GetValue("ExcludeFunctionIdList")
                        .Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries)
                        .Select(s => int.Parse(s))
                        .ToList();
            }
        }

        #endregion ExcludeFunctionIdList

        #region HTML Fix Header Table

        public int ColumnWidth
        {
            get
            {
                return int.Parse(GetValue("ColumnWidth"));
            }
        }

        #endregion HTML Fix Header Table

        #region Assign Currency

        public bool EnableAssignCurrency
        {
            get { return bool.Parse(GetValue("EnableAssignCurrency")); }
        }

        public string ClientViewType
        {
            get
            {
                return ConfigurationManager.AppSettings["ClientViewType"];
            }
        }

        #endregion Assign Currency

        public string GetValue(string key)
        {
            return ConfigurationManager.AppSettings[key];
        }
    }
}