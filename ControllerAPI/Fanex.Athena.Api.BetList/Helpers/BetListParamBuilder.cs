namespace Fanex.Athena.Api.Helpers
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using Model.BetList.MetaModel;
    using Model.Core.DataDomain;
    using Model.Core.DTO.Common;

    public static class BetListParamBuilder
    {
        private static readonly IEnumerable<string> SupportedBaseCurrencies = SystemConstants.BaseCurrencyList;

        private static readonly Dictionary<string, Action<Hashtable, BetListMetaModel>> SpNameActionMap =
            new Dictionary<string, Action<Hashtable, BetListMetaModel>>(StringComparer.OrdinalIgnoreCase)
        {
            { "Lrf_BetList_WinlostByLeague_Mem", (param, metalModel) => CreateWinlossByLeagueParameters(param, metalModel) },
            { "Lrf_BetList_SelDeleted", (param, metalModel) => CreateCancelBetListParameters(param, metalModel) },
            { "Lrf_TotalBets_Sel430", (param, metalModel) => CreateCorectScoreBetlistParameters(param, metalModel) },
            { "Lrf_BetList_SelWinLossByDate_Mem", (param, metalModel) => CreateCustomerWlBetlistParameters(param, metalModel) },
            { "Lrf_BetList_SelWinlossDaily", (param, metalModel) => CreateDailyWLBetlistParameters(param, metalModel) },
            { "Lrf_BetList_SelWinloss_Mem", (param, metalModel) => CreateLicenseeWLBetlistParameters(param, metalModel) },
            { "Lrf_BetList_SelRunningByBettype", (param, metalModel) => CreateRunningBetListByBettypePaprameters(param, metalModel) },
            { "Lrf_BetList_SelRunningByBettypeList", (param, metalModel) => CreateRunningBetListByMultipleBettypePaprameters(param, metalModel) },
            { "Lrf_BetList_LiveReport_SelWinlossByMatch", (param, metalModel) => CreateLiveReportBetListByMatchParameters(param, metalModel) },
            { "Lrf_BetList_LiveReport_SelWinlossOutright", (param, metalModel) => CreateLiveReportOutRightBetlistParameters(param, metalModel) },
            { "Lrf_BetList_LiveReport_SelWinlossParlay", (param, metalModel) => CreateLiveReportParlayBetlistParameters(param, metalModel) },
            { "Lrf_BetList_LiveReport_SelWinlossVHRacing", (param, metalModel) => CreateLiveReportBetListVHRacingParameters(param, metalModel) },
            { "Lrf_BetList_MatchWL_Bettype", (param, metalModel) => CreateMatchWLBettypeBetlistParameters(param, metalModel) },
            { "Lrf_BetList_MatchWL_Outright", (param, metalModel) => CreateMatchWLOutRightBetListParameters(param, metalModel) },
            { "Lrf_BetList_MatchWL_Parlay", (param, metalModel) => CreateMatchWLParlayBetListParameters(param, metalModel) },
            { "Lrf_BetList_PlatformWinLoss_Mem", (param, metalModel) => CreatePlatformWinLossBetListParameters(param, metalModel) },
            { "Lrf_BetList_SelByDayAll_Mem", (param, metalModel) => CreateDailyStatementBetListParameters(param, metalModel) },
            { "Lrf_BetList_SelByDayAll_Mem_Old", (param, metalModel) => CreateDailyStatementBetListOldParameters(param, metalModel) },
            { "Lrf_BetList_SelRunningByDate_Mem", (param, metalModel) => CreateRunningBetListParameters(param, metalModel) },
            { "Lrf_BetList_SelRunningByBettypeMatch", (param, metalModel) => CreateMatchRunningBetLitByBettypeParameters(param, metalModel) },
            { "Lrf_BetList_SelRunningByMultipleBettypeMatch", (param, metalModel) => CreateMatchRunningBetLitByMultipleBettypeOfMatchParameters(param, metalModel) },
            { "Lrf_BetList_Forecast_SelByMatch", (param, metalModel) => CreateRunningBetListByForeCastMatchParameters(param, metalModel) },
            { "Lrf_BetList_RNG_Detail", (param, metalModel) => CreateCasinoDetailParameters(param, metalModel) },
            { "Lrf_BetList_LC_Detail", (param, metalModel) => CreateCasinoDetailParameters(param, metalModel) },
            { "Lrf_BetList_SelWinLossByBetType", (param, metalModel) => CreateWinLossByBetTypeParameters(param, metalModel) }
        };

        private static readonly Dictionary<string, Action<Hashtable, BetListMetaModel>> NonBaseCurrencySpNameActionMap =
            new Dictionary<string, Action<Hashtable, BetListMetaModel>>(StringComparer.OrdinalIgnoreCase)
        {
            { "Lrf_BetList_CustomerList_Mem", (param, metalModel) => CreateCustomerListBetListParameters(param, metalModel) },
            { "Lrf_BetList_LicenseeBonus_Detail", (param, metalModel) => CreateBonusDetailParameters(param, metalModel) },
            { "Lrf_BetList_LiveReport_SelWinlossNumberGame", (param, metalModel) => CreateLiveReportNumberGameBetlistParameters(param, metalModel) }
        };

        public static bool HasBaseCurrencyOutputColumn(string uspKey)
        {
            return SpNameActionMap.ContainsKey(uspKey);
        }

        public static void CreateBetListParameters(Hashtable param, string uspKey, BetListMetaModel metaModel)
        {
            if (NonBaseCurrencySpNameActionMap.ContainsKey(uspKey))
            {
                NonBaseCurrencySpNameActionMap[uspKey](param, metaModel);
            }
            else
            {
                SpNameActionMap[uspKey](param, metaModel);
            }
        }

        public static void CreateBetListDateRangeParameters(Hashtable param, BetListMetaModel metaModel)
        {
            param.Add("FromDate", metaModel.FromDate);
            param.Add("ToDate", metaModel.ToDate);
        }

        public static void CreateBetlistBUParameters(Hashtable param, BetListMetaModel metaModel)
        {
            param.Add("MoveBetsDay", metaModel.MoveBetsDay);
            param.Add("BalanceUpDay", metaModel.BalanceUpDay);
        }

        public static void CreateBetListPagingParameters(Hashtable param, BetListMetaModel metaModel)
        {
            param.Add("PageSize", metaModel.PageSize);
            param.Add("PageIndex", metaModel.PageIndex);
            param.Add("totalrows", null);
        }

        private static void CreateMatchRunningBetLitByMultipleBettypeOfMatchParameters(Hashtable param, BetListMetaModel metaModel)
        {
            param.Add("AgentList", metaModel.AgentListXml);
            param.Add("bettype", !string.IsNullOrEmpty(metaModel.MultipleBettype) ? metaModel.MultipleBettype : string.Empty);
            param.Add("MatchID", metaModel.MatchId);
            param.Add("liveindicator", metaModel.LiveIndicator.HasValue ? metaModel.LiveIndicator.Value : -1);
            param.Add("betteam", metaModel.BetTeam);
            CreateBetListPagingParameters(param, metaModel);
        }

        private static void CreateWinlossByLeagueParameters(Hashtable param, BetListMetaModel betlistMetaModel)
        {
            var metaModel = betlistMetaModel as WinLossByLeagueBetListMetaModel;
            var products = metaModel.ProductCodeList.Split(',');

            param.Add("CustID", metaModel.CustId);
            param.Add("LeagueID", metaModel.LeagueId);
            param.Add("MatchId", metaModel.MatchId);
            param.Add("AgentList", metaModel.AgentListXml);
            param.Add("Odds", metaModel.Odds);
            param.Add("FromDate", metaModel.FromDate);
            param.Add("ToDate", metaModel.ToDate);
            param.Add("MoveBetsDay", metaModel.MoveBetsDay);
            param.Add("BalanceUpDay", metaModel.BalanceUpDay);
            param.Add("ShowWin", metaModel.ShowWin);
            param.Add("ShowLoss", metaModel.ShowLoss);
            param.Add("ShowDraw", metaModel.ShowDraw);
            param.Add("ShowSB", products.Contains(Product.SB));
            param.Add("ShowBA", products.Contains(Product.BA));
        }

        private static void CreateRunningBetListByForeCastMatchParameters(Hashtable param, BetListMetaModel metaModel)
        {
            param.Add("AgentList", metaModel.AgentListXml);
            param.Add("bettype", metaModel.BetType);
            param.Add("MatchID", metaModel.MatchId);
            param.Add("liveindicator", metaModel.LiveIndicator.HasValue ? metaModel.LiveIndicator.Value : -1);
            param.Add("betteam", metaModel.BetTeam);
            param.Add("BetChoice", metaModel.BetChoice);
            param.Add("Handicap", metaModel.Handicap);
            CreateBetListPagingParameters(param, metaModel);
        }

        private static void CreateMatchRunningBetLitByBettypeParameters(Hashtable param, BetListMetaModel metaModel)
        {
            param.Add("AgentList", metaModel.AgentListXml);
            param.Add("bettype", metaModel.BetType);
            param.Add("MatchID", metaModel.MatchId);
            param.Add("liveindicator", metaModel.LiveIndicator.HasValue ? metaModel.LiveIndicator.Value : -1);
            param.Add("betteam", metaModel.BetTeam);
            CreateBetListPagingParameters(param, metaModel);
        }
        
        private static void CreateRunningBetListParameters(Hashtable param, BetListMetaModel metaModel)
        {
            param.Add("custid", metaModel.CustId);
            param.Add("AgentList", metaModel.AgentListXml);
            CreateBetListDateRangeParameters(param, metaModel);
            CreateBetListPagingParameters(param, metaModel);
            CreateProductListParameters(param, metaModel.ProductCodeList);
        }

        private static void CreateDailyStatementBetListOldParameters(Hashtable param, BetListMetaModel metaModel)
        {
            param.Add("AgentList", metaModel.AgentListXml);
            param.Add("custid", metaModel.CustId);
            param.Add("fromdate", metaModel.FromDate);
            param.Add("transtype", metaModel.TransType);
            CreateBetListPagingParameters(param, metaModel);
            CreateSummaryOutputParameters(param);
        }

        private static void CreateDailyStatementBetListParameters(Hashtable param, BetListMetaModel metaModel)
        {
            param.Add("AgentList", metaModel.AgentListXml);
            param.Add("custid", metaModel.CustId);
            param.Add("fromdate", metaModel.FromDate);
            param.Add("transtype", metaModel.TransType);
            param.Add("MoveBetsDay", metaModel.MoveBetsDay);
            CreateBetListPagingParameters(param, metaModel);
            CreateSummaryOutputParameters(param);
            CreateBetListErrorParameters(param);
        }

        private static void CreateCustomerListBetListParameters(Hashtable param, BetListMetaModel metaModel)
        {
            param.Add("custid", metaModel.CustId);
            param.Add("AgentList", metaModel.AgentListXml);
            param.Add("BaseCurrency", metaModel.BaseCurrency);
            CreateBetListDateRangeParameters(param, metaModel);
            CreateBetListPagingParameters(param, metaModel);
            CreateSummaryOutputParameters(param, false);
            CreateProductListParameters(param, metaModel.ProductCodeList);
            CreateBetlistBUParameters(param, metaModel);
            if (!string.IsNullOrWhiteSpace(metaModel.SortBy))
            {
                param.Add("Sort", metaModel.SortBy + " " + metaModel.SortOrder);
            }
        }

        private static void CreatePlatformWinLossBetListParameters(Hashtable param, BetListMetaModel metaModel)
        {
            param.Add("custid", metaModel.CustId);
            param.Add("AgentList", metaModel.AgentListXml);
            param.Add("drawfilter", metaModel.DrawFilter);
            param.Add("PlatformType", metaModel.PlatformType);
            CreateBetListDateRangeParameters(param, metaModel);
            CreateBetlistBUParameters(param, metaModel);
            CreateProductListParameters(param, metaModel.ProductCodeList);
            CreateBetListPagingParameters(param, metaModel);
            CreateSummaryOutputParameters(param);
        }

        private static void CreateMatchWLParlayBetListParameters(Hashtable param, BetListMetaModel metaModel)
        {
            CreateBetListSortingParameters(param, metaModel);
            param.Add("AgentList", metaModel.AgentListXml);
            param.Add("bettype", metaModel.BetType);
            param.Add("MoveBetsDay", metaModel.MoveBetsDay);
            CreateBetListDateRangeParameters(param, metaModel);
            CreateBetListPagingParameters(param, metaModel);
            CreateSummaryOutputParameters(param);

            var supportedProducts = new[] { Product.SB, Product.NG, Product.BA, Product.VS, Product.VS2 };
            CreateProductListParameters(param, metaModel.ProductCodeList, supportedProducts);
        }

        private static void CreateMatchWLOutRightBetListParameters(Hashtable param, BetListMetaModel metaModel)
        {
            param.Add("AgentList", metaModel.AgentListXml);
            CreateBetListDateRangeParameters(param, metaModel);
            param.Add("MoveBetsDay", metaModel.MoveBetsDay);
            CreateBetListSortingParameters(param, metaModel);
            CreateBetListPagingParameters(param, metaModel);
            CreateSummaryOutputParameters(param);

            var supportedProducts = new[] { Product.SB, Product.BA };
            CreateProductListParameters(param, metaModel.ProductCodeList, supportedProducts);
        }

        private static void CreateMatchWLBettypeBetlistParameters(Hashtable param, BetListMetaModel metaModel)
        {
            param.Add("AgentList", metaModel.AgentListXml);
            param.Add("bettype", metaModel.BetType);
            param.Add("MatchID", metaModel.MatchId);
            param.Add("liveindicator", metaModel.LiveIndicator.HasValue ? metaModel.LiveIndicator.Value : -1);
            param.Add("Date", metaModel.Date);
            CreateBetListSortingParameters(param, metaModel);
            CreateBetListPagingParameters(param, metaModel);
            CreateSummaryOutputParameters(param);

            var supportedProducts = new[] { Product.SB, Product.NG, Product.BA, Product.VS, Product.VS2 };
            CreateProductListParameters(param, metaModel.ProductCodeList, supportedProducts);
        }

        private static void CreateLiveReportBetListVHRacingParameters(Hashtable param, BetListMetaModel betlistMetaModel)
        {
            var metaModel = betlistMetaModel as LiveReportBetListMetaModel;
            var defaultParams = new[] { "SUMWL", "SUMLicenseeWL" };

            CreateBetListSortingParameters(param, metaModel);
            param.Add("AgentList", metaModel.AgentListXml);
            CreateBetListDateRangeParameters(param, metaModel);
            param.Add("MoveBetsDay", metaModel.MoveBetsDay);
            AddLiveReportPagingParams(param, metaModel);
            param.Add("SportType", metaModel.SportType);

            var summaryParameters = GetOutputSummaryParametersWithBaseCurrency(defaultParams, SupportedBaseCurrencies);
            foreach (var name in summaryParameters)
            {
                param.Add(name, null);
            }
        }

        private static void CreateLiveReportParlayBetlistParameters(Hashtable param, BetListMetaModel betlistMetaModel)
        {
            var metaModel = betlistMetaModel as LiveReportBetListMetaModel;

            CreateBetListSortingParameters(param, metaModel);
            param.Add("AgentList", metaModel.AgentListXml);
            CreateBetListDateRangeParameters(param, metaModel);
            param.Add("MoveBetsDay", metaModel.MoveBetsDay);
            AddLiveReportPagingParams(param, metaModel);
            param.Add("Bettype", metaModel.BetType);
            param.Add("LiveIndicator", metaModel.LiveIndicator);
            var supportedProducts = new[] { Product.SB, Product.BA, Product.VS2 };
            CreateProductListParameters(param, metaModel.ProductCodeList, supportedProducts);
        }

        private static void CreateLiveReportNumberGameBetlistParameters(Hashtable param, BetListMetaModel betlistMetaModel)
        {
            var metaModel = betlistMetaModel as LiveReportBetListMetaModel;

            CreateBetListSortingParameters(param, metaModel);
            param.Add("AgentList", metaModel.AgentListXml);
            CreateBetListDateRangeParameters(param, metaModel);
            param.Add("MoveBetsDay", metaModel.MoveBetsDay);
            AddLiveReportPagingParams(param, metaModel);
            param.Add("LiveIndicator", metaModel.LiveIndicator);
            param.Add("SportType", metaModel.SportType);
            param.Add("BaseCurrency", metaModel.BaseCurrency);
        }

        private static void CreateLiveReportBetListByMatchParameters(Hashtable param, BetListMetaModel betlistMetaModel)
        {
            var metaModel = betlistMetaModel as LiveReportBetListMetaModel;

            param.Add("AgentList", metaModel.AgentListXml);
            param.Add("MatchID", metaModel.MatchId);
            AddLiveReportPagingParams(param, metaModel);
            param.Add("LiveIndicator", metaModel.LiveIndicator);
            var supportedProducts = new[] { Product.SB, Product.BA, Product.VS, Product.MGCLC, Product.MGCRNG, Product.VS2 };
            CreateProductListParameters(param, metaModel.ProductCodeList, supportedProducts);
        }

        private static void CreateLiveReportOutRightBetlistParameters(Hashtable param, BetListMetaModel betlistMetaModel)
        {
            var metaModel = betlistMetaModel as LiveReportBetListMetaModel;

            param.Add("AgentList", metaModel.AgentListXml);
            CreateBetListDateRangeParameters(param, metaModel);
            param.Add("MoveBetsDay", metaModel.MoveBetsDay);
            AddLiveReportPagingParams(param, metaModel);
            var supportedProducts = new[] { Product.SB, Product.BA };
            CreateProductListParameters(param, metaModel.ProductCodeList, supportedProducts);
        }

        private static void CreateLicenseeWLBetlistParameters(Hashtable param, BetListMetaModel metaModel)
        {
            param.Add("AgentList", metaModel.AgentListXml);
            param.Add("custid", metaModel.CustId);
            param.Add("drawfilter", metaModel.DrawFilter);
            CreateBetListDateRangeParameters(param, metaModel);
            CreateBetlistBUParameters(param, metaModel);
            CreateProductListParameters(param, metaModel.ProductCodeList);
            CreateBetListPagingParameters(param, metaModel);
            CreateSummaryOutputParameters(param);
        }

        private static void CreateDailyWLBetlistParameters(Hashtable param, BetListMetaModel betlistMetaModel)
        {
            var metaModel = betlistMetaModel as DailyBetListMetaModel;

            CreateBetListSortingParameters(param, metaModel);
            param.Add("date", metaModel.Date);
            param.Add("AgentList", metaModel.AgentListXml);
            param.Add("product", metaModel.ProductCode);
            param.Add("status", metaModel.DailyBetListStatus);
            CreateBetlistBUParameters(param, metaModel);
            CreateBetListPagingParameters(param, metaModel);
            CreateSummaryOutputParameters(param);
        }

        private static void CreateCorectScoreBetlistParameters(Hashtable param, BetListMetaModel metaModel)
        {
            param.Add("AgentList", metaModel.AgentListXml);
            CreateBetListPagingParameters(param, metaModel);
        }

        private static void CreateCustomerWlBetlistParameters(Hashtable param, BetListMetaModel metaModel)
        {
            param.Add("CustID", metaModel.CustId);
            param.Add("AgentList", metaModel.AgentListXml);
            CreateBetListDateRangeParameters(param, metaModel);
            CreateBetlistBUParameters(param, metaModel);
            CreateBetListPagingParameters(param, metaModel);
            CreateSummaryOutputParameters(param);
            CreateProductListParameters(param, metaModel.ProductCodeList);
        }

        private static void CreateCancelBetListParameters(Hashtable param, BetListMetaModel metaModel)
        {
            param.Add("CustID", metaModel.CustId);
            param.Add("AgentList", metaModel.AgentListXml);
            CreateBetListPagingParameters(param, metaModel);
        }

        private static void CreateRunningBetListByBettypePaprameters(Hashtable param, BetListMetaModel metaModel)
        {
            param.Add("AgentList", metaModel.AgentListXml);
            param.Add("bettype", metaModel.BetType);
            CreateBetListPagingParameters(param, metaModel);
        }

        private static void CreateRunningBetListByMultipleBettypePaprameters(Hashtable param, BetListMetaModel metaModel)
        {
            param.Add("AgentList", metaModel.AgentListXml);
            param.Add("bettype", metaModel.MultipleBettype);
            CreateBetListPagingParameters(param, metaModel);
        }

        private static void CreateCasinoDetailParameters(Hashtable param, BetListMetaModel metaModel)
        {
            param.Add("AgentList", metaModel.AgentListXml);
            param.Add("refno", metaModel.TransId);
            param.Add("custid", metaModel.CustId);
            param.Add("transtype", metaModel.TransType);
        }

        private static void CreateWinLossByBetTypeParameters(Hashtable param, BetListMetaModel metaModel)
        {
            CreateBetListSortingParameters(param, metaModel);
            param.Add("AgentList", metaModel.AgentListXml);
            param.Add("bettype", metaModel.BetType);
            CreateBetListDateRangeParameters(param, metaModel);
            CreateBetlistBUParameters(param, metaModel);
            CreateBetListPagingParameters(param, metaModel);
            CreateSummaryOutputParameters(param);
        }

        private static void CreateBonusDetailParameters(Hashtable param, BetListMetaModel metaModel)
        {
            param.Add("AgentList", metaModel.AgentListXml);
            param.Add("TransId", metaModel.TransId);
            param.Add("RefCode", metaModel.RefCode);
            param.Add("BaseCurrency", metaModel.BaseCurrency);
        }

        #region Utility

        private static void AddLiveReportPagingParams(Hashtable param, LiveReportBetListMetaModel metaModel)
        {
            param.Add("PageSize", metaModel.IsExportExcel ? int.MaxValue - 1 : metaModel.PageSize);
            param.Add("PageIndex", metaModel.IsExportExcel ? 1 : metaModel.PageIndex);
        }

        private static void CreateBetListSortingParameters(Hashtable param, BetListMetaModel metaModel)
        {
            if (!string.IsNullOrWhiteSpace(metaModel.SortBy))
            {
                var nonBaseCurrencyColumns = new[] { "nickname", "username" };
                var sortBy = nonBaseCurrencyColumns.Contains(metaModel.SortBy.ToLowerInvariant()) ?
                    metaModel.SortBy : metaModel.SortBy + metaModel.BaseCurrency;
                param.Add("Sort", sortBy + " " + metaModel.SortOrder);
            }
        }

        private static void CreateBetListErrorParameters(Hashtable param)
        {
            param.Add("ErrCode", null);
            param.Add("ErrMsg", null);
        }

        private static void CreateSummaryOutputParameters(Hashtable param, bool hasBaseCurrency = true)
        {
            var defaultParams = new[] { "SumWinlost", "SumLicenseeWL", "SumComm", "SumPlayerComm" };

            var summaryParameters = hasBaseCurrency ?
                GetOutputSummaryParametersWithBaseCurrency(defaultParams, SupportedBaseCurrencies) :
                defaultParams;

            foreach (var name in summaryParameters)
            {
                param.Add(name, null);
            }
        }

        private static IEnumerable<string> GetOutputSummaryParametersWithBaseCurrency(IEnumerable<string> parameters, IEnumerable<string> baseCurrencyCols)
            => baseCurrencyCols.Join(parameters, b => true, d => true, (b, d) => d + b.ToUpperInvariant()).Concat(parameters);

        private static void CreateProductListParameters(Hashtable param, string productCodeList, IEnumerable<string> supportedProducts = null)
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
                param.Add("show" + productCode, true);
            }
        }

        #endregion Utility
    }
}