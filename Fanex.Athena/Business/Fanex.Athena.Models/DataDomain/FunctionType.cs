// -----------------------------------------------------------------------
// <copyright file="FunctionType.cs" company="Nexcel">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Models.DataDomain
{
    public enum FunctionType
    {
        /// <summary>
        /// None, value = 0.
        /// </summary>
        None = 0,

        /// <summary>
        /// Total bets function, value = 1.
        /// </summary>
        TotalBets = 1,

        /// <summary>
        /// Forecast function, value = 2.
        /// </summary>
        Forecast = 2,

        /// <summary>
        /// Reports function, value = 3.
        /// </summary>
        Reports = 3,

        /// <summary>
        /// CustomerInformation function, value = 4.
        /// </summary>
        CustomerInformation = 4,

        /// <summary>
        /// DataWarehouse function, value = 5.
        /// </summary>
        DataWarehouse = 5,

        /// <summary>
        /// AccountManagement function, value = 6.
        /// </summary>
        AccountManagement = 6,

        /// <summary>
        /// Total bets function, value = 7.
        /// </summary>
        HandicapOverUnderLive = 7,

        /// <summary>
        /// MoneyLine function, value = 8.
        /// </summary>
        MoneyLine = 8,

        /// <summary>
        /// OddEven, One X Two, Draw no draw function, value = 9.
        /// </summary>
        OddEven1x2Dnd = 9,

        /// <summary>
        /// CorrectScore function, value = 10.
        /// </summary>
        CorrectScore = 10,

        /// <summary>
        /// TotalGoal function, value = 11.
        /// </summary>
        TotalGoal = 11,

        /// <summary>
        /// MixParlay function, value = 12.
        /// </summary>
        MixParlay = 12,

        /// <summary>
        /// Outright function, value = 13.
        /// </summary>
        Outright = 13,

        /// <summary>
        /// Half time full time function, value = 14.
        /// </summary>
        HtFt = 14,

        /// <summary>
        /// First goal last goal function, value = 15.
        /// </summary>
        FgLg = 15,

        /// <summary>
        /// CleanSheet function, value = 16.
        /// </summary>
        CleanSheet = 16,

        /// <summary>
        /// DoubleChance function, value = 17.
        /// </summary>
        DoubleChance = 17,

        /// <summary>
        /// HomeDrawAwayNoBet function, value = 18.
        /// </summary>
        HomeDrawAwayNoBet = 18,

        /// <summary>
        /// BONTeamToScore function, value = 19.
        /// </summary>
        BONTeamToScore = 19,

        /// <summary>
        /// ToWinToNil function, value = 20.
        /// </summary>
        ToWinToNil = 20,

        /// <summary>
        /// ThreeWayHandicap function, value = 21.
        /// </summary>
        ThreeWayHandicap = 21,

        /// <summary>
        /// Racing function, value = 22.
        /// </summary>
        Racing = 22,

        /// <summary>
        /// NumberGame function, value = 23.
        /// </summary>
        NumberGame = 23,

        /// <summary>
        /// ForecastHandicapOverUnderLive function, value = 24.
        /// </summary>
        ForecastHandicapOverUnderLive = 24,

        /// <summary>
        /// ForecastOnexTwo function, value = 25.
        /// </summary>
        ForecastOnexTwo = 25,

        /// <summary>
        /// LastBetsMonitoring function, value = 26.
        /// </summary>
        LastBetsMonitoring = 26,

        /// <summary>
        /// LicenseeWinLoss function, value = 27.
        /// </summary>
        LicenseeWinLoss = 27,

        /// <summary>
        /// SettledMatchCount function, value = 28.
        /// </summary>
        SettledMatchCount = 28,

        /// <summary>
        /// CustomerWinLoss function, value = 29.
        /// </summary>
        CustomerWinLoss = 29,

        /// <summary>
        /// CustomerPendingFunds function, value = 30.
        /// </summary>
        CustomerPendingFunds = 30,

        /// <summary>
        /// MatchWinLoss function, value = 31.
        /// </summary>
        MatchWinLoss = 31,

        /// <summary>
        /// WinLossByProducts function, value = 32.
        /// </summary>
        WinLossByProducts = 32,

        /// <summary>
        /// WinLossByBetType function, value = 33.
        /// </summary>
        WinLossByBetType = 33,

        /// <summary>
        /// RacingCommission function, value = 34.
        /// </summary>
        RacingCommission = 34,

        /// <summary>
        /// DailyRacingWinLoss function, value = 35.
        /// </summary>
        DailyRacingWinLoss = 35,

        /// <summary>
        /// CancelledBets function, value = 36.
        /// </summary>
        CancelledBets = 36,

        /// <summary>
        /// DailyBetList function, value = 37.
        /// </summary>
        DailyBetList = 37,

        /// <summary>
        /// LiveReport function, value = 38.
        /// </summary>
        LiveReport = 38,

        /// <summary>
        /// Turnover function, value = 39.
        /// </summary>
        Turnover = 39,

        /// <summary>
        /// CustomerBalance function, value = 40.
        /// </summary>
        CustomerBalance = 40,

        /// <summary>
        /// Results function, value = 41.
        /// </summary>
        Results = 41,

        /// <summary>
        /// Balance function, value = 42.
        /// </summary>
        Balance = 42,

        /// <summary>
        /// CustomerList function, value = 43.
        /// </summary>
        CustomerList = 43,

        /// <summary>
        /// WinLoss function, value = 44.
        /// </summary>
        WinLoss = 44,

        /// <summary>
        /// TopCustomer function, value = 45.
        /// </summary>
        TopCustomer = 45,

        /// <summary>
        /// ManageAccount function, value = 46.
        /// </summary>
        ManageAccount = 46,

        /// <summary>
        /// AddNewAccount function, value = 47.
        /// </summary>
        AddNewAccount = 47,

        /// <summary>
        /// EditNewAccount function, value = 48.
        /// </summary>
        EditNewAccount = 48,

        /// <summary>
        /// AssignPermission function, value = 49.
        /// </summary>
        AssignPermission = 49,

        /// <summary>
        /// ResetPassword function, value = 50.
        /// </summary>
        ResetPassword = 50,

        /// <summary>
        /// CustomerTurnover function, value = 51.
        /// </summary>
        CustomerTurnover = 51,

        /// <summary>
        /// CustomerReports function, value = 52.
        /// </summary>
        CustomerReports = 52,

        /// <summary>
        /// MobileWinLoss function, value = 53.
        /// </summary>
        MobileWinLoss = 53,

        /// <summary>
        /// SpecialBa function, value = 54.
        /// </summary>
        SpecialBa = 54,

        /// <summary>
        /// CommissionGroupList function, value = 55.
        /// </summary>
        CommissionGroupList = 55,
    }
}