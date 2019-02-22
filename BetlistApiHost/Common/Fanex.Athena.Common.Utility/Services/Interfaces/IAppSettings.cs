namespace Fanex.Athena.Common.Utility.Services.Interfaces
{
    using System;
    using System.Collections.Generic;

    public interface IAppSettings
    {
#pragma warning disable S3996 // URI properties should not be strings
        string BetListApiHostUrl { get; }
#pragma warning restore S3996 // URI properties should not be strings

        bool IsProduction { get; }

        bool IsDisableExportLargeBetList { get; }

        IEnumerable<int> LicenseesWithLargeData { get; }

        IEnumerable<int> LicenseeOfferNewForecast { get; }

        IEnumerable<int> LicenseeAllowCasinoTransfer { get; }

        IEnumerable<int> LicenseeOfferStatusFunction { get; }

        string ExchangeProductApiHost { get; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Sonar Code Smell", "S3996:UriPropertiesShouldNotBeStrings",
            Justification = "Reviewed. Suppression is OK here.")]
        string ResultAmendmentApiUrl { get; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Sonar Code Smell", "S3996:UriPropertiesShouldNotBeStrings",
            Justification = "Reviewed. Suppression is OK here.")]
        string MarkNotificationAsReadApiUrl { get; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Sonar Code Smell", "S3996:UriPropertiesShouldNotBeStrings",
            Justification = "Reviewed. Suppression is OK here.")]
        string ResultAmendmentAttachmentApiUrl { get; }

        IEnumerable<int> FakeLicenseeList { get; }

        bool IsEnabledMoveBetNotification { get; }

        string[] BasedCurrency { get; }

        bool EnableAssignCurrency { get; }

        bool EnableSessionValidatingService { get; }

        string Environment { get; }

        DateTime HistoryDate { get; }

        bool IsDisableRngCasino { get; }

        bool IsShowMixedSport { get; }

        bool IsShowAllDataForSettleMatchCountReport { get; }

        IEnumerable<int> LiveReportNoDetailLinkSports { get; }

        bool LocalTest { get; }

        DateTime MaxDate { get; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Sonar Code Smell", "S3996:UriPropertiesShouldNotBeStrings",
            Justification = "Reviewed. Suppression is OK here.")]
        string RemoteRefUrl { get; }

        string TempFilePath { get; }

        string GetValue(string key);
    }
}
