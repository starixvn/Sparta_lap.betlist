namespace Fanex.Athena.DataAccess.BetList.Util
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using Fanex.Athena.Model.BetList.MetaModel;
    using Fanex.Data;
    using Model.Core.DTO.Common;

    [System.CLSCompliant(false)]
    public static class BetListParameterBuilder
    {
        private static Dictionary<string, Action<IObjectDb, BetListMetaModel>> addSqlParameters = new Dictionary<string, Action<IObjectDb, BetListMetaModel>>
        {
            { "Lrf_BetList_WinlostByLeague_Mem", (db, metalModel) => CreateWinlossByLeagueParameters(db, metalModel) },
            { "Lrf_BetList_SelDeleted", (db, metalModel) => CreateCancelBetListParameters(db, metalModel) },
            { "Lrf_BetList_CorrectScore", (db, metalModel) => CreateCorectScoreBetlistParameters(db, metalModel) },
            { "Lrf_BetList_SelWinLossByDate_Mem", (db, metalModel) => CreateCustomerWlBetlistParameters(db, metalModel) },
            { "Lrf_BetList_SelWinLossRacingByDate_Mem", (db, metalModel) => CreateRacingCommissionBetlistParameters(db, metalModel) },
            { "Lrf_BetList_SelWinlossDaily", (db, metalModel) => CreateDailyWLBetlistParameters(db, metalModel) },
            { "Lrf_BetList_SelWinlostRacing", (db, metalModel) => CreateDailyRacingWLBetlistParameters(db, metalModel) },
            { "Lrf_BetList_SelWinloss_Mem", (db, metalModel) => CreateLicenseeWLBetlistParameters(db, metalModel) },
            { "Lrf_BetList_SelRunningByBettype", (db, metalModel) => CreateRunningBetListByBettypePaprameters(db, metalModel) },
            { "Lrf_BetList_SelRunningByBettypeList", (db, metalModel) => CreateRunningBetListByMultipleBettypePaprameters(db, metalModel) },
            { "Lrf_BetList_LiveReport_SelWinlossByMatch", (db, metalModel) => CreateLiveReportBetListByMatchParameters(db, metalModel) },
            { "Lrf_BetList_LiveReport_SelWinlossOutright", (db, metalModel) => CreateLiveReportOutRightBetlistParameters(db, metalModel) },
            { "Lrf_BetList_LiveReport_SelWinlossNumberGame", (db, metalModel) => CreaeteLiveReportNumberGameBetlistParameters(db, metalModel) },
            { "Lrf_BetList_LiveReport_SelWinlossParlay", (db, metalModel) => CreateLiveReportParlayBetlistParameters(db, metalModel) },
            { "Lrf_BetList_LiveReport_SelWinlossRacing", (db, metalModel) => CreateLiveReportBetListRacingParameters(db, metalModel) },
            { "Lrf_BetList_LiveReport_SelWinlossVHRacing", (db, metalModel) => CreateLiveReportBetListRacingParameters(db, metalModel) },
            { "Lrf_BetList_MatchWL_Racing", (db, metalModel) => CreateMatchWLRacingBetlistParameters(db, metalModel) },
            { "Lrf_BetList_MatchWL_Bettype", (db, metalModel) => CreateMatchWLBettypeBetlistParameters(db, metalModel) },
            { "Lrf_BetList_MatchWL_Outright", (db, metalModel) => CreateMatchWLOutRightBetListParameters(db, metalModel) },
            { "Lrf_BetList_MatchWL_Parlay", (db, metalModel) => CreateMatchWLParlayBetListParameters(db, metalModel) },
            { "Lrf_BetList_PlatformWinLoss_Mem", (db, metalModel) => CreatePlatformWinLossBetListParameters(db, metalModel) },
            { "Lrf_BetList_SelAllByDate_Mem", (db, metalModel) => CreateOutStandingCompletedBetListParameters(db, metalModel) },
            { "Lrf_BetList_SelByDayAll_Mem", (db, metalModel) => CreateDailyStatementBetListParameters(db, metalModel) },
            { "Lrf_BetList_SelByDayAll_Mem_Old", (db, metalModel) => CreateDailyStatementBetListOldParameters(db, metalModel) },
            { "Lrf_BetList_SelRunningByDate_Mem", (db, metalModel) => CreateRunningBetListParameters(db, metalModel) },
            { "Lrf_BetList_SelAllByDateHR", (db, metalModel) => CreateMatchRunningBetListOfHRParameters(db, metalModel) },
            { "Lrf_BetList_SelRunningByBettypeMatch", (db, metalModel) => CreateMatchRunningBetLitByBettypeParameters(db, metalModel) },
            { "Lrf_BetList_SelRunningByMultipleBettypeMatch", (db, metalModel) => CreateMatchRunningBetLitByMultipleBettypeOfMatchParameters(db, metalModel) },
            { "Lrf_BetList_Forecast_SelByMatch", (db, metalModel) => CreateRunningBetListByForeCastMatchParameters(db, metalModel) },
            { "Lrf_BetList_RNG_Detail", (db, metalModel) => CreateMicroGamingRngDetailParameters(db, metalModel) },
            { "Lrf_BetList_LC_Detail", (db, metalModel) => CreateAGCasinoDetailParameters(db, metalModel) },
            { "Lrf_BetList_SelWinLossByBetType", (db, metalModel) => CreateWinLossByBetTypeParameters(db, metalModel) }
        };

        public static void CreateBetListParameters(IObjectDb db, string uspKey, BetListMetaModel metaModel)
        {
            addSqlParameters[uspKey](db, metaModel);
        }

        public static void CreateBetListDateRangeParameters(IObjectDb db, BetListMetaModel metaModel)
        {
            db.AddInParameter("FromDate", DbType.DateTime, metaModel.FromDate);
            db.AddInParameter("ToDate", DbType.DateTime, metaModel.ToDate);
        }

        public static void CreateBetlistBUParameters(IObjectDb db, BetListMetaModel metaModel)
        {
            db.AddInParameter("MoveBetsDay", DbType.DateTime, metaModel.MoveBetsDay);
            db.AddInParameter("BalanceUpDay", DbType.DateTime, metaModel.BalanceUpDay);
        }

        public static void CreateBetListPagingParameters(IObjectDb db, BetListMetaModel metaModel)
        {
            db.AddInParameter("PageSize", DbType.Int32, metaModel.PageSize);
            db.AddInParameter("PageIndex", DbType.Int32, metaModel.PageIndex);
            db.AddOutParameter("totalrows", DbType.Int32, 4);
        }

        private static void CreateMatchRunningBetLitByMultipleBettypeOfMatchParameters(IObjectDb db, BetListMetaModel metaModel)
        {
            db.AddInParameter("AgentList", DbType.String, metaModel.AgentListXml);
            db.AddInParameter("bettype", DbType.String, !string.IsNullOrEmpty(metaModel.MultipleBettype) ? metaModel.MultipleBettype : string.Empty);
            db.AddInParameter("MatchID", DbType.Int32, metaModel.MatchId);
            CreateBetListPagingParameters(db, metaModel);
            db.AddInParameter("liveindicator", DbType.Int32, metaModel.LiveIndicator.HasValue ? metaModel.LiveIndicator.Value : -1);
            db.AddInParameter("betteam", DbType.String, metaModel.BetTeam);
        }

        private static void CreateWinlossByLeagueParameters(IObjectDb db, BetListMetaModel metaModel)
        {
            var showSb = metaModel.ProductOptions.Any(i => i.Code == Product.SB && i.Status == true);
            var showBa = metaModel.ProductOptions.Any(i => i.Code == Product.BA && i.Status == true);

            db.AddInParameter("CustID", DbType.Int32, metaModel.CustId);
            db.AddInParameter("LeagueID", DbType.Int32, metaModel.LeagueId);
            db.AddInParameter("MatchId", DbType.Int32, metaModel.MatchId);
            db.AddInParameter("AgentList", DbType.String, metaModel.AgentListXml);
            db.AddInParameter("Odds", DbType.Decimal, metaModel.Odds);
            db.AddInParameter("FromDate", DbType.DateTime, metaModel.FromDate);
            db.AddInParameter("ToDate", DbType.DateTime, metaModel.ToDate);
            db.AddInParameter("MoveBetsDay", DbType.DateTime, metaModel.MoveBetsDay);
            db.AddInParameter("BalanceUpDay", DbType.DateTime, metaModel.BalanceUpDay);
            db.AddInParameter("ShowWin", DbType.Boolean, metaModel.ShowWin);
            db.AddInParameter("ShowLoss", DbType.Boolean, metaModel.ShowLoss);
            db.AddInParameter("ShowDraw", DbType.Boolean, metaModel.ShowDraw);
            db.AddInParameter("ShowSB", DbType.Boolean, showSb);
            db.AddInParameter("ShowBA", DbType.Boolean, showBa);
        }

        private static void CreateRunningBetListByForeCastMatchParameters(IObjectDb db, BetListMetaModel metaModel)
        {
            db.AddInParameter("AgentList", DbType.String, metaModel.AgentListXml);
            db.AddInParameter("bettype", DbType.Int32, metaModel.BetType);
            db.AddInParameter("MatchID", DbType.Int32, metaModel.MatchId);
            CreateBetListPagingParameters(db, metaModel);
            db.AddInParameter("liveindicator", DbType.Int32, metaModel.LiveIndicator.HasValue ? metaModel.LiveIndicator.Value : -1);
            db.AddInParameter("betteam", DbType.String, metaModel.BetTeam);
            db.AddInParameter("BetChoice", DbType.String, metaModel.BetChoice);
            db.AddInParameter("Handicap", DbType.Decimal, metaModel.Handicap);
        }

        private static void CreateMatchRunningBetLitByBettypeParameters(IObjectDb db, BetListMetaModel metaModel)
        {
            db.AddInParameter("AgentList", DbType.String, metaModel.AgentListXml);
            db.AddInParameter("bettype", DbType.Int32, metaModel.BetType);
            db.AddInParameter("MatchID", DbType.Int32, metaModel.MatchId);
            CreateBetListPagingParameters(db, metaModel);
            db.AddInParameter("liveindicator", DbType.Int32, metaModel.LiveIndicator.HasValue ? metaModel.LiveIndicator.Value : -1);
            db.AddInParameter("betteam", DbType.String, metaModel.BetTeam);
        }

        private static void CreateMatchRunningBetListOfHRParameters(IObjectDb db, BetListMetaModel metaModel)
        {
            db.AddInParameter("AgentList", DbType.String, metaModel.AgentListXml);
            db.AddInParameter("bettype", DbType.Int32, metaModel.BetType);
            db.AddInParameter("MatchID", DbType.Int32, metaModel.MatchId);
            CreateBetListPagingParameters(db, metaModel);
            CreateBetlistSummaryParameters(db);
        }

        private static void CreateRunningBetListParameters(IObjectDb db, BetListMetaModel metaModel)
        {
            db.AddInParameter("custid", DbType.Int32, metaModel.CustId);
            db.AddInParameter("AgentList", DbType.String, metaModel.AgentListXml);
            CreateBetListDateRangeParameters(db, metaModel);
            CreateBetListPagingParameters(db, metaModel);
            CreateBetlistProductOptionParameters(db, metaModel);
        }

        private static void CreateDailyStatementBetListOldParameters(IObjectDb db, BetListMetaModel metaModel)
        {
            db.AddInParameter("AgentList", DbType.String, metaModel.AgentListXml);
            db.AddInParameter("custid", DbType.Int32, metaModel.CustId);
            db.AddInParameter("fromdate", DbType.DateTime, metaModel.FromDate);
            db.AddInParameter("transtype", DbType.Int32, metaModel.TransType);
            CreateBetListPagingParameters(db, metaModel);
            CreateBetlistSummaryParameters(db);
        }

        private static void CreateDailyStatementBetListParameters(IObjectDb db, BetListMetaModel metaModel)
        {
            db.AddInParameter("AgentList", DbType.String, metaModel.AgentListXml);
            db.AddInParameter("custid", DbType.Int32, metaModel.CustId);
            db.AddInParameter("fromdate", DbType.DateTime, metaModel.FromDate);
            db.AddInParameter("transtype", DbType.Int32, metaModel.TransType);
            db.AddInParameter("MoveBetsDay", DbType.DateTime, metaModel.MoveBetsDay);
            CreateBetListPagingParameters(db, metaModel);
            CreateBetlistSummaryParameters(db);
            CreateBetListErrorParameters(db);
        }

        private static void CreateOutStandingCompletedBetListParameters(IObjectDb db, BetListMetaModel metaModel)
        {
            db.AddInParameter("custid", DbType.Int32, metaModel.CustId);
            db.AddInParameter("AgentList", DbType.String, metaModel.AgentListXml);
            CreateBetListDateRangeParameters(db, metaModel);
            CreateBetListPagingParameters(db, metaModel);
            CreateBetlistSummaryParameters(db);
            CreateBetlistProductOptionParameters(db, metaModel);
        }

        private static void CreatePlatformWinLossBetListParameters(IObjectDb db, BetListMetaModel metaModel)
        {
            db.AddInParameter("custid", DbType.Int32, metaModel.CustId);
            db.AddInParameter("AgentList", DbType.String, metaModel.AgentListXml);
            CreateBetListDateRangeParameters(db, metaModel);
            CreateBetlistBUParameters(db, metaModel);
            CreateBetlistProductOptionParameters(db, metaModel);
            CreateBetListPagingParameters(db, metaModel);
            CreateBetlistSummaryParameters(db);
            db.AddInParameter("drawfilter", DbType.Int16, metaModel.DrawFilter);
            db.AddInParameter("PlatformType", DbType.Byte, metaModel.PlatformType);
        }

        private static void CreateMatchWLParlayBetListParameters(IObjectDb db, BetListMetaModel metaModel)
        {
            CreateBetListSortingParameters(db, metaModel);

            db.AddInParameter("AgentList", DbType.String, metaModel.AgentListXml);
            db.AddInParameter("bettype", DbType.Int32, metaModel.BetType);
            db.AddInParameter("MoveBetsDay", DbType.DateTime, metaModel.MoveBetsDay);
            CreateBetListDateRangeParameters(db, metaModel);
            CreateBetListPagingParameters(db, metaModel);
            CreateBetlistSummaryParameters(db);
            var suportProducts = new string[5] { Product.SB, Product.NG, Product.BA, Product.LC, Product.VS };
            var products = metaModel.ProductOptions.Where(i => suportProducts.Contains(i.Code));
            foreach (var product in products)
            {
                db.AddInParameter("show" + product.Code, DbType.Boolean, product.Status);
            }
        }

        private static void CreateMatchWLOutRightBetListParameters(IObjectDb db, BetListMetaModel metaModel)
        {
            db.AddInParameter("AgentList", DbType.String, metaModel.AgentListXml);
            CreateBetListDateRangeParameters(db, metaModel);
            db.AddInParameter("MoveBetsDay", DbType.DateTime, metaModel.MoveBetsDay);
            CreateBetListSortingParameters(db, metaModel);
            CreateBetListPagingParameters(db, metaModel);
            CreateBetlistSummaryParameters(db);
            var suportProducts = new string[2] { Product.SB, Product.BA };
            var products = metaModel.ProductOptions.Where(i => suportProducts.Contains(i.Code));
            foreach (var product in products)
            {
                db.AddInParameter("show" + product.Code, DbType.Boolean, product.Status);
            }
        }

        private static void CreateMatchWLBettypeBetlistParameters(IObjectDb db, BetListMetaModel metaModel)
        {
            db.AddInParameter("AgentList", DbType.String, metaModel.AgentListXml);
            db.AddInParameter("bettype", DbType.Int32, metaModel.BetType);
            db.AddInParameter("MatchID", DbType.Int32, metaModel.MatchId);
            db.AddInParameter("liveindicator", DbType.Int32, metaModel.LiveIndicator.HasValue ? metaModel.LiveIndicator.Value : -1);

            CreateBetListSortingParameters(db, metaModel);
            CreateBetListPagingParameters(db, metaModel);
            CreateBetlistSummaryParameters(db);
            db.AddInParameter("Date", DbType.DateTime, metaModel.Date);

            var suportProducts = new string[5] { Product.SB, Product.NG, Product.BA, Product.LC, Product.VS };
            var products = metaModel.ProductOptions.Where(i => suportProducts.Contains(i.Code));
            foreach (var product in products)
            {
                db.AddInParameter("show" + product.Code, DbType.Boolean, product.Status);
            }
        }

        private static void CreateMatchWLRacingBetlistParameters(IObjectDb db, BetListMetaModel metaModel)
        {
            db.AddInParameter("AgentList", DbType.String, metaModel.AgentListXml);
            db.AddInParameter("Bettype", DbType.Int32, metaModel.BetType);
            db.AddInParameter("LeagueId", DbType.Int32, metaModel.LeagueId);
            db.AddInParameter("RaceNo", DbType.String, metaModel.RaceNo);
            CreateBetListDateRangeParameters(db, metaModel);
            db.AddInParameter("MoveBetsDay", DbType.DateTime, metaModel.MoveBetsDay);
            CreateBetListSortingParameters(db, metaModel);
            CreateBetListPagingParameters(db, metaModel);
            CreateBetlistSummaryParameters(db);
        }

        private static void CreateLiveReportBetListRacingParameters(IObjectDb db, BetListMetaModel metaModel)
        {
            CreateBetListSortingParameters(db, metaModel);
            db.AddInParameter("AgentList", DbType.String, metaModel.AgentListXml);
            CreateBetListDateRangeParameters(db, metaModel);
            db.AddInParameter("MoveBetsDay", DbType.DateTime, metaModel.MoveBetsDay);
            db.AddInParameter("PageSize", DbType.Int32, metaModel.PageSize);
            db.AddInParameter("PageIndex", DbType.Int32, metaModel.PageIndex);
            db.AddInParameter("SportType", DbType.Int32, metaModel.SportType);
            db.AddOutParameter("SUMWL", DbType.Currency, 8);
            db.AddOutParameter("SUMLicenseeWL", DbType.Currency, 8);
            db.AddOutParameter("SUMWLUSD", DbType.Currency, 8);
            db.AddOutParameter("SUMLicenseeWLUSD", DbType.Currency, 8);
            db.AddOutParameter("SUMWLEUR", DbType.Currency, 8);
            db.AddOutParameter("SUMLicenseeWLEUR", DbType.Currency, 8);
            db.AddOutParameter("SUMWLGBP", DbType.Currency, 8);
            db.AddOutParameter("SUMLicenseeWLGBP", DbType.Currency, 8);
        }

        private static void CreateLiveReportParlayBetlistParameters(IObjectDb db, BetListMetaModel metaModel)
        {
            CreateBetListSortingParameters(db, metaModel);
            db.AddInParameter("AgentList", DbType.String, metaModel.AgentListXml);
            CreateBetListDateRangeParameters(db, metaModel);
            db.AddInParameter("MoveBetsDay", DbType.DateTime, metaModel.MoveBetsDay);
            db.AddInParameter("PageSize", DbType.Int32, metaModel.PageSize);
            db.AddInParameter("PageIndex", DbType.Int32, metaModel.PageIndex);
            var suportProducts = new string[2] { Product.SB, Product.BA };
            var products = metaModel.ProductOptions.Where(i => suportProducts.Contains(i.Code));

            foreach (var product in products)
            {
                db.AddInParameter("show" + product.Code, DbType.Boolean, product.Status);
            }

            db.AddInParameter("Bettype", DbType.Int32, metaModel.BetType);
            db.AddInParameter("LiveIndicator", DbType.Int32, metaModel.LiveIndicator);
        }

        private static void CreaeteLiveReportNumberGameBetlistParameters(IObjectDb db, BetListMetaModel metaModel)
        {
            CreateBetListSortingParameters(db, metaModel);
            db.AddInParameter("AgentList", DbType.String, metaModel.AgentListXml);
            CreateBetListDateRangeParameters(db, metaModel);
            db.AddInParameter("MoveBetsDay", DbType.DateTime, metaModel.MoveBetsDay);
            db.AddInParameter("PageSize", DbType.Int32, metaModel.PageSize);
            db.AddInParameter("PageIndex", DbType.Int32, metaModel.PageIndex);
            db.AddInParameter("LiveIndicator", DbType.Int32, metaModel.LiveIndicator);
        }

        private static void CreateLiveReportBetListByMatchParameters(IObjectDb db, BetListMetaModel metaModel)
        {
            db.AddInParameter("AgentList", DbType.String, metaModel.AgentListXml);
            db.AddInParameter("MatchID", DbType.Int32, metaModel.MatchId);
            db.AddInParameter("PageSize", DbType.Int32, metaModel.PageSize);
            db.AddInParameter("PageIndex", DbType.Int32, metaModel.PageIndex);
            db.AddInParameter("LiveIndicator", DbType.Int32, metaModel.LiveIndicator);
            var suportProducts = new string[6] { Product.SB, Product.BA, Product.LC, Product.VS, Product.MGCLC, Product.MGCRNG };
            var products = metaModel.ProductOptions.Where(i => suportProducts.Contains(i.Code));
            foreach (var product in products)
            {
                db.AddInParameter("show" + product.Code, DbType.Boolean, product.Status);
            }
        }

        private static void CreateLiveReportOutRightBetlistParameters(IObjectDb db, BetListMetaModel metaModel)
        {
            db.AddInParameter("AgentList", DbType.String, metaModel.AgentListXml);
            CreateBetListDateRangeParameters(db, metaModel);
            db.AddInParameter("MoveBetsDay", DbType.DateTime, metaModel.MoveBetsDay);
            db.AddInParameter("PageSize", DbType.Int32, metaModel.PageSize);
            db.AddInParameter("PageIndex", DbType.Int32, metaModel.PageIndex);
            var suportProducts = new string[2] { Product.SB, Product.BA };
            var products = metaModel.ProductOptions.Where(i => suportProducts.Contains(i.Code));
            foreach (var product in products)
            {
                db.AddInParameter("show" + product.Code, DbType.Boolean, product.Status);
            }
        }

        private static void CreateLicenseeWLBetlistParameters(IObjectDb db, BetListMetaModel metaModel)
        {
            db.AddInParameter("AgentList", DbType.String, metaModel.AgentListXml);
            db.AddInParameter("custid", DbType.Int32, metaModel.CustId);
            CreateBetListDateRangeParameters(db, metaModel);
            CreateBetlistBUParameters(db, metaModel);
            CreateBetlistProductOptionParameters(db, metaModel);
            CreateBetListPagingParameters(db, metaModel);
            CreateBetlistSummaryParameters(db);
            db.AddInParameter("drawfilter", DbType.Int16, metaModel.DrawFilter);
        }

        private static void CreateDailyWLBetlistParameters(IObjectDb db, BetListMetaModel metaModel)
        {
            CreateBetListSortingParameters(db, metaModel);

            db.AddInParameter("date", DbType.DateTime, metaModel.Date);
            db.AddInParameter("AgentList", DbType.String, metaModel.AgentListXml);
            db.AddInParameter("product", DbType.String, metaModel.ProductCode);
            db.AddInParameter("status", DbType.String, metaModel.DailyBetListStatus);
            CreateBetlistBUParameters(db, metaModel);
            CreateBetListPagingParameters(db, metaModel);
            CreateBetlistSummaryParameters(db);
        }

        private static void CreateDailyRacingWLBetlistParameters(IObjectDb db, BetListMetaModel metaModel)
        {
            db.AddInParameter("date", DbType.DateTime, metaModel.Date);
            db.AddInParameter("AgentList", DbType.String, metaModel.AgentListXml);
            db.AddInParameter("leagueid", DbType.Int32, metaModel.LeagueId);
            db.AddInParameter("raceNo", DbType.String, metaModel.RaceNo);
            db.AddInParameter("movebetsday", DbType.DateTime, metaModel.MoveBetsDay);
            CreateBetListPagingParameters(db, metaModel);
            CreateBetlistSummaryParameters(db);
            CreateBetListSummaryStakeParameters(db);
        }

        private static void CreateBetListSummaryStakeParameters(IObjectDb db)
        {
            db.AddOutParameter("SumStake", DbType.Currency, 8);
            db.AddOutParameter("SumStakeUSD", DbType.Currency, 8);
            db.AddOutParameter("SumStakeEUR", DbType.Currency, 8);
            db.AddOutParameter("SumStakeGBP", DbType.Currency, 8);
        }

        private static void CreateCorectScoreBetlistParameters(IObjectDb db, BetListMetaModel metaModel)
        {
            db.AddInParameter("AgentList", DbType.String, metaModel.AgentListXml);
            CreateBetListPagingParameters(db, metaModel);
        }

        private static void CreateCustomerWlBetlistParameters(IObjectDb db, BetListMetaModel metaModel)
        {
            db.AddInParameter("CustID", DbType.Int32, metaModel.CustId);
            db.AddInParameter("AgentList", DbType.String, metaModel.AgentListXml);
            CreateBetListDateRangeParameters(db, metaModel);
            CreateBetlistBUParameters(db, metaModel);
            CreateBetListPagingParameters(db, metaModel);
            CreateBetlistSummaryParameters(db);
            CreateBetlistProductOptionParameters(db, metaModel);
        }

        private static void CreateRacingCommissionBetlistParameters(IObjectDb db, BetListMetaModel metaModel)
        {
            db.AddInParameter("CustID", DbType.Int32, metaModel.CustId);
            db.AddInParameter("AgentList", DbType.String, metaModel.AgentListXml);
            CreateBetListDateRangeParameters(db, metaModel);
            CreateBetlistBUParameters(db, metaModel);
            CreateBetListPagingParameters(db, metaModel);
            CreateBetlistSummaryParameters(db);
        }

        private static void CreateCancelBetListParameters(IObjectDb db, BetListMetaModel metaModel)
        {
            db.AddInParameter("CustID", DbType.Int32, metaModel.CustId);
            db.AddInParameter("AgentList", DbType.String, metaModel.AgentListXml);
            CreateBetListPagingParameters(db, metaModel);
        }

        private static void CreateRunningBetListByBettypePaprameters(IObjectDb db, BetListMetaModel metaModel)
        {
            db.AddInParameter("AgentList", DbType.String, metaModel.AgentListXml);
            db.AddInParameter("bettype", DbType.Int32, metaModel.BetType);
            CreateBetListPagingParameters(db, metaModel);
        }

        private static void CreateRunningBetListByMultipleBettypePaprameters(IObjectDb db, BetListMetaModel metaModel)
        {
            db.AddInParameter("AgentList", DbType.String, metaModel.AgentListXml);
            db.AddInParameter("bettype", DbType.String, metaModel.MultipleBettype);
            CreateBetListPagingParameters(db, metaModel);
        }

        private static void CreateMicroGamingRngDetailParameters(IObjectDb db, BetListMetaModel metaModel)
        {
            db.AddInParameter("AgentList", DbType.String, metaModel.AgentListXml);
            db.AddInParameter("refno", DbType.Int64, metaModel.TransId);
            db.AddInParameter("custid", DbType.Int32, metaModel.CustId);
            db.AddInParameter("transtype", DbType.Int16, metaModel.TransType);
        }

        private static void CreateAGCasinoDetailParameters(IObjectDb db, BetListMetaModel metaModel)
        {
            db.AddInParameter("AgentList", DbType.String, metaModel.AgentListXml);
            db.AddInParameter("refno", DbType.Int64, metaModel.TransId);
            db.AddInParameter("custid", DbType.Int32, metaModel.CustId);
            db.AddInParameter("transtype", DbType.Int16, metaModel.TransType);
        }

        private static void CreateWinLossByBetTypeParameters(IObjectDb db, BetListMetaModel metaModel)
        {
            CreateBetListSortingParameters(db, metaModel);
            db.AddInParameter("AgentList", DbType.String, metaModel.AgentListXml);
            db.AddInParameter("bettype", DbType.Int32, metaModel.BetType);
            db.AddInParameter("fromdate", DbType.DateTime, metaModel.FromDate);
            db.AddInParameter("todate", DbType.DateTime, metaModel.ToDate);
            db.AddInParameter("movebetsday", DbType.DateTime, metaModel.MoveBetsDay);
            db.AddInParameter("balanceupday", DbType.DateTime, metaModel.BalanceUpDay);
            CreateBetListPagingParameters(db, metaModel);
            CreateBetlistSummaryParameters(db);
        }

        private static void CreateBetListSortingParameters(IObjectDb db, BetListMetaModel metaModel)
        {
            if (string.IsNullOrWhiteSpace(metaModel.SortBy) == false)
            {
                var nonBaseCurrencyColumns = new string[] { "nickname", "username" };
                var sortBy = nonBaseCurrencyColumns.Contains(metaModel.SortBy.ToLowerInvariant()) ?
                    metaModel.SortBy : metaModel.SortBy + metaModel.BaseCurrency;
                db.AddInParameter("Sort", DbType.String, sortBy + " " + metaModel.SortOrder);
            }
        }

        private static void CreateBetListErrorParameters(IObjectDb db)
        {
            db.AddOutParameter("ErrCode", DbType.Int32, 4);
            db.AddOutParameter("ErrMsg", DbType.String, 8000);
        }

        private static void CreateBetlistSummaryParameters(IObjectDb db)
        {
            db.AddOutParameter("SumWinlost", DbType.Currency, 8);
            db.AddOutParameter("SumLicenseeWL", DbType.Currency, 8);
            db.AddOutParameter("SumComm", DbType.Currency, 8);
            db.AddOutParameter("SumPlayerComm", DbType.Currency, 8);
            db.AddOutParameter("SumWinlostUSD", DbType.Currency, 8);
            db.AddOutParameter("SumLicenseeWLUSD", DbType.Currency, 8);
            db.AddOutParameter("SumCommUSD", DbType.Currency, 8);
            db.AddOutParameter("SumPlayerCommUSD", DbType.Currency, 8);
            db.AddOutParameter("SumWinlostEUR", DbType.Currency, 8);
            db.AddOutParameter("SumLicenseeWLEUR", DbType.Currency, 8);
            db.AddOutParameter("SumCommEUR", DbType.Currency, 8);
            db.AddOutParameter("SumPlayerCommEUR", DbType.Currency, 8);
            db.AddOutParameter("SumWinlostGBP", DbType.Currency, 8);
            db.AddOutParameter("SumLicenseeWLGBP", DbType.Currency, 8);
            db.AddOutParameter("SumCommGBP", DbType.Currency, 8);
            db.AddOutParameter("SumPlayerCommGBP", DbType.Currency, 8);
        }

        private static void CreateBetlistProductOptionParameters(IObjectDb db, BetListMetaModel metaModel)
        {
            foreach (var productOption in metaModel.ProductOptions)
            {
                db.AddInParameter("show" + productOption.Code, DbType.Boolean, productOption.Status);
            }
        }
    }
}