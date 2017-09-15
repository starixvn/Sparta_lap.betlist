namespace Fanex.Athena.Extension.Utility
{
    using System;
    using System.Collections.Generic;

    public interface IAppSettings
    {
        IEnumerable<int> LicenseesNotHaveSystemId { get; }

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

        string RemoteRefUrl { get; }

        string ClientViewType { get; }

        string GetValue(string key);
    }
}