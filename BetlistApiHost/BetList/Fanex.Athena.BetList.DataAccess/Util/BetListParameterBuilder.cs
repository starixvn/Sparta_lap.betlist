namespace Fanex.Athena.BetList.DataAccess.Util
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Athena.Model.Core.DataDomain;
    using Athena.Model.Core.DTO.Common;
    using BetList.Model.MetaModel;
    using Data;

    [CLSCompliant(false)]
    public static class BetListParameterBuilder
    {
        private static readonly IEnumerable<string> SupportedBaseCurrencies = SystemConstants.BaseCurrencyList;

        private static readonly Dictionary<string, Action<IObjectDb, BetListMetaModel>> SpNameActionMap =
            new Dictionary<string, Action<IObjectDb, BetListMetaModel>>(StringComparer.OrdinalIgnoreCase)
        {
            { "Lrf_BetList_WinlostByLeague_Mem", (db, metalModel) => CreateWinlossByLeagueParameters(db, metalModel) },
            { "Lrf_BetList_SelDeleted", (db, metalModel) => CreateCancelBetListParameters(db, metalModel) },
            { "Lrf_TotalBets_Sel430", (db, metalModel) => CreateCorectScoreBetlistParameters(db, metalModel) },
            { "Lrf_BetList_SelWinLossByDate_Mem", (db, metalModel) => CreateCustomerWlBetlistParameters(db, metalModel) },
            { "Lrf_BetList_SelWinlossDaily", (db, metalModel) => CreateDailyWLBetlistParameters(db, metalModel) },
            { "Lrf_BetList_SelWinloss_Mem", (db, metalModel) => CreateLicenseeWLBetlistParameters(db, metalModel) },
            { "Lrf_BetList_SelRunningByBettype", (db, metalModel) => CreateRunningBetListByBettypePaprameters(db, metalModel) },
            { "Lrf_BetList_SelRunningByBettypeList", (db, metalModel) => CreateRunningBetListByMultipleBettypePaprameters(db, metalModel) },
            { "Lrf_BetList_LiveReport_SelWinlossByMatch", (db, metalModel) => CreateLiveReportBetListByMatchParameters(db, metalModel) },
            { "Lrf_BetList_LiveReport_SelWinlossOutright", (db, metalModel) => CreateLiveReportOutRightBetlistParameters(db, metalModel) },
            { "Lrf_BetList_LiveReport_SelWinlossParlay", (db, metalModel) => CreateLiveReportParlayBetlistParameters(db, metalModel) },
            { "Lrf_BetList_LiveReport_SelWinlossVHRacing", (db, metalModel) => CreateLiveReportBetListVHRacingParameters(db, metalModel) },
            { "Lrf_BetList_MatchWL_Bettype", (db, metalModel) => CreateMatchWLBettypeBetlistParameters(db, metalModel) },
            { "Lrf_BetList_MatchWL_Outright", (db, metalModel) => CreateMatchWLOutRightBetListParameters(db, metalModel) },
            { "Lrf_BetList_MatchWL_Parlay", (db, metalModel) => CreateMatchWLParlayBetListParameters(db, metalModel) },
            { "Lrf_BetList_PlatformWinLoss_Mem", (db, metalModel) => CreatePlatformWinLossBetListParameters(db, metalModel) },
            { "Lrf_BetList_SelByDayAll_Mem", (db, metalModel) => CreateDailyStatementBetListParameters(db, metalModel) },
            { "Lrf_BetList_SelByDayAll_Mem_Old", (db, metalModel) => CreateDailyStatementBetListOldParameters(db, metalModel) },
            { "Lrf_BetList_SelRunningByDate_Mem", (db, metalModel) => CreateRunningBetListParameters(db, metalModel) },
            { "Lrf_BetList_SelRunningByBettypeMatch", (db, metalModel) => CreateMatchRunningBetLitByBettypeParameters(db, metalModel) },
            { "Lrf_BetList_SelRunningByMultipleBettypeMatch", (db, metalModel) => CreateMatchRunningBetLitByMultipleBettypeOfMatchParameters(db, metalModel) },
            { "Lrf_BetList_Forecast_SelByMatch", (db, metalModel) => CreateRunningBetListByForeCastMatchParameters(db, metalModel) },
            { "Lrf_BetList_RNG_Detail", (db, metalModel) => CreateCasinoDetailParameters(db, metalModel) },
            { "Lrf_BetList_LC_Detail", (db, metalModel) => CreateCasinoDetailParameters(db, metalModel) },
            { "Lrf_BetList_SelWinLossByBetType", (db, metalModel) => CreateWinLossByBetTypeParameters(db, metalModel) }
        };

        private static readonly Dictionary<string, Action<IObjectDb, BetListMetaModel>> NonBaseCurrencySpNameActionMap =
            new Dictionary<string, Action<IObjectDb, BetListMetaModel>>(StringComparer.OrdinalIgnoreCase)
        {
            { "Lrf_BetList_CustomerList_Mem", (db, metalModel) => CreateCustomerListBetListParameters(db, metalModel) },
            { "Lrf_BetList_LicenseeBonus_Detail", (db, metalModel) => CreateBonusDetailParameters(db, metalModel) },
            { "Lrf_BetList_LiveReport_SelWinlossNumberGame", (db, metalModel) => CreateLiveReportNumberGameBetlistParameters(db, metalModel) }
        };

        public static bool HasBaseCurrencyOutputColumn(string uspKey)
        {
            return SpNameActionMap.ContainsKey(uspKey);
        }

        public static void CreateBetListParameters(IObjectDb db, string uspKey, BetListMetaModel metaModel)
        {
            if (NonBaseCurrencySpNameActionMap.ContainsKey(uspKey))
            {
                NonBaseCurrencySpNameActionMap[uspKey](db, metaModel);
            }
            else
            {
                SpNameActionMap[uspKey](db, metaModel);
            }
        }

        public static void CreateBetListDateRangeParameters(IObjectDb db, BetListMetaModel metaModel)
        {
            db.SetParameterValue("FromDate", metaModel.FromDate);
            db.SetParameterValue("ToDate", metaModel.ToDate);
        }

        public static void CreateBetlistBUParameters(IObjectDb db, BetListMetaModel metaModel)
        {
            db.SetParameterValue("MoveBetsDay", metaModel.MoveBetsDay);
            db.SetParameterValue("BalanceUpDay", metaModel.BalanceUpDay);
        }

        public static void CreateBetListPagingParameters(IObjectDb db, BetListMetaModel metaModel)
        {
            db.SetParameterValue("PageSize", metaModel.PageSize);
            db.SetParameterValue("PageIndex", metaModel.PageIndex);
            db.SetParameterValue("totalrows", null);
        }

        private static void CreateMatchRunningBetLitByMultipleBettypeOfMatchParameters(IObjectDb db, BetListMetaModel metaModel)
        {
            db.SetParameterValue("AgentList", metaModel.AgentListXml);
            db.SetParameterValue("bettype", !string.IsNullOrEmpty(metaModel.MultipleBettype) ? metaModel.MultipleBettype : string.Empty);
            db.SetParameterValue("MatchID", metaModel.MatchId);
            db.SetParameterValue("liveindicator", metaModel.LiveIndicator.HasValue ? metaModel.LiveIndicator.Value : -1);
            db.SetParameterValue("betteam", metaModel.BetTeam);
            CreateBetListPagingParameters(db, metaModel);
        }

        private static void CreateWinlossByLeagueParameters(IObjectDb db, BetListMetaModel betlistMetaModel)
        {
            var metaModel = betlistMetaModel as WinLossByLeagueBetListMetaModel;
            var products = metaModel.ProductCodeList.Split(',');

            db.SetParameterValue("CustID", metaModel.CustId);
            db.SetParameterValue("LeagueID", metaModel.LeagueId);
            db.SetParameterValue("MatchId", metaModel.MatchId);
            db.SetParameterValue("AgentList", metaModel.AgentListXml);
            db.SetParameterValue("Odds", metaModel.Odds);
            db.SetParameterValue("FromDate", metaModel.FromDate);
            db.SetParameterValue("ToDate", metaModel.ToDate);
            db.SetParameterValue("MoveBetsDay", metaModel.MoveBetsDay);
            db.SetParameterValue("BalanceUpDay", metaModel.BalanceUpDay);
            db.SetParameterValue("ShowWin", metaModel.ShowWin);
            db.SetParameterValue("ShowLoss", metaModel.ShowLoss);
            db.SetParameterValue("ShowDraw", metaModel.ShowDraw);
            db.SetParameterValue("ShowSB", products.Contains(Product.SB));
            db.SetParameterValue("ShowBA", products.Contains(Product.BA));
        }

        private static void CreateRunningBetListByForeCastMatchParameters(IObjectDb db, BetListMetaModel metaModel)
        {
            db.SetParameterValue("AgentList", metaModel.AgentListXml);
            db.SetParameterValue("bettype", metaModel.BetType);
            db.SetParameterValue("MatchID", metaModel.MatchId);
            db.SetParameterValue("liveindicator", metaModel.LiveIndicator.HasValue ? metaModel.LiveIndicator.Value : -1);
            db.SetParameterValue("betteam", metaModel.BetTeam);
            db.SetParameterValue("BetChoice", metaModel.BetChoice);
            db.SetParameterValue("Handicap", metaModel.Handicap);
            CreateBetListPagingParameters(db, metaModel);
        }

        private static void CreateMatchRunningBetLitByBettypeParameters(IObjectDb db, BetListMetaModel metaModel)
        {
            db.SetParameterValue("AgentList", metaModel.AgentListXml);
            db.SetParameterValue("bettype", metaModel.BetType);
            db.SetParameterValue("MatchID", metaModel.MatchId);
            db.SetParameterValue("liveindicator", metaModel.LiveIndicator.HasValue ? metaModel.LiveIndicator.Value : -1);
            db.SetParameterValue("betteam", metaModel.BetTeam);
            CreateBetListPagingParameters(db, metaModel);
        }

        private static void CreateRunningBetListParameters(IObjectDb db, BetListMetaModel metaModel)
        {
            db.SetParameterValue("custid", metaModel.CustId);
            db.SetParameterValue("AgentList", metaModel.AgentListXml);
            CreateBetListDateRangeParameters(db, metaModel);
            CreateBetListPagingParameters(db, metaModel);
            CreateProductListParameters(db, metaModel.ProductCodeList);
        }

        private static void CreateDailyStatementBetListOldParameters(IObjectDb db, BetListMetaModel metaModel)
        {
            db.SetParameterValue("AgentList", metaModel.AgentListXml);
            db.SetParameterValue("custid", metaModel.CustId);
            db.SetParameterValue("fromdate", metaModel.FromDate);
            db.SetParameterValue("transtype", metaModel.TransType);
            CreateBetListPagingParameters(db, metaModel);
            CreateSummaryOutputParameters(db);
        }

        private static void CreateDailyStatementBetListParameters(IObjectDb db, BetListMetaModel metaModel)
        {
            db.SetParameterValue("AgentList", metaModel.AgentListXml);
            db.SetParameterValue("custid", metaModel.CustId);
            db.SetParameterValue("fromdate", metaModel.FromDate);
            db.SetParameterValue("transtype", metaModel.TransType);
            db.SetParameterValue("MoveBetsDay", metaModel.MoveBetsDay);
            CreateBetListPagingParameters(db, metaModel);
            CreateSummaryOutputParameters(db);
            CreateBetListErrorParameters(db);
        }

        private static void CreateCustomerListBetListParameters(IObjectDb db, BetListMetaModel metaModel)
        {
            db.SetParameterValue("custid", metaModel.CustId);
            db.SetParameterValue("AgentList", metaModel.AgentListXml);
            db.SetParameterValue("BaseCurrency", metaModel.BaseCurrency);
            CreateBetListDateRangeParameters(db, metaModel);
            CreateBetListPagingParameters(db, metaModel);
            CreateSummaryOutputParameters(db, false);
            CreateProductListParameters(db, metaModel.ProductCodeList);
            CreateBetlistBUParameters(db, metaModel);
            if (!string.IsNullOrWhiteSpace(metaModel.SortBy))
            {
                db.SetParameterValue("Sort", metaModel.SortBy + " " + metaModel.SortOrder);
            }
        }

        private static void CreatePlatformWinLossBetListParameters(IObjectDb db, BetListMetaModel metaModel)
        {
            db.SetParameterValue("custid", metaModel.CustId);
            db.SetParameterValue("AgentList", metaModel.AgentListXml);
            db.SetParameterValue("drawfilter", metaModel.DrawFilter);
            db.SetParameterValue("PlatformType", metaModel.PlatformType);
            CreateBetListDateRangeParameters(db, metaModel);
            CreateBetlistBUParameters(db, metaModel);
            CreateProductListParameters(db, metaModel.ProductCodeList);
            CreateBetListPagingParameters(db, metaModel);
            CreateSummaryOutputParameters(db);
        }

        private static void CreateMatchWLParlayBetListParameters(IObjectDb db, BetListMetaModel metaModel)
        {
            CreateBetListSortingParameters(db, metaModel);
            db.SetParameterValue("AgentList", metaModel.AgentListXml);
            db.SetParameterValue("bettype", metaModel.BetType);
            db.SetParameterValue("MoveBetsDay", metaModel.MoveBetsDay);
            CreateBetListDateRangeParameters(db, metaModel);
            CreateBetListPagingParameters(db, metaModel);
            CreateSummaryOutputParameters(db);

            var supportedProducts = new[] { Product.SB, Product.NG, Product.BA, Product.VS, Product.VS2 };
            CreateProductListParameters(db, metaModel.ProductCodeList, supportedProducts);
        }

        private static void CreateMatchWLOutRightBetListParameters(IObjectDb db, BetListMetaModel metaModel)
        {
            db.SetParameterValue("AgentList", metaModel.AgentListXml);
            CreateBetListDateRangeParameters(db, metaModel);
            db.SetParameterValue("MoveBetsDay", metaModel.MoveBetsDay);
            CreateBetListSortingParameters(db, metaModel);
            CreateBetListPagingParameters(db, metaModel);
            CreateSummaryOutputParameters(db);

            var supportedProducts = new[] { Product.SB, Product.BA };
            CreateProductListParameters(db, metaModel.ProductCodeList, supportedProducts);
        }

        private static void CreateMatchWLBettypeBetlistParameters(IObjectDb db, BetListMetaModel metaModel)
        {
            db.SetParameterValue("AgentList", metaModel.AgentListXml);
            db.SetParameterValue("bettype", metaModel.BetType);
            db.SetParameterValue("MatchID", metaModel.MatchId);
            db.SetParameterValue("liveindicator", metaModel.LiveIndicator.HasValue ? metaModel.LiveIndicator.Value : -1);
            db.SetParameterValue("Date", metaModel.Date);
            CreateBetListSortingParameters(db, metaModel);
            CreateBetListPagingParameters(db, metaModel);
            CreateSummaryOutputParameters(db);

            var supportedProducts = new[] { Product.SB, Product.NG, Product.BA, Product.VS, Product.VS2 };
            CreateProductListParameters(db, metaModel.ProductCodeList, supportedProducts);
        }

        private static void CreateLiveReportBetListVHRacingParameters(IObjectDb db, BetListMetaModel betlistMetaModel)
        {
            var metaModel = betlistMetaModel as LiveReportBetListMetaModel;
            var defaultParams = new[] { "SUMWL", "SUMLicenseeWL" };

            CreateBetListSortingParameters(db, metaModel);
            db.SetParameterValue("AgentList", metaModel.AgentListXml);
            CreateBetListDateRangeParameters(db, metaModel);
            db.SetParameterValue("MoveBetsDay", metaModel.MoveBetsDay);
            AddLiveReportPagingParams(db, metaModel);
            db.SetParameterValue("SportType", metaModel.SportType);

            var summaryParameters = GetOutputSummaryParametersWithBaseCurrency(defaultParams, SupportedBaseCurrencies);
            foreach (var name in summaryParameters)
            {
                db.SetParameterValue(name, null);
            }
        }

        private static void CreateLiveReportParlayBetlistParameters(IObjectDb db, BetListMetaModel betlistMetaModel)
        {
            var metaModel = betlistMetaModel as LiveReportBetListMetaModel;

            CreateBetListSortingParameters(db, metaModel);
            db.SetParameterValue("AgentList", metaModel.AgentListXml);
            CreateBetListDateRangeParameters(db, metaModel);
            db.SetParameterValue("MoveBetsDay", metaModel.MoveBetsDay);
            AddLiveReportPagingParams(db, metaModel);
            db.SetParameterValue("Bettype", metaModel.BetType);
            db.SetParameterValue("LiveIndicator", metaModel.LiveIndicator);
            var supportedProducts = new[] { Product.SB, Product.BA, Product.VS2 };
            CreateProductListParameters(db, metaModel.ProductCodeList, supportedProducts);
        }

        private static void CreateLiveReportNumberGameBetlistParameters(IObjectDb db, BetListMetaModel betlistMetaModel)
        {
            var metaModel = betlistMetaModel as LiveReportBetListMetaModel;

            CreateBetListSortingParameters(db, metaModel);
            db.SetParameterValue("AgentList", metaModel.AgentListXml);
            CreateBetListDateRangeParameters(db, metaModel);
            db.SetParameterValue("MoveBetsDay", metaModel.MoveBetsDay);
            AddLiveReportPagingParams(db, metaModel);
            db.SetParameterValue("LiveIndicator", metaModel.LiveIndicator);
            db.SetParameterValue("SportType", metaModel.SportType);
            db.SetParameterValue("BaseCurrency", metaModel.BaseCurrency);
        }

        private static void CreateLiveReportBetListByMatchParameters(IObjectDb db, BetListMetaModel betlistMetaModel)
        {
            var metaModel = betlistMetaModel as LiveReportBetListMetaModel;

            db.SetParameterValue("AgentList", metaModel.AgentListXml);
            db.SetParameterValue("MatchID", metaModel.MatchId);
            AddLiveReportPagingParams(db, metaModel);
            db.SetParameterValue("LiveIndicator", metaModel.LiveIndicator);
            var supportedProducts = new[] { Product.SB, Product.BA, Product.VS, Product.MGCLC, Product.MGCRNG, Product.VS2 };
            CreateProductListParameters(db, metaModel.ProductCodeList, supportedProducts);
        }

        private static void CreateLiveReportOutRightBetlistParameters(IObjectDb db, BetListMetaModel betlistMetaModel)
        {
            var metaModel = betlistMetaModel as LiveReportBetListMetaModel;

            db.SetParameterValue("AgentList", metaModel.AgentListXml);
            CreateBetListDateRangeParameters(db, metaModel);
            db.SetParameterValue("MoveBetsDay", metaModel.MoveBetsDay);
            AddLiveReportPagingParams(db, metaModel);
            var supportedProducts = new[] { Product.SB, Product.BA };
            CreateProductListParameters(db, metaModel.ProductCodeList, supportedProducts);
        }

        private static void CreateLicenseeWLBetlistParameters(IObjectDb db, BetListMetaModel metaModel)
        {
            db.SetParameterValue("AgentList", metaModel.AgentListXml);
            db.SetParameterValue("custid", metaModel.CustId);
            db.SetParameterValue("drawfilter", metaModel.DrawFilter);
            CreateBetListDateRangeParameters(db, metaModel);
            CreateBetlistBUParameters(db, metaModel);
            CreateProductListParameters(db, metaModel.ProductCodeList);
            CreateBetListPagingParameters(db, metaModel);
            CreateSummaryOutputParameters(db);
        }

        private static void CreateDailyWLBetlistParameters(IObjectDb db, BetListMetaModel betlistMetaModel)
        {
            var metaModel = betlistMetaModel as DailyBetListMetaModel;

            CreateBetListSortingParameters(db, metaModel);
            db.SetParameterValue("date", metaModel.Date);
            db.SetParameterValue("AgentList", metaModel.AgentListXml);
            db.SetParameterValue("product", metaModel.ProductCode);
            db.SetParameterValue("status", metaModel.DailyBetListStatus);
            CreateBetlistBUParameters(db, metaModel);
            CreateBetListPagingParameters(db, metaModel);
            CreateSummaryOutputParameters(db);
        }

        private static void CreateCorectScoreBetlistParameters(IObjectDb db, BetListMetaModel metaModel)
        {
            db.SetParameterValue("AgentList", metaModel.AgentListXml);
            CreateBetListPagingParameters(db, metaModel);
        }

        private static void CreateCustomerWlBetlistParameters(IObjectDb db, BetListMetaModel metaModel)
        {
            db.SetParameterValue("CustID", metaModel.CustId);
            db.SetParameterValue("AgentList", metaModel.AgentListXml);
            CreateBetListDateRangeParameters(db, metaModel);
            CreateBetlistBUParameters(db, metaModel);
            CreateBetListPagingParameters(db, metaModel);
            CreateSummaryOutputParameters(db);
            CreateProductListParameters(db, metaModel.ProductCodeList);
        }

        private static void CreateCancelBetListParameters(IObjectDb db, BetListMetaModel metaModel)
        {
            db.SetParameterValue("CustID", metaModel.CustId);
            db.SetParameterValue("AgentList", metaModel.AgentListXml);
            CreateBetListPagingParameters(db, metaModel);
        }

        private static void CreateRunningBetListByBettypePaprameters(IObjectDb db, BetListMetaModel metaModel)
        {
            db.SetParameterValue("AgentList", metaModel.AgentListXml);
            db.SetParameterValue("bettype", metaModel.BetType);
            CreateBetListPagingParameters(db, metaModel);
        }

        private static void CreateRunningBetListByMultipleBettypePaprameters(IObjectDb db, BetListMetaModel metaModel)
        {
            db.SetParameterValue("AgentList", metaModel.AgentListXml);
            db.SetParameterValue("bettype", metaModel.MultipleBettype);
            CreateBetListPagingParameters(db, metaModel);
        }

        private static void CreateCasinoDetailParameters(IObjectDb db, BetListMetaModel metaModel)
        {
            db.SetParameterValue("AgentList", metaModel.AgentListXml);
            db.SetParameterValue("refno", metaModel.TransId);
            db.SetParameterValue("custid", metaModel.CustId);
            db.SetParameterValue("transtype", metaModel.TransType);
        }

        private static void CreateWinLossByBetTypeParameters(IObjectDb db, BetListMetaModel metaModel)
        {
            CreateBetListSortingParameters(db, metaModel);
            db.SetParameterValue("AgentList", metaModel.AgentListXml);
            db.SetParameterValue("bettype", metaModel.BetType);
            CreateBetListDateRangeParameters(db, metaModel);
            CreateBetlistBUParameters(db, metaModel);
            CreateBetListPagingParameters(db, metaModel);
            CreateSummaryOutputParameters(db);
        }

        private static void CreateBonusDetailParameters(IObjectDb db, BetListMetaModel metaModel)
        {
            db.SetParameterValue("AgentList", metaModel.AgentListXml);
            db.SetParameterValue("TransId", metaModel.TransId);
            db.SetParameterValue("RefCode", metaModel.RefCode);
            db.SetParameterValue("BaseCurrency", metaModel.BaseCurrency);
        }

        #region Utility

        private static void AddLiveReportPagingParams(IObjectDb db, LiveReportBetListMetaModel metaModel)
        {
            db.SetParameterValue("PageSize", metaModel.IsExportExcel ? int.MaxValue - 1 : metaModel.PageSize);
            db.SetParameterValue("PageIndex", metaModel.IsExportExcel ? 1 : metaModel.PageIndex);
        }

        private static void CreateBetListSortingParameters(IObjectDb db, BetListMetaModel metaModel)
        {
            if (!string.IsNullOrWhiteSpace(metaModel.SortBy))
            {
                var nonBaseCurrencyColumns = new[] { "nickname", "username" };
                var sortBy = nonBaseCurrencyColumns.Contains(metaModel.SortBy.ToLowerInvariant()) ?
                    metaModel.SortBy : metaModel.SortBy + metaModel.BaseCurrency;
                db.SetParameterValue("Sort", sortBy + " " + metaModel.SortOrder);
            }
        }

        private static void CreateBetListErrorParameters(IObjectDb db)
        {
            db.SetParameterValue("ErrCode", null);
            db.SetParameterValue("ErrMsg", null);
        }

        private static void CreateSummaryOutputParameters(IObjectDb db, bool hasBaseCurrency = true)
        {
            var defaultParams = new[] { "SumWinlost", "SumLicenseeWL", "SumComm", "SumPlayerComm" };

            var summaryParameters = hasBaseCurrency ?
                GetOutputSummaryParametersWithBaseCurrency(defaultParams, SupportedBaseCurrencies) :
                defaultParams;

            foreach (var name in summaryParameters)
            {
                db.SetParameterValue(name, null);
            }
        }

        private static IEnumerable<string> GetOutputSummaryParametersWithBaseCurrency(IEnumerable<string> parameters, IEnumerable<string> baseCurrencyCols)
            => baseCurrencyCols.Join(parameters, b => true, d => true, (b, d) => d + b.ToUpperInvariant()).Concat(parameters);

        private static void CreateProductListParameters(IObjectDb db, string productCodeList, IEnumerable<string> supportedProducts = null)
        {
            if (string.IsNullOrWhiteSpace(productCodeList))
            {
                return;
            }

            IEnumerable<string> products = productCodeList.Split(',');

            if (supportedProducts != null)
            {
                products = products.Where(x => supportedProducts.Contains(x)).ToList();
            }

            foreach (var productCode in products)
            {
                db.SetParameterValue("show" + productCode, true);
            }
        }

        #endregion Utility
    }
}