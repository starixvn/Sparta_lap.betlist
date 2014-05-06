// -----------------------------------------------------------------------
// <copyright file="AppSettings.cs" company="Nexcel Solution">
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
    public class AppSettings
    {
        #region Definitions

        public const string SessionValidatingKey = "SESSION_VALIDATING";

        #endregion Definitions

        internal const string HttpSlashSlash = "http://";
        internal const char UrlSlash = '/';

        #region Configurations

        public static bool EnableSessionValidatingService
        {
            get { return int.Parse(GetValue("EnableSessionValidatingService")) != 0; }
        }

        public static string RemoteRefUrl
        {
            get { return GetValue("RemoteRefURL"); }
        }

        public static string JsVersion
        {
            get { return GetValue("JsVersion"); }
        }

        public static bool LocalTest
        {
            get { return int.Parse(GetValue("LocalTest")) != 0; }
        }

        public static bool IsTesting
        {
            get { return int.Parse(GetValue("IsTesting")) != 0; }
        }

        public static string LbUrl
        {
            get { return GetValue("LBUrl"); }
        }

        public static bool IsShowMixedSport
        {
            get { return int.Parse(GetValue("IsShowMixedSport")) == 1; }
        }

        public static DateTime HistoryDate
        {
            get { return DateTime.Parse(GetValue("HistoryDate")); }
        }

        public static string[] BasedCurrency
        {
            get { return GetValue("BasedCurrency").Split(new[] { ';' }); }
        }

        #endregion Configurations

        #region Racing Propertise

        public static bool EnableRacing
        {
            get
            {
                return GetValue("EnableRacing").Equals("1");
            }
        }

        public static bool EnableRacingHrUk
        {
            get
            {
                return GetValue("EnableRacingHRUK").Equals("1");
            }
        }

        public static bool EnableRacingHrSa
        {
            get
            {
                return GetValue("EnableRacingHRSA").Equals("1");
            }
        }

        public static bool EnableRacingHrAust
        {
            get
            {
                return GetValue("EnableRacingHRAust").Equals("1");
            }
        }

        public static bool EnableRacingGreyhoundUk
        {
            get
            {
                return GetValue("EnableRacingGreyhoundUK").Equals("1");
            }
        }

        public static bool EnableRacingGreyhoundAust
        {
            get
            {
                return GetValue("EnableRacingGreyhoundAust").Equals("1");
            }
        }

        public static bool EnableRacingHarnessAust
        {
            get
            {
                return GetValue("EnableRacingHarnessAust").Equals("1");
            }
        }

        #endregion Racing Propertise

        #region Casino

        public static bool IsDisableRngCasino
        {
            get { return bool.Parse(GetValue("IsDisableRngCasino")); }
        }

        #endregion Casino

        #region NumberGame

        public static bool EnableNumberGame
        {
            get { return GetValue("EnableNumberGame").Equals("1"); }
        }

        #endregion NumberGame

        #region ExcludeFunctionIdList

        public static IList<int> ExcludeFunctionIdList
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

        public static int ColumnWidth
        {
            get
            {
                return int.Parse(GetValue("ColumnWidth"));
            }
        }

        #endregion HTML Fix Header Table

        #region Assign Currency

        public static bool EnableAssignCurrency
        {
            get { return bool.Parse(GetValue("EnableAssignCurrency")); }
        }

        #endregion Assign Currency

        public static string GetValue(string key)
        {
            return ConfigurationManager.AppSettings[key];
        }
    }
}