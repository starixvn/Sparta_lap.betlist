namespace Fanex.Athena
{
    using System.Web.Configuration;
    using System.Web.Optimization;

    /// <summary>
    /// Bundle config.
    /// </summary>
    public static class BundleConfig
    {
        /// <summary>
        /// The CSS rewrite URL transform.
        /// </summary>
        private static CssRewriteUrlTransformWrapper cssRewriteUrlTransform;

        /// <summary>
        /// Registers the bundles.
        /// </summary>
        /// <param name="bundles">The bundles.</param>
        public static void RegisterBundles(BundleCollection bundles)
        {
            cssRewriteUrlTransform = new CssRewriteUrlTransformWrapper();
            RegisterLayoutBundles(bundles);
            RegisterViewBundles(bundles);

            BundleTable.EnableOptimizations = WebConfigurationManager.AppSettings["EnableBundle"] == "0";
        }

        /// <summary>
        /// Registers the view bundles.
        /// </summary>
        /// <param name="bundles">The bundles.</param>
        private static void RegisterViewBundles(BundleCollection bundles)
        {
            RegisterLastBetsMonitoringViewScripts(bundles);
            RegisterCustomerCommUpdateGroupViewScripts(bundles);
            RegisterCustomerLogCustomersViewScripts(bundles);
            RegisterForeCastAHViewScripts(bundles);
            RegisterHeaderViewScripts(bundles);
            RegisterHomeViewScripts(bundles);
            RegisterMenuViewScripts(bundles);

            RegisterEditAccountViewStyles(bundles);
            RegisterResetPasswordViewStyles(bundles);
            RegisterLastBetsMonitoringViewStyles(bundles);
            RegisterCustomerCommUpdateGroupViewStyles(bundles);
            RegisterCustomerLogCustomersViewStyles(bundles);
            RegisterForeCastAHViewStyles(bundles);
            RegisterHeaderViewStyles(bundles);
            RegisterHomeViewStyles(bundles);
            RegisterMenuViewCss(bundles);
        }

        /// <summary>
        /// Registers the layout bundles.
        /// </summary>
        /// <param name="bundles">The bundles.</param>
        private static void RegisterLayoutBundles(BundleCollection bundles)
        {
            RegisterLayoutScripts(bundles);
            RegisterAccountLayoutScripts(bundles);
            RegisterBetSettingLayoutScripts(bundles);
            RegisterForecastLayoutScripts(bundles);
            RegisterGroupCommLayoutScripts(bundles);
            RegisterTotalBetsLayoutScripts(bundles);
            RegisterJqgridLayoutScripts(bundles);
            RegisterBetListLayoutScripts(bundles);
            RegisterBetListLayoutNoStandardScripts(bundles);
            RegisterBetListSimpleLayoutScripts(bundles);
            RegisterCustomerLogLayoutScripts(bundles);
            RegisterReportLayoutScripts(bundles);
            RegisterReportLayoutSettledMatchCountScripts(bundles);
            RegisterReportTableLayoutScripts(bundles);
            RegisterWareHouseLayoutScripts(bundles);

            RegisterAccountLayoutStyles(bundles);
            RegisterBetListLayoutStyles(bundles);
            RegisterBetListLayoutNoStandardStyles(bundles);
            RegisterBetListSimpleLayoutStyles(bundles);
            RegisterBetSettingLayoutStyles(bundles);
            RegisterCustomerLogLayoutStyles(bundles);
            RegisterForecastLayoutStyles(bundles);
            RegisterGroupCommLayoutStyles(bundles);
            RegisterjqGridLayoutStyles(bundles);
            RegisterReportLayoutStyles(bundles);
            RegisterReportLayoutSettledMatchCountStyles(bundles);
            RegisterReportTableLayoutStyles(bundles);
            RegisterTotalBetsLayoutStyles(bundles);
            RegisterWareHouseLayoutStyles(bundles);

            RegisterViewAgCasinoBalance(bundles);
        }

        #region JavaScripts Shared Layout

        /// <summary>
        /// Registers the layout scripts.
        /// </summary>
        /// <param name="bundles">The bundles.</param>
        private static void RegisterLayoutScripts(BundleCollection bundles)
        {
            var bundle = new ScriptBundle("~/bundles/CoreJs").Include(
                        "~/Scripts/Library/jquery.min.js",
                        "~/Scripts/Library/jquery-ui.min.js",
                        "~/Scripts/Library/BigDecimal-all-last.min.js",
                        "~/Scripts/Library/jquery.cookie.js",
                        "~/Scripts/Library/jquery.blockUI.js",
                        "~/Scripts/Library/jquery.balloon.js",
                        "~/Scripts/Library/jquery.autocomplete.js",
                        "~/Scripts/Extension/jquery.extension.js",
                        "~/Scripts/Languages/ViewResources.js",
                        "~/Scripts/Languages/ViewResources.zh-CN.js",
                        "~/Scripts/Languages/ViewResources.zh-TW.js",
                        "~/Scripts/Languages/ViewResources.ko-KR.js",
                        "~/Scripts/Global/site.js",
                        "~/Scripts/Global/query.string.js",
                        "~/Scripts/Plugin/Note/note.js");
            AddNewScriptBundle(bundles, bundle);
        }

        /// <summary>
        /// Adds the new bundle.
        /// </summary>
        /// <param name="bundles">The bundles.</param>
        /// <param name="bundle">The bundle.</param>
        private static void AddNewScriptBundle(BundleCollection bundles, Bundle bundle)
        {
            bundle.Orderer = new NonOrderingBundleOrderer();
            bundles.Add(bundle);
        }

        /// <summary>
        /// Registers the account layout scripts.
        /// </summary>
        /// <param name="bundles">The bundles.</param>
        private static void RegisterAccountLayoutScripts(BundleCollection bundles)
        {
            var bundle = new ScriptBundle("~/bundles/AccountLayoutScripts")
                .Include(
                        "~/Scripts/Plugin/FlatPopup/jquery.custom-scrollbar.js",
                        "~/Scripts/Plugin/FlatPopup/jquery.flatpopup.js");
            AddNewScriptBundle(bundles, bundle);
        }

        /// <summary>
        /// Registers the bet setting layout scripts.
        /// </summary>
        /// <param name="bundles">The bundles.</param>
        private static void RegisterBetSettingLayoutScripts(BundleCollection bundles)
        {
            var bundle = new ScriptBundle("~/bundles/BetSettingLayoutScripts")
                .Include(
                        "~/Scripts/Customer/BetSetting/betsetting.js",
                        "~/Scripts/Library/jquery.balloon.js",
                        "~/Scripts/Plugin/FormatNumber/jshashtable.js",
                        "~/Scripts/Plugin/FormatNumber/jquery.numberformatter.js");
            AddNewScriptBundle(bundles, bundle);
        }

        /// <summary>
        /// Registers the forecast layout scripts.
        /// </summary>
        /// <param name="bundles">The bundles.</param>
        private static void RegisterForecastLayoutScripts(BundleCollection bundles)
        {
            var bundle = new ScriptBundle("~/bundles/ForecastLayoutScripts")
                .Include(
                "~/Scripts/Plugin/Refresh/refreshcountdown.js",
                "~/Scripts/Forecast/forecastpopup.js",
                "~/Scripts/Plugin/FlatPopup/jquery.custom-scrollbar.js",
                "~/Scripts/Plugin/FlatPopup/jquery.flatpopup.js");
            AddNewScriptBundle(bundles, bundle);
        }

        /// <summary>
        /// Registers the group commission layout scripts.
        /// </summary>
        /// <param name="bundles">The bundles.</param>
        private static void RegisterGroupCommLayoutScripts(BundleCollection bundles)
        {
            var bundle = new ScriptBundle("~/bundles/GroupCommLayoutScripts")
                .Include(
                        "~/Scripts/Plugin/FlatPopup/jquery.custom-scrollbar.js",
                        "~/Scripts/Plugin/FlatPopup/jquery.flatpopup.js");
            AddNewScriptBundle(bundles, bundle);
        }

        /// <summary>
        /// Registers the total bets layout scripts.
        /// </summary>
        /// <param name="bundles">The bundles.</param>
        private static void RegisterTotalBetsLayoutScripts(BundleCollection bundles)
        {
            var bundle = new ScriptBundle("~/bundles/TotalBetsLayoutScripts")
                .Include(
                        "~/Scripts/TotalBets/totalbetsbase.js",
                        "~/Scripts/Plugin/Refresh/refreshcountdown.js",
                        "~/Scripts/Plugin/FlatPopup/jquery.custom-scrollbar.js",
                        "~/Scripts/Plugin/FlatPopup/jquery.flatpopup.js");
            AddNewScriptBundle(bundles, bundle);
        }

        /// <summary>
        /// Registers the ware house layout scripts.
        /// </summary>
        /// <param name="bundles">The bundles.</param>
        private static void RegisterWareHouseLayoutScripts(BundleCollection bundles)
        {
            var bundle = new ScriptBundle("~/bundles/WareHouseLayoutScripts")
                .Include(
                    "~/Scripts/Plugin/DateRangePicker/date.min.js",
                    "~/Scripts/Plugin/DateRangePicker/daterangepicker.js");
            AddNewScriptBundle(bundles, bundle);
        }

        /// <summary>
        /// Registers the jQuery grid layout scripts.
        /// </summary>
        /// <param name="bundles">The bundles.</param>
        private static void RegisterJqgridLayoutScripts(BundleCollection bundles)
        {
            var bundle = new ScriptBundle("~/bundles/jqGridLayoutScripts")
                .Include(
                        "~/Scripts/global/jquery.jqgrid.additional.js",
                        "~/Scripts/Library/jquery.jqGrid.en.js",
                        "~/Scripts/Library/jquery.jqGrid.min.js",
                        "~/Scripts/Plugin/DateRangePicker/date.min.js",
                        "~/Scripts/Plugin/MonthPickr/monpickr.js",
                        "~/Scripts/Plugin/DateRangePicker/daterangepicker.js",
                        "~/Scripts/Plugin/LinqJs/JSLINQ.js",
                        "~/Scripts/Report/report.js",
                        "~/Scripts/global/prototype.js",
                        "~/Scripts/Plugin/FlatPopup/jquery.custom-scrollbar.js",
                        "~/Scripts/Plugin/FlatPopup/jquery.flatpopup.js",
                        "~/Scripts/Plugin/FanexGrid/Fanex.Grid.js",
                        "~/Scripts/Plugin/FanexGrid/Fanex.Pagination.js",
                        "~/Scripts/Plugin/FanexGrid/moment.min.js",
                        "~/Scripts/Plugin/FanexGrid/numeral.min.js");
            AddNewScriptBundle(bundles, bundle);
        }

        /// <summary>
        /// Registers the bet list layout scripts.
        /// </summary>
        /// <param name="bundles">The bundles.</param>
        private static void RegisterBetListLayoutScripts(BundleCollection bundles)
        {
            var bundle = new ScriptBundle("~/bundles/BetListLayoutScripts")
                .Include(
                        "~/Scripts/global/jquery.jqgrid.additional.js",
                        "~/Scripts/Library/jquery.jqGrid.en.js",
                        "~/Scripts/Library/jquery.jqGrid.min.js",
                        "~/Scripts/Plugin/MonthPickr/monpickr.js",
                        "~/Scripts/Plugin/FlatPopup/jquery.custom-scrollbar.js",
                        "~/Scripts/Plugin/FlatPopup/jquery.flatpopup.js",
                        "~/Scripts/Report/report.js",
                        "~/scripts/global/Paging.js",
                        "~/Scripts/Plugin/DateRangePicker/date.min.js",
                        "~/Scripts/Plugin/DateRangePicker/daterangepicker.js",
                        "~/Scripts/BetList/betlist.option.js",
                        "~/Scripts/BetList/betlist.js");
            AddNewScriptBundle(bundles, bundle);
        }

        /// <summary>
        /// Registers the bet list layout no standard scripts.
        /// </summary>
        /// <param name="bundles">The bundles.</param>
        private static void RegisterBetListLayoutNoStandardScripts(BundleCollection bundles)
        {
            var bundle = new ScriptBundle("~/bundles/BetListLayoutNoStandardScripts")
                .Include(
                        "~/Scripts/global/jquery.jqgrid.additional.js",
                        "~/Scripts/Library/jquery.jqGrid.en.js",
                        "~/Scripts/Library/jquery.jqGrid.min.js",
                        "~/Scripts/Plugin/MonthPickr/monpickr.js",
                        "~/Scripts/Report/report.js",
                        "~/scripts/global/Paging.js",
                        "~/Scripts/Plugin/DateRangePicker/date.min.js",
                        "~/Scripts/Plugin/DateRangePicker/daterangepicker.js",
                        "~/Scripts/BetList/betlist.option.js",
                        "~/Scripts/BetList/betlist.js");
            AddNewScriptBundle(bundles, bundle);
        }

        /// <summary>
        /// Registers the bet list simple layout scripts.
        /// </summary>
        /// <param name="bundles">The bundles.</param>
        private static void RegisterBetListSimpleLayoutScripts(BundleCollection bundles)
        {
            var bundle = new ScriptBundle("~/bundles/BetListSimpleLayoutScripts")
                .Include(
                        "~/Scripts/global/jquery.jqgrid.additional.js",
                        "~/Scripts/Library/jquery.jqGrid.en.js",
                        "~/Scripts/Library/jquery.jqGrid.min.js",
                        "~/Scripts/Plugin/MonthPickr/monpickr.js",
                        "~/Scripts/Report/report.js",
                        "~/scripts/global/Paging.js",
                        "~/Scripts/Plugin/DateRangePicker/date.min.js",
                        "~/Scripts/Plugin/DateRangePicker/daterangepicker.js",
                        "~/Scripts/BetList/betlist.option.js",
                        "~/Scripts/BetList/betlist.js",
                        "~/Scripts/BetList/commonbetlist.js");
            AddNewScriptBundle(bundles, bundle);
        }

        /// <summary>
        /// Registers the customer log layout scripts.
        /// </summary>
        /// <param name="bundles">The bundles.</param>
        private static void RegisterCustomerLogLayoutScripts(BundleCollection bundles)
        {
            var bundle = new ScriptBundle("~/bundles/CustomerLogLayoutScripts")
                .Include(
                        "~/Scripts/Plugin/MonthPickr/monpickr.js",
                        "~/Scripts/global/jquery.jqgrid.additional.js",
                        "~/Scripts/Library/jquery.jqGrid.en.js",
                        "~/Scripts/Library/jquery.jqGrid.min.js",
                        "~/Scripts/Plugin/FlatPopup/jquery.custom-scrollbar.js",
                        "~/Scripts/Plugin/FlatPopup/jquery.flatpopup.js",
                        "~/Scripts/Customer/CustomerLog/customerlog.js");
            AddNewScriptBundle(bundles, bundle);
        }

        /// <summary>
        /// Registers the report layout scripts.
        /// </summary>
        /// <param name="bundles">The bundles.</param>
        private static void RegisterReportLayoutScripts(BundleCollection bundles)
        {
            var bundle = new ScriptBundle("~/bundles/ReportLayoutScripts")
                .Include(
                        "~/Scripts/Report/report.js",
                        "~/scripts/global/Paging.js",
                        "~/Scripts/Plugin/DateRangePicker/date.min.js",
                        "~/Scripts/Plugin/MonthPickr/monpickr.js",
                        "~/Scripts/Plugin/DateRangePicker/daterangepicker.js",
                        "~/Scripts/Plugin/FlatPopup/jquery.custom-scrollbar.js",
                        "~/Scripts/Plugin/FlatPopup/jquery.flatpopup.js");
            AddNewScriptBundle(bundles, bundle);
        }

        /// <summary>
        /// Registers the report layout settled match count scripts.
        /// </summary>
        /// <param name="bundles">The bundles.</param>
        private static void RegisterReportLayoutSettledMatchCountScripts(BundleCollection bundles)
        {
            var bundle = new ScriptBundle("~/bundles/ReportLayoutSettledMatchCountScripts")
                .Include(
                        "~/Scripts/Plugin/MonthPickr/monpickr.js",
                        "~/Scripts/Report/report.js",
                        "~/scripts/global/Paging.js",
                        "~/Scripts/Plugin/DateRangePicker/date.min.js",
                        "~/Scripts/Plugin/DateRangePicker/daterangepicker.js");
            AddNewScriptBundle(bundles, bundle);
        }

        /// <summary>
        /// Registers the report table layout scripts.
        /// </summary>
        /// <param name="bundles">The bundles.</param>
        private static void RegisterReportTableLayoutScripts(BundleCollection bundles)
        {
            var bundle = new ScriptBundle("~/bundles/ReportTableLayoutScripts")
                .Include(
                        "~/Scripts/Plugin/DateRangePicker/date.min.js",
                        "~/Scripts/Plugin/MonthPickr/monpickr.js",
                        "~/Scripts/Report/report.js",
                        "~/scripts/global/Paging.js",
                        "~/Scripts/Plugin/DateRangePicker/daterangepicker.js",
                        "~/Scripts/Plugin/FlatPopup/jquery.custom-scrollbar.js",
                        "~/Scripts/Plugin/FlatPopup/jquery.flatpopup.js");
            AddNewScriptBundle(bundles, bundle);
        }

        #endregion JavaScripts Shared Layout

        #region CSS Shared Layout

        /// <summary>
        /// Registers the account layout styles.
        /// </summary>
        /// <param name="bundles">The bundles.</param>
        private static void RegisterAccountLayoutStyles(BundleCollection bundles)
        {
            AddStyleBundle(
                bundles,
                "~/bundles/AccountLayoutStyles",
                "~/Content/Library/start/jquery-ui.min.css",
                "~/Content/Global/lrf.global.content.css",
                "~/Content/FlatPopup/jquery.custom-scrollbar.css",
                "~/Content/FlatPopup/jquery.flatpopup.css");
        }

        /// <summary>
        /// Registers the bet list layout styles.
        /// </summary>
        /// <param name="bundles">The bundles.</param>
        private static void RegisterBetListLayoutStyles(BundleCollection bundles)
        {
            AddStyleBundle(
                bundles,
                "~/bundles/BetListLayoutStyles",
                "~/Content/Library/smoothness/jqgrid-custom.css",
                "~/Content/Library/ui.jqgrid/jqgrid.extension.css",
                "~/Content/Library/start/jquery-ui.min.css",
                "~/Content/Library/ui.jqgrid/ui.jqgrid.css",
                "~/Content/Global/grid26.css",
                "~/Content/Global/lrf.global.content.css",
                "~/Content/Plugin/MonthPickr/monPickr.css",
                "~/Content/Global/Paging.css",
                "~/Content/Plugin/DateRangePicker/daterangepicker.css",
                "~/Content/BetList/betlist.core.css",
                "~/Content/FlatPopup/jquery.custom-scrollbar.css",
                "~/Content/FlatPopup/jquery.flatpopup.css");
        }

        /// <summary>
        /// Registers the bet list layout no standard styles.
        /// </summary>
        /// <param name="bundles">The bundles.</param>
        private static void RegisterBetListLayoutNoStandardStyles(BundleCollection bundles)
        {
            AddStyleBundle(
                bundles,
                "~/bundles/BetListLayoutNoStandardStyles",
                "~/Content/Library/smoothness/jqgrid-custom.css",
                "~/Content/Library/ui.jqgrid/jqgrid.extension.css",
                "~/Content/Library/start/jquery-ui.min.css",
                "~/Content/Library/ui.jqgrid/ui.jqgrid.css",
                "~/Content/Global/grid26.css",
                "~/Content/Global/lrf.global.content.css",
                "~/Content/Plugin/MonthPickr/monPickr.css",
                "~/Content/Global/Paging.css",
                "~/Content/Plugin/DateRangePicker/daterangepicker.css",
                "~/Content/BetList/betlist.core.css");
        }

        /// <summary>
        /// Registers the bet list simple layout styles.
        /// </summary>
        /// <param name="bundles">The bundles.</param>
        private static void RegisterBetListSimpleLayoutStyles(BundleCollection bundles)
        {
            AddStyleBundle(
                bundles,
                "~/bundles/BetListSimpleLayoutStyles",
                "~/Content/Library/smoothness/jqgrid-custom.css",
                "~/Content/Library/ui.jqgrid/jqgrid.extension.css",
                "~/Content/Library/start/jquery-ui.min.css",
                "~/Content/Library/ui.jqgrid/ui.jqgrid.css",
                "~/Content/Global/grid26.css",
                "~/Content/Global/lrf.global.content.css",
                "~/Content/Plugin/MonthPickr/monPickr.css",
                "~/Content/Global/Paging.css",
                "~/Content/Plugin/DateRangePicker/daterangepicker.css",
                "~/Content/BetList/betlist.core.css");
        }

        /// <summary>
        /// Registers the bet setting layout styles.
        /// </summary>
        /// <param name="bundles">The bundles.</param>
        private static void RegisterBetSettingLayoutStyles(BundleCollection bundles)
        {
            AddStyleBundle(
                bundles,
                "~/bundles/BetSettingLayoutStyles",
                "~/Content/Library/start/jquery-ui.min.css",
                "~/Content/Global/grid26.css",
                "~/Content/Global/lrf.global.grid.css",
                "~/Content/Global/lrf.global.content.css",
                "~/Content/Global/lrf.global.reportable.css",
                "~/Content/Customer/BetSetting/BetSetting.css");
        }

        /// <summary>
        /// Registers the customer log layout styles.
        /// </summary>
        /// <param name="bundles">The bundles.</param>
        private static void RegisterCustomerLogLayoutStyles(BundleCollection bundles)
        {
            AddStyleBundle(
                bundles,
                "~/bundles/CustomerLogLayoutStyles",
                "~/Content/Global/grid26.css",
                "~/Content/Global/lrf.global.content.css",
                "~/Content/Global/lrf.global.reportable.css",
                "~/Content/Library/start/jquery-ui.min.css",
                "~/Content/Plugin/MonthPickr/monPickr.css",
                "~/Content/Library/smoothness/jqgrid-custom.css",
                "~/Content/Library/ui.jqgrid/jqgrid.extension.css",
                "~/Content/Library/start/jquery-ui.min.css",
                "~/Content/Library/ui.jqgrid/ui.jqgrid.css",
                "~/Content/FlatPopup/jquery.custom-scrollbar.css",
                "~/Content/FlatPopup/jquery.flatpopup.css",
                "~/Content/Customer/BetSetting/BetSetting.css",
                "~/Content/Customer/CustomerLog/customerlog.css");
        }

        /// <summary>
        /// Registers the forecast layout styles.
        /// </summary>
        /// <param name="bundles">The bundles.</param>
        private static void RegisterForecastLayoutStyles(BundleCollection bundles)
        {
            AddStyleBundle(
                bundles,
                "~/bundles/ForecastLayoutStyles",
                "~/Content/Global/grid26.css",
                "~/Content/Global/lrf.global.content.css",
                "~/Content/Global/lrf.global.reportable.css",
                "~/Content/Forecast/forecast.css",
                "~/Content/TotalBets/refreshcountdown.css",
                "~/Content/Library/start/jquery-ui.min.css",
                "~/Content/FlatPopup/jquery.custom-scrollbar.css",
                "~/Content/FlatPopup/jquery.flatpopup.css",
                "~/Content/UserGuide/user-guide.css");
        }

        /// <summary>
        /// Registers the group commission layout styles.
        /// </summary>
        /// <param name="bundles">The bundles.</param>
        private static void RegisterGroupCommLayoutStyles(BundleCollection bundles)
        {
            AddStyleBundle(
                bundles,
                "~/bundles/GroupCommLayoutStyles",
                "~/Content/Library/start/jquery-ui.min.css",
                "~/Content/Global/lrf.global.content.css",
                "~/Content/Global/lrf.global.reportable.css",
                "~/Content/Customer/groupcomm.css",
                "~/Content/Global/grid26.css",
                "~/Content/FlatPopup/jquery.custom-scrollbar.css",
                "~/Content/FlatPopup/jquery.flatpopup.css");
        }

        /// <summary>
        /// Register jQuery Grid layout styles.
        /// </summary>
        /// <param name="bundles">The bundles.</param>
        private static void RegisterjqGridLayoutStyles(BundleCollection bundles)
        {
            AddStyleBundle(
                bundles,
                "~/bundles/jqGridLayoutStyles",
                "~/Content/Library/smoothness/jqgrid-custom.css",
                "~/Content/Library/ui.jqgrid/jqgrid.extension.css",
                "~/Content/Library/start/jquery-ui.min.css",
                "~/Content/Library/ui.jqgrid/ui.jqgrid.css",
                "~/Content/Global/grid26.css",
                "~/Content/Global/lrf.global.content.css",
                "~/Content/Plugin/MonthPickr/monPickr.css",
                "~/Content/Plugin/DateRangePicker/daterangepicker.css",
                "~/Content/FlatPopup/jquery.custom-scrollbar.css",
                "~/Content/FlatPopup/jquery.flatpopup.css",
                "~/Content/Plugin/FanexGrid/Fanex.Grid.css",
                "~/Content/Plugin/FanexGrid/Fanex.Pagination.css");
        }

        /// <summary>
        /// Registers the report layout styles.
        /// </summary>
        /// <param name="bundles">The bundles.</param>
        private static void RegisterReportLayoutStyles(BundleCollection bundles)
        {
            AddStyleBundle(
                bundles,
                "~/bundles/ReportLayoutStyles",
                "~/Content/Library/start/jquery-ui.min.css",
                "~/Content/Global/grid26.css",
                "~/Content/Global/lrf.global.grid.css",
                "~/Content/Global/lrf.global.content.css",
                "~/Content/Plugin/MonthPickr/monPickr.css",
                "~/Content/Global/Paging.css",
                "~/Content/Plugin/DateRangePicker/daterangepicker.css",
                "~/Content/FlatPopup/jquery.custom-scrollbar.css",
                "~/Content/FlatPopup/jquery.flatpopup.css");
        }

        /// <summary>
        /// Registers the report layout settled match count styles.
        /// </summary>
        /// <param name="bundles">The bundles.</param>
        private static void RegisterReportLayoutSettledMatchCountStyles(BundleCollection bundles)
        {
            AddStyleBundle(
                bundles,
                "~/bundles/ReportLayoutSettledMatchCountStyles",
                "~/Content/Library/start/jquery-ui.min.css",
                "~/Content/Global/grid26.css",
                "~/Content/Global/lrf.global.grid.css",
                "~/Content/Global/lrf.global.content.css",
                "~/Content/Plugin/MonthPickr/monPickr.css",
                "~/Content/Global/Paging.css",
                "~/Content/Plugin/DateRangePicker/daterangepicker.css");
        }

        /// <summary>
        /// Registers the report table layout styles.
        /// </summary>
        /// <param name="bundles">The bundles.</param>
        private static void RegisterReportTableLayoutStyles(BundleCollection bundles)
        {
            AddStyleBundle(
                bundles,
                "~/bundles/ReportTableLayoutStyles",
                "~/Content/Library/start/jquery-ui.min.css",
                "~/Content/Global/grid26.css",
                "~/Content/Global/lrf.global.content.css",
                "~/Content/Global/lrf.global.reportable.css",
                "~/Content/Plugin/MonthPickr/monPickr.css",
                "~/Content/Global/Paging.css",
                "~/Content/Plugin/DateRangePicker/daterangepicker.css",
                "~/Content/FlatPopup/jquery.custom-scrollbar.css",
                "~/Content/FlatPopup/jquery.flatpopup.css");
        }

        /// <summary>
        /// Registers the total bets layout styles.
        /// </summary>
        /// <param name="bundles">The bundles.</param>
        private static void RegisterTotalBetsLayoutStyles(BundleCollection bundles)
        {
            AddStyleBundle(
                bundles,
                "~/bundles/TotalBetsLayoutStyles",
                "~/Content/Global/grid26.css",
                "~/Content/Global/lrf.global.content.css",
                "~/Content/Global/lrf.global.reportable.css",
                "~/Content/TotalBets/totalbetsbase.css",
                "~/Content/TotalBets/refreshcountdown.css",
                "~/Content/Library/start/jquery-ui.min.css",
                "~/Content/FlatPopup/jquery.custom-scrollbar.css",
                "~/Content/FlatPopup/jquery.flatpopup.css",
                "~/Content/UserGuide/user-guide.css");
        }

        /// <summary>
        /// Registers the ware house layout styles.
        /// </summary>
        /// <param name="bundles">The bundles.</param>
        private static void RegisterWareHouseLayoutStyles(BundleCollection bundles)
        {
            AddStyleBundle(
                bundles,
                "~/bundles/WareHouseLayoutStyes",
                "~/Content/Library/start/jquery-ui.min.css",
                "~/Content/Global/grid26.css",
                "~/Content/Global/lrf.global.content.css",
                "~/Content/Global/lrf.global.grid.css",
                "~/Content/Plugin/DateRangePicker/daterangepicker.css",
                "~/Content/Warehouse/Warehouse.css");
        }

        #endregion CSS Shared Layout

        #region Scripts & Styles Views

        private static void RegisterViewAgCasinoBalance(BundleCollection bundles)
        {
            var bundle = new ScriptBundle("~/bundles/AgCasinoBalanceScripts")
                .Include(
                        "~/Scripts/Global/Paging.js",
                        "~/Scripts/Report/agcasinobalance.js",
                        "~/Scripts/Report/agcasinobalance.option.js");
            AddNewScriptBundle(bundles, bundle);

            AddStyleBundle(
               bundles,
               "~/bundles/AgCasinoBalanceStyles",
               "~/Content/Report/agcasinowinloss.css",
               "~/Content/Plugin/Common/bread-crumb.css",
               "~/Content/Global/Paging.css");
        }

        #endregion Scripts & Styles Views

        #region Javascript Views

        /// <summary>
        /// Registers the last bets monitoring view scripts.
        /// </summary>
        /// <param name="bundles">The bundles.</param>
        private static void RegisterLastBetsMonitoringViewScripts(BundleCollection bundles)
        {
            var bundle = new ScriptBundle("~/bundles/LastBetsMonitoringScripts")
                .Include(
                        "~/Scripts/Plugin/LinqJs/JSLINQ.js",
                        "~/Scripts/Global/prototype.js",
                        "~/Scripts/Plugin/MultiSelect/jquery.multiselect.js",
                        "~/Scripts/BetList/lastbetsmonitornew.js");
            AddNewScriptBundle(bundles, bundle);
        }

        /// <summary>
        /// Registers the customer commission update group view scripts.
        /// </summary>
        /// <param name="bundles">The bundles.</param>
        private static void RegisterCustomerCommUpdateGroupViewScripts(BundleCollection bundles)
        {
            var bundle = new ScriptBundle("~/bundles/CustomerCommUpdateGroupScripts")
                .Include(
                        "~/Scripts/Plugin/ColorPicker/spectrum.js",
                        "~/Scripts/Customer/CustomerComm/updategroup.js",
                        "~/Scripts/Customer/CustomerComm/groupcomm.js");
            AddNewScriptBundle(bundles, bundle);
        }

        /// <summary>
        /// Registers the customer log customers view scripts.
        /// </summary>
        /// <param name="bundles">The bundles.</param>
        private static void RegisterCustomerLogCustomersViewScripts(BundleCollection bundles)
        {
            var bundle = new ScriptBundle("~/bundles/CustomerLogCustomersScripts")
                .Include(
                        "~/Scripts/Plugin/MultiSelect/jquery.multiselect.js",
                        "~/Scripts/Global/Paging.js",
                        "~/Scripts/Customer/Customers.option.js",
                        "~/Scripts/Customer/Customers.js",
                        "~/Scripts/Customer/CustomerCommList.js");
            AddNewScriptBundle(bundles, bundle);
        }

        /// <summary>
        /// Registers the fore cast ah view scripts.
        /// </summary>
        /// <param name="bundles">The bundles.</param>
        private static void RegisterForeCastAHViewScripts(BundleCollection bundles)
        {
            var bundle = new ScriptBundle("~/bundles/ForeCastAHScripts")
                .Include(
                        "~/Scripts/Plugin/Refresh/refreshcountdown.js",
                        "~/Scripts/Forecast/forecastpopup.js",
                        "~/Scripts/Forecast/forecast.ah.js");
            AddNewScriptBundle(bundles, bundle);
        }

        /// <summary>
        /// Registers the header view scripts.
        /// </summary>
        /// <param name="bundles">The bundles.</param>
        private static void RegisterHeaderViewScripts(BundleCollection bundles)
        {
            var bundle = new ScriptBundle("~/bundles/HeaderScripts")
                .Include(
                        "~/Scripts/Library/jquery.min.js",
                        "~/Scripts/Library/jquery.blockUI.js",
                        "~/Scripts/Global/site.js",
                        "~/Scripts/Header/header.js",
                        "~/Scripts/Header/jquery.jclock.js");
            AddNewScriptBundle(bundles, bundle);
        }

        /// <summary>
        /// Registers the home view scripts.
        /// </summary>
        /// <param name="bundles">The bundles.</param>
        private static void RegisterHomeViewScripts(BundleCollection bundles)
        {
            var bundle = new ScriptBundle("~/bundles/HomeScripts")
                .Include(
                        "~/Scripts/Library/jquery.min.js",
                        "~/Scripts/Library/jquery-ui.min.js",
                        "~/Scripts/Library/jquery.cookie.js",
                        "~/Scripts/Library/jquery.blockUI.js",
                        "~/Scripts/Global/site.js");
            AddNewScriptBundle(bundles, bundle);
        }

        /// <summary>
        /// Registers the menu view scripts.
        /// </summary>
        /// <param name="bundles">The bundles.</param>
        private static void RegisterMenuViewScripts(BundleCollection bundles)
        {
            var bundle = new ScriptBundle("~/bundles/MenuScripts")
                .Include(
                        "~/Scripts/Library/jquery.min.js",
                        "~/Scripts/Library/jquery.autocomplete.js",
                        "~/Scripts/Menu/Menu.js");
            AddNewScriptBundle(bundles, bundle);
        }

        #endregion Javascript Views

        #region CSS Views

        /// <summary>
        /// Registers the edit account view styles.
        /// </summary>
        /// <param name="bundles">The bundles.</param>
        private static void RegisterEditAccountViewStyles(BundleCollection bundles)
        {
            AddStyleBundle(
                bundles,
                "~/bundles/EditAccountViewStyles",
                "~/Content/Library/start/jquery-ui.min.css",
                "~/Content/Global/Global.css",
                "~/Content/Global/lrf.global.content.css",
                "~/Content/Global/lrf.global.reportable.css",
                "~/Content/Account/AccountManagement.css");
        }

        /// <summary>
        /// Registers the reset password view styles.
        /// </summary>
        /// <param name="bundles">The bundles.</param>
        private static void RegisterResetPasswordViewStyles(BundleCollection bundles)
        {
            AddStyleBundle(
                bundles,
                "~/bundles/ResetPasswordViewStyles",
                "~/Content/Library/start/jquery-ui.min.css",
                "~/Content/Global/Global.css",
                "~/Content/Global/lrf.global.content.css",
                "~/Content/Global/lrf.global.reportable.css",
                "~/Content/Account/AccountManagement.css");
        }

        /// <summary>
        /// Registers the last bets monitoring view styles.
        /// </summary>
        /// <param name="bundles">The bundles.</param>
        private static void RegisterLastBetsMonitoringViewStyles(BundleCollection bundles)
        {
            AddStyleBundle(
                bundles,
                "~/bundles/LastBetsMonitoringViewStyles",
                "~/Content/Plugin/MultiSelect/jquery.multiselect.css",
                "~/Content/BetList/lastbetmonitornew.css",
                "~/Content/BetList/lastbetmonitor.table.css");
        }

        /// <summary>
        /// Registers the customer commission update group view styles.
        /// </summary>
        /// <param name="bundles">The bundles.</param>
        private static void RegisterCustomerCommUpdateGroupViewStyles(BundleCollection bundles)
        {
            AddStyleBundle(
                bundles,
                "~/bundles/CustomerCommUpdateGroupViewStyles",
                "~/Content/Plugin/ColorPicker/Spectrum.css",
                "~/Content/Global/grid26.css");
        }

        /// <summary>
        /// Registers the customer log customers view styles.
        /// </summary>
        /// <param name="bundles">The bundles.</param>
        private static void RegisterCustomerLogCustomersViewStyles(BundleCollection bundles)
        {
            AddStyleBundle(
                bundles,
                "~/bundles/CustomerLogCustomersViewStyles",
                "~/Content/Customer/CustomerList.css",
                "~/Content/Plugin/MultiSelect/jquery.multiselect.css",
                "~/Content/Global/Paging.css");
        }

        /// <summary>
        /// Registers the fore cast ah view styles.
        /// </summary>
        /// <param name="bundles">The bundles.</param>
        private static void RegisterForeCastAHViewStyles(BundleCollection bundles)
        {
            AddStyleBundle(
                bundles,
                "~/bundles/ForeCastAHViewStyles",
                "~/Content/Global/grid26.css",
               "~/Content/Global/lrf.global.content.css",
               "~/Content/Global/lrf.global.reportable.css",
               "~/Content/Forecast/forecast.css",
               "~/Content/Forecast/forecast.ah.css",
               "~/Content/TotalBets/refreshcountdown.css",
               "~/Content/Library/start/jquery-ui.min.css");
        }

        /// <summary>
        /// Registers the header view styles.
        /// </summary>
        /// <param name="bundles">The bundles.</param>
        private static void RegisterHeaderViewStyles(BundleCollection bundles)
        {
            AddStyleBundle(
                bundles,
                "~/bundles/HeaderViewStyles",
                "~/Content/Themes/12Bet/Site.css",
                "~/Content/Global/Global.css");
        }

        /// <summary>
        /// Registers the home view styles.
        /// </summary>
        /// <param name="bundles">The bundles.</param>
        private static void RegisterHomeViewStyles(BundleCollection bundles)
        {
            AddStyleBundle(
                bundles,
                "~/bundles/HomeViewStyles",
                "~/Content/Global/Global.css",
                "~/Content/Themes/12Bet/Site.css",
                "~/Content/Layout/master.css");
        }

        /// <summary>
        /// Registers the menu view CSS.
        /// </summary>
        /// <param name="bundles">The bundles.</param>
        private static void RegisterMenuViewCss(BundleCollection bundles)
        {
            AddStyleBundle(
                bundles,
                "~/bundles/MenuViewCss",
                "~/Content/Global/Global.css",
                "~/Content/Themes/12Bet/Site.css");
        }

        #endregion CSS Views

        #region Private Utility Functions

        /// <summary>
        /// Adds the style bundle.
        /// </summary>
        /// <param name="bundles">The bundles.</param>
        /// <param name="bundleVirtualPath">The bundle virtual path.</param>
        /// <param name="virtualPaths">The virtual paths.</param>
        private static void AddStyleBundle(BundleCollection bundles, string bundleVirtualPath, params string[] virtualPaths)
        {
            var styleBundle = new StyleBundle(bundleVirtualPath);
            foreach (var virtualPath in virtualPaths)
            {
                styleBundle.Include(virtualPath, cssRewriteUrlTransform);
            }

            bundles.Add(styleBundle);
        }

        #endregion Private Utility Functions
    }
}