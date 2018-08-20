namespace Fanex.Athena.DataAccess.BetList.Util
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using Data;
    using Model.BetList.MetaModel;
    using Model.Core.DataDomain;
    using Model.Core.DTO.Common;

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
            { "Lrf_BetList_LiveReport_SelWinlossNumberGame", (db, metalModel) => CreaeteLiveReportNumberGameBetlistParameters(db, metalModel) },
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
            { "Lrf_BetList_LicenseeBonus_Detail", (db, metalModel) => CreateBonusDetailParameters(db, metalModel) }
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
            db.AddInParameter("liveindicator", DbType.Int32, metaModel.LiveIndicator.HasValue ? metaModel.LiveIndicator.Value : -1);
            db.AddInParameter("betteam", DbType.String, metaModel.BetTeam);
            CreateBetListPagingParameters(db, metaModel);
        }

        private static void CreateWinlossByLeagueParameters(IObjectDb db, BetListMetaModel betlistMetaModel)
        {
            var metaModel = betlistMetaModel as WinLossByLeagueBetListMetaModel;
            var products = metaModel.ProductCodeList.Split(',');

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
            db.AddInParameter("ShowSB", DbType.Boolean, products.Contains(Product.SB));
            db.AddInParameter("ShowBA", DbType.Boolean, products.Contains(Product.BA));
        }

        private static void CreateRunningBetListByForeCastMatchParameters(IObjectDb db, BetListMetaModel metaModel)
        {
            db.AddInParameter("AgentList", DbType.String, metaModel.AgentListXml);
            db.AddInParameter("bettype", DbType.Int32, metaModel.BetType);
            db.AddInParameter("MatchID", DbType.Int32, metaModel.MatchId);
            db.AddInParameter("liveindicator", DbType.Int32, metaModel.LiveIndicator.HasValue ? metaModel.LiveIndicator.Value : -1);
            db.AddInParameter("betteam", DbType.String, metaModel.BetTeam);
            db.AddInParameter("BetChoice", DbType.String, metaModel.BetChoice);
            db.AddInParameter("Handicap", DbType.Decimal, metaModel.Handicap);
            CreateBetListPagingParameters(db, metaModel);
        }

        private static void CreateMatchRunningBetLitByBettypeParameters(IObjectDb db, BetListMetaModel metaModel)
        {
            db.AddInParameter("AgentList", DbType.String, metaModel.AgentListXml);
            db.AddInParameter("bettype", DbType.Int32, metaModel.BetType);
            db.AddInParameter("MatchID", DbType.Int32, metaModel.MatchId);
            db.AddInParameter("liveindicator", DbType.Int32, metaModel.LiveIndicator.HasValue ? metaModel.LiveIndicator.Value : -1);
            db.AddInParameter("betteam", DbType.String, metaModel.BetTeam);
            CreateBetListPagingParameters(db, metaModel);
        }
        
        private static void CreateRunningBetListParameters(IObjectDb db, BetListMetaModel metaModel)
        {
            db.AddInParameter("custid", DbType.Int32, metaModel.CustId);
            db.AddInParameter("AgentList", DbType.String, metaModel.AgentListXml);
            CreateBetListDateRangeParameters(db, metaModel);
            CreateBetListPagingParameters(db, metaModel);
            CreateProductListParameters(db, metaModel.ProductCodeList);
        }

        private static void CreateDailyStatementBetListOldParameters(IObjectDb db, BetListMetaModel metaModel)
        {
            db.AddInParameter("AgentList", DbType.String, metaModel.AgentListXml);
            db.AddInParameter("custid", DbType.Int32, metaModel.CustId);
            db.AddInParameter("fromdate", DbType.DateTime, metaModel.FromDate);
            db.AddInParameter("transtype", DbType.Int32, metaModel.TransType);
            CreateBetListPagingParameters(db, metaModel);
            CreateSummaryOutputParameters(db);
        }

        private static void CreateDailyStatementBetListParameters(IObjectDb db, BetListMetaModel metaModel)
        {
            db.AddInParameter("AgentList", DbType.String, metaModel.AgentListXml);
            db.AddInParameter("custid", DbType.Int32, metaModel.CustId);
            db.AddInParameter("fromdate", DbType.DateTime, metaModel.FromDate);
            db.AddInParameter("transtype", DbType.Int32, metaModel.TransType);
            db.AddInParameter("MoveBetsDay", DbType.DateTime, metaModel.MoveBetsDay);
            CreateBetListPagingParameters(db, metaModel);
            CreateSummaryOutputParameters(db);
            CreateBetListErrorParameters(db);
        }

        private static void CreateCustomerListBetListParameters(IObjectDb db, BetListMetaModel metaModel)
        {
            db.AddInParameter("custid", DbType.Int32, metaModel.CustId);
            db.AddInParameter("AgentList", DbType.String, metaModel.AgentListXml);
            db.AddInParameter("BaseCurrency", DbType.String, metaModel.BaseCurrency);
            CreateBetListDateRangeParameters(db, metaModel);
            CreateBetListPagingParameters(db, metaModel);
            CreateSummaryOutputParameters(db, false);
            CreateProductListParameters(db, metaModel.ProductCodeList);
            CreateBetlistBUParameters(db, metaModel);
            if (!string.IsNullOrWhiteSpace(metaModel.SortBy))
            {
                db.AddInParameter("Sort", DbType.String, metaModel.SortBy + " " + metaModel.SortOrder);
            }
        }

        private static void CreatePlatformWinLossBetListParameters(IObjectDb db, BetListMetaModel metaModel)
        {
            db.AddInParameter("custid", DbType.Int32, metaModel.CustId);
            db.AddInParameter("AgentList", DbType.String, metaModel.AgentListXml);
            db.AddInParameter("drawfilter", DbType.Int16, metaModel.DrawFilter);
            db.AddInParameter("PlatformType", DbType.Byte, metaModel.PlatformType);
            CreateBetListDateRangeParameters(db, metaModel);
            CreateBetlistBUParameters(db, metaModel);
            CreateProductListParameters(db, metaModel.ProductCodeList);
            CreateBetListPagingParameters(db, metaModel);
            CreateSummaryOutputParameters(db);
        }

        private static void CreateMatchWLParlayBetListParameters(IObjectDb db, BetListMetaModel metaModel)
        {
            CreateBetListSortingParameters(db, metaModel);
            db.AddInParameter("AgentList", DbType.String, metaModel.AgentListXml);
            db.AddInParameter("bettype", DbType.Int32, metaModel.BetType);
            db.AddInParameter("MoveBetsDay", DbType.DateTime, metaModel.MoveBetsDay);
            CreateBetListDateRangeParameters(db, metaModel);
            CreateBetListPagingParameters(db, metaModel);
            CreateSummaryOutputParameters(db);

            var supportedProducts = new[] { Product.SB, Product.NG, Product.BA, Product.VS, Product.VS2 };
            CreateProductListParameters(db, metaModel.ProductCodeList, supportedProducts);
        }

        private static void CreateMatchWLOutRightBetListParameters(IObjectDb db, BetListMetaModel metaModel)
        {
            db.AddInParameter("AgentList", DbType.String, metaModel.AgentListXml);
            CreateBetListDateRangeParameters(db, metaModel);
            db.AddInParameter("MoveBetsDay", DbType.DateTime, metaModel.MoveBetsDay);
            CreateBetListSortingParameters(db, metaModel);
            CreateBetListPagingParameters(db, metaModel);
            CreateSummaryOutputParameters(db);

            var supportedProducts = new[] { Product.SB, Product.BA };
            CreateProductListParameters(db, metaModel.ProductCodeList, supportedProducts);
        }

        private static void CreateMatchWLBettypeBetlistParameters(IObjectDb db, BetListMetaModel metaModel)
        {
            db.AddInParameter("AgentList", DbType.String, metaModel.AgentListXml);
            db.AddInParameter("bettype", DbType.Int32, metaModel.BetType);
            db.AddInParameter("MatchID", DbType.Int32, metaModel.MatchId);
            db.AddInParameter("liveindicator", DbType.Int32, metaModel.LiveIndicator.HasValue ? metaModel.LiveIndicator.Value : -1);
            db.AddInParameter("Date", DbType.DateTime, metaModel.Date);
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
            db.AddInParameter("AgentList", DbType.String, metaModel.AgentListXml);
            CreateBetListDateRangeParameters(db, metaModel);
            db.AddInParameter("MoveBetsDay", DbType.DateTime, metaModel.MoveBetsDay);
            AddLiveReportPagingParams(db, metaModel);
            db.AddInParameter("SportType", DbType.Int32, metaModel.SportType);

            var summaryParameters = GetOutputSummaryParametersWithBaseCurrency(defaultParams, SupportedBaseCurrencies);
            foreach (var name in summaryParameters)
            {
                db.AddOutParameter(name, DbType.Currency, 8);
            }
        }

        private static void CreateLiveReportParlayBetlistParameters(IObjectDb db, BetListMetaModel betlistMetaModel)
        {
            var metaModel = betlistMetaModel as LiveReportBetListMetaModel;

            CreateBetListSortingParameters(db, metaModel);
            db.AddInParameter("AgentList", DbType.String, metaModel.AgentListXml);
            CreateBetListDateRangeParameters(db, metaModel);
            db.AddInParameter("MoveBetsDay", DbType.DateTime, metaModel.MoveBetsDay);
            AddLiveReportPagingParams(db, metaModel);
            db.AddInParameter("Bettype", DbType.Int32, metaModel.BetType);
            db.AddInParameter("LiveIndicator", DbType.Int32, metaModel.LiveIndicator);
            var supportedProducts = new[] { Product.SB, Product.BA, Product.VS2 };
            CreateProductListParameters(db, metaModel.ProductCodeList, supportedProducts);
        }

        private static void CreaeteLiveReportNumberGameBetlistParameters(IObjectDb db, BetListMetaModel betlistMetaModel)
        {
            var metaModel = betlistMetaModel as LiveReportBetListMetaModel;

            CreateBetListSortingParameters(db, metaModel);
            db.AddInParameter("AgentList", DbType.String, metaModel.AgentListXml);
            CreateBetListDateRangeParameters(db, metaModel);
            db.AddInParameter("MoveBetsDay", DbType.DateTime, metaModel.MoveBetsDay);
            AddLiveReportPagingParams(db, metaModel);
            db.AddInParameter("LiveIndicator", DbType.Int32, metaModel.LiveIndicator);
        }

        private static void CreateLiveReportBetListByMatchParameters(IObjectDb db, BetListMetaModel betlistMetaModel)
        {
            var metaModel = betlistMetaModel as LiveReportBetListMetaModel;

            db.AddInParameter("AgentList", DbType.String, metaModel.AgentListXml);
            db.AddInParameter("MatchID", DbType.Int32, metaModel.MatchId);
            AddLiveReportPagingParams(db, metaModel);
            db.AddInParameter("LiveIndicator", DbType.Int32, metaModel.LiveIndicator);
            var supportedProducts = new[] { Product.SB, Product.BA, Product.VS, Product.MGCLC, Product.MGCRNG, Product.VS2 };
            CreateProductListParameters(db, metaModel.ProductCodeList, supportedProducts);
        }

        private static void CreateLiveReportOutRightBetlistParameters(IObjectDb db, BetListMetaModel betlistMetaModel)
        {
            var metaModel = betlistMetaModel as LiveReportBetListMetaModel;

            db.AddInParameter("AgentList", DbType.String, metaModel.AgentListXml);
            CreateBetListDateRangeParameters(db, metaModel);
            db.AddInParameter("MoveBetsDay", DbType.DateTime, metaModel.MoveBetsDay);
            AddLiveReportPagingParams(db, metaModel);
            var supportedProducts = new[] { Product.SB, Product.BA };
            CreateProductListParameters(db, metaModel.ProductCodeList, supportedProducts);
        }

        private static void CreateLicenseeWLBetlistParameters(IObjectDb db, BetListMetaModel metaModel)
        {
            db.AddInParameter("AgentList", DbType.String, metaModel.AgentListXml);
            db.AddInParameter("custid", DbType.Int32, metaModel.CustId);
            db.AddInParameter("drawfilter", DbType.Int16, metaModel.DrawFilter);
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
            db.AddInParameter("date", DbType.DateTime, metaModel.Date);
            db.AddInParameter("AgentList", DbType.String, metaModel.AgentListXml);
            db.AddInParameter("product", DbType.String, metaModel.ProductCode);
            db.AddInParameter("status", DbType.String, metaModel.DailyBetListStatus);
            CreateBetlistBUParameters(db, metaModel);
            CreateBetListPagingParameters(db, metaModel);
            CreateSummaryOutputParameters(db);
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
            CreateSummaryOutputParameters(db);
            CreateProductListParameters(db, metaModel.ProductCodeList);
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

        private static void CreateCasinoDetailParameters(IObjectDb db, BetListMetaModel metaModel)
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
            CreateBetListDateRangeParameters(db, metaModel);
            CreateBetlistBUParameters(db, metaModel);
            CreateBetListPagingParameters(db, metaModel);
            CreateSummaryOutputParameters(db);
        }

        private static void CreateBonusDetailParameters(IObjectDb db, BetListMetaModel metaModel)
        {
            db.AddInParameter("AgentList", DbType.String, metaModel.AgentListXml);
            db.AddInParameter("TransId", DbType.Int64, metaModel.TransId);
            db.AddInParameter("RefCode", DbType.Int64, metaModel.RefCode);
            db.AddInParameter("BaseCurrency", DbType.String, metaModel.BaseCurrency);
        }

        #region Utility

        private static void AddLiveReportPagingParams(IObjectDb db, LiveReportBetListMetaModel metaModel)
        {
            db.AddInParameter("PageSize", DbType.Int32, metaModel.IsExportExcel ? int.MaxValue - 1 : metaModel.PageSize);
            db.AddInParameter("PageIndex", DbType.Int32, metaModel.IsExportExcel ? 1 : metaModel.PageIndex);
        }

        private static void CreateBetListSortingParameters(IObjectDb db, BetListMetaModel metaModel)
        {
            if (!string.IsNullOrWhiteSpace(metaModel.SortBy))
            {
                var nonBaseCurrencyColumns = new[] { "nickname", "username" };
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

        private static void CreateSummaryOutputParameters(IObjectDb db, bool hasBaseCurrency = true)
        {
            var defaultParams = new[] { "SumWinlost", "SumLicenseeWL", "SumComm", "SumPlayerComm" };

            var summaryParameters = hasBaseCurrency ?
                GetOutputSummaryParametersWithBaseCurrency(defaultParams, SupportedBaseCurrencies) :
                defaultParams;

            foreach (var name in summaryParameters)
            {
                db.AddOutParameter(name, DbType.Currency, 8);
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
                db.AddInParameter("show" + productCode, DbType.Boolean, true);
            }
        }

        #endregion Utility
    }
}