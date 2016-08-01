namespace Fanex.Athena.Extension.Utility
{
    using System;
    using System.Collections.Generic;

    public interface IAppSettings
    {
        string[] BasedCurrency { get; }

        int ColumnWidth { get; }

        bool EnableAssignCurrency { get; }

        bool EnableNumberGame { get; }

        bool EnableRacing { get; }

        bool EnableRacingGreyhoundAust { get; }

        bool EnableRacingGreyhoundUk { get; }

        bool EnableRacingHarnessAust { get; }

        bool EnableRacingHRAust { get; }

        bool EnableRacingHRSa { get; }

        bool EnableRacingHRUk { get; }

        bool EnableSessionValidatingService { get; }

        string Environment { get; }

        IList<int> ExcludeFunctionIdList { get; }

        DateTime HistoryDate { get; }

        bool IsDisableRngCasino { get; }

        bool IsEnablePiwik { get; }

        bool IsShowMixedSport { get; }

        bool IsTesting { get; }

        IEnumerable<int> LiveReportNoDetailLinkSports { get; }

        bool LocalTest { get; }

        DateTime MaxDate { get; }

        DateTime? MoveBetDayEnd { get; }

        DateTime? MoveBetDayStart { get; }

        string RemoteRefUrl { get; }

        string GetValue(string key);
    }
}