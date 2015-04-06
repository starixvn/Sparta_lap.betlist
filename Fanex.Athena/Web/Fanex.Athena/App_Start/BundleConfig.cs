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
        /// Registers the bundles.
        /// </summary>
        /// <param name="bundles">The bundles.</param>
        public static void RegisterBundles(BundleCollection bundles)
        {
            RegisterLayoutBundles(bundles);
            RegisterViewBundles(bundles);

            BundleTable.EnableOptimizations = WebConfigurationManager.AppSettings["EnableBundle"] == "1";
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
        }

        #region JavaScripts Shared Layout

        /// <summary>
        /// Registers the layout scripts.
        /// </summary>
        /// <param name="bundles">The bundles.</param>
        private static void RegisterLayoutScripts(BundleCollection bundles)
        {
            var bundle = new ScriptBundle("~/bundles/CoreJs")
                .Include("~/Scripts/Library/jquery.min.js")
                .Include("~/Scripts/Library/jquery-ui.min.js")
                .Include("~/Scripts/Library/BigDecimal-all-last.min.js")
                .Include("~/Scripts/Library/jquery.cookie.js")
                .Include("~/Scripts/Library/jquery.blockUI.js")
                .Include("~/Scripts/Library/jquery.balloon.js")
                .Include("~/Scripts/Library/jquery.autocomplete.js")
                .Include("~/Scripts/Extension/jquery.extension.js")
                .Include("~/Scripts/Languages/ViewResources.js")
                .Include("~/Scripts/Languages/ViewResources.zh-CN.js")
                .Include("~/Scripts/Languages/ViewResources.zh-TW.js")
                .Include("~/Scripts/Languages/ViewResources.ko-KR.js")
                .Include("~/Scripts/Global/site.js")
                .Include("~/Scripts/Global/query.string.js")
                .Include("~/Scripts/Plugin/Note/note.js");

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
                .Include("~/Scripts/Plugin/FlatPopup/jquery.custom-scrollbar.js")
                .Include("~/Scripts/Plugin/FlatPopup/jquery.flatpopup.js");
            bundle.Orderer = new NonOrderingBundleOrderer();
            bundles.Add(bundle);
        }

        /// <summary>
        /// Registers the bet setting layout scripts.
        /// </summary>
        /// <param name="bundles">The bundles.</param>
        private static void RegisterBetSettingLayoutScripts(BundleCollection bundles)
        {
            var bundle = new ScriptBundle("~/bundles/BetSettingLayoutScripts")
                .Include("~/Scripts/Customer/BetSetting/betsetting.js")
                .Include("~/Scripts/Library/jquery.balloon.js")
                .Include("~/Scripts/Plugin/FormatNumber/jshashtable.js")
                .Include("~/Scripts/Plugin/FormatNumber/jquery.numberformatter.js");
            bundle.Orderer = new NonOrderingBundleOrderer();
            bundles.Add(bundle);
        }

        /// <summary>
        /// Registers the forecast layout scripts.
        /// </summary>
        /// <param name="bundles">The bundles.</param>
        private static void RegisterForecastLayoutScripts(BundleCollection bundles)
        {
            var bundle = new ScriptBundle("~/bundles/ForecastLayoutScripts")
                .Include("~/Scripts/Plugin/Refresh/refreshcountdown.js")
                .Include("~/Scripts/Forecast/forecastpopup.js")
                .Include("~/Scripts/Plugin/FlatPopup/jquery.custom-scrollbar.js")
                .Include("~/Scripts/Plugin/FlatPopup/jquery.flatpopup.js");
            bundle.Orderer = new NonOrderingBundleOrderer();
            bundles.Add(bundle);
        }

        /// <summary>
        /// Registers the group commission layout scripts.
        /// </summary>
        /// <param name="bundles">The bundles.</param>
        private static void RegisterGroupCommLayoutScripts(BundleCollection bundles)
        {
            var bundle = new ScriptBundle("~/bundles/GroupCommLayoutScripts")
                .Include("~/Scripts/Plugin/FlatPopup/jquery.custom-scrollbar.js")
                .Include("~/Scripts/Plugin/FlatPopup/jquery.flatpopup.js");
            bundle.Orderer = new NonOrderingBundleOrderer();
            bundles.Add(bundle);
        }

        /// <summary>
        /// Registers the total bets layout scripts.
        /// </summary>
        /// <param name="bundles">The bundles.</param>
        private static void RegisterTotalBetsLayoutScripts(BundleCollection bundles)
        {
            var bundle = new ScriptBundle("~/bundles/TotalBetsLayoutScripts")
                .Include("~/Scripts/TotalBets/totalbetsbase.js")
                .Include("~/Scripts/Plugin/Refresh/refreshcountdown.js")
                .Include("~/Scripts/Plugin/FlatPopup/jquery.custom-scrollbar.js")
                .Include("~/Scripts/Plugin/FlatPopup/jquery.flatpopup.js");
            bundle.Orderer = new NonOrderingBundleOrderer();
            bundles.Add(bundle);
        }

        /// <summary>
        /// Registers the ware house layout scripts.
        /// </summary>
        /// <param name="bundles">The bundles.</param>
        private static void RegisterWareHouseLayoutScripts(BundleCollection bundles)
        {
            var bundle = new ScriptBundle("~/bundles/WareHouseLayoutScripts")
                .Include("~/Scripts/Plugin/DateRangePicker/date.min.js")
                .Include("~/Scripts/Plugin/DateRangePicker/daterangepicker.js");
            bundle.Orderer = new NonOrderingBundleOrderer();
            bundles.Add(bundle);
        }

        /// <summary>
        /// Registers the jQuery grid layout scripts.
        /// </summary>
        /// <param name="bundles">The bundles.</param>
        private static void RegisterJqgridLayoutScripts(BundleCollection bundles)
        {
            var bundle = new ScriptBundle("~/bundles/jqGridLayoutScripts")
                .Include("~/Scripts/global/jquery.jqgrid.additional.js")
                .Include("~/Scripts/Library/jquery.jqGrid.en.js")
                .Include("~/Scripts/Library/jquery.jqGrid.min.js")
                .Include("~/Scripts/Plugin/DateRangePicker/date.min.js")
                .Include("~/Scripts/Plugin/MonthPickr/monpickr.js")
                .Include("~/Scripts/Plugin/DateRangePicker/daterangepicker.js")
                .Include("~/Scripts/Plugin/LinqJs/JSLINQ.js")
                .Include("~/Scripts/Report/report.js")
                .Include("~/Scripts/global/prototype.js")
                .Include("~/Scripts/Plugin/FlatPopup/jquery.custom-scrollbar.js")
                .Include("~/Scripts/Plugin/FlatPopup/jquery.flatpopup.js");

            bundle.Orderer = new NonOrderingBundleOrderer();
            bundles.Add(bundle);
        }

        /// <summary>
        /// Registers the bet list layout scripts.
        /// </summary>
        /// <param name="bundles">The bundles.</param>
        private static void RegisterBetListLayoutScripts(BundleCollection bundles)
        {
            var bundle = new ScriptBundle("~/bundles/BetListLayoutScripts")
                .Include("~/Scripts/global/jquery.jqgrid.additional.js")
                .Include("~/Scripts/Library/jquery.jqGrid.en.js")
                .Include("~/Scripts/Library/jquery.jqGrid.min.js")
                .Include("~/Scripts/Plugin/MonthPickr/monpickr.js")
                .Include("~/Scripts/Plugin/FlatPopup/jquery.custom-scrollbar.js")
                .Include("~/Scripts/Plugin/FlatPopup/jquery.flatpopup.js")
                .Include("~/Scripts/Report/report.js")
                .Include("~/scripts/global/Paging.js")
                .Include("~/Scripts/Plugin/DateRangePicker/date.min.js")
                .Include("~/Scripts/Plugin/DateRangePicker/daterangepicker.js")
                .Include("~/Scripts/BetList/betlist.option.js")
                .Include("~/Scripts/BetList/betlist.js");

            bundle.Orderer = new NonOrderingBundleOrderer();
            bundles.Add(bundle);
        }

        /// <summary>
        /// Registers the bet list layout no standard scripts.
        /// </summary>
        /// <param name="bundles">The bundles.</param>
        private static void RegisterBetListLayoutNoStandardScripts(BundleCollection bundles)
        {
            var bundle = new ScriptBundle("~/bundles/BetListLayoutNoStandardScripts")
                .Include("~/Scripts/global/jquery.jqgrid.additional.js")
                .Include("~/Scripts/Library/jquery.jqGrid.en.js")
                .Include("~/Scripts/Library/jquery.jqGrid.min.js")
                .Include("~/Scripts/Plugin/MonthPickr/monpickr.js")
                .Include("~/Scripts/Report/report.js")
                .Include("~/scripts/global/Paging.js")
                .Include("~/Scripts/Plugin/DateRangePicker/date.min.js")
                .Include("~/Scripts/Plugin/DateRangePicker/daterangepicker.js")
                .Include("~/Scripts/BetList/betlist.option.js")
                .Include("~/Scripts/BetList/betlist.js");

            bundle.Orderer = new NonOrderingBundleOrderer();
            bundles.Add(bundle);
        }

        /// <summary>
        /// Registers the bet list simple layout scripts.
        /// </summary>
        /// <param name="bundles">The bundles.</param>
        private static void RegisterBetListSimpleLayoutScripts(BundleCollection bundles)
        {
            var bundle = new ScriptBundle("~/bundles/BetListSimpleLayoutScripts")
                .Include("~/Scripts/global/jquery.jqgrid.additional.js")
                .Include("~/Scripts/Library/jquery.jqGrid.en.js")
                .Include("~/Scripts/Library/jquery.jqGrid.min.js")
                .Include("~/Scripts/Plugin/MonthPickr/monpickr.js")
                .Include("~/Scripts/Report/report.js")
                .Include("~/scripts/global/Paging.js")
                .Include("~/Scripts/Plugin/DateRangePicker/date.min.js")
                .Include("~/Scripts/Plugin/DateRangePicker/daterangepicker.js")
                .Include("~/Scripts/BetList/betlist.option.js")
                .Include("~/Scripts/BetList/betlist.js")
                .Include("~/Scripts/BetList/commonbetlist.js");
            bundle.Orderer = new NonOrderingBundleOrderer();
            bundles.Add(bundle);
        }

        /// <summary>
        /// Registers the customer log layout scripts.
        /// </summary>
        /// <param name="bundles">The bundles.</param>
        private static void RegisterCustomerLogLayoutScripts(BundleCollection bundles)
        {
            var bundle = new ScriptBundle("~/bundles/CustomerLogLayoutScripts")
                .Include("~/Scripts/Plugin/MonthPickr/monpickr.js")
                .Include("~/Scripts/global/jquery.jqgrid.additional.js")
                .Include("~/Scripts/Library/jquery.jqGrid.en.js")
                .Include("~/Scripts/Library/jquery.jqGrid.min.js")
                .Include("~/Scripts/Plugin/FlatPopup/jquery.custom-scrollbar.js")
                .Include("~/Scripts/Plugin/FlatPopup/jquery.flatpopup.js")
                .Include("~/Scripts/Customer/CustomerLog/customerlog.js");

            bundle.Orderer = new NonOrderingBundleOrderer();
            bundles.Add(bundle);
        }

        /// <summary>
        /// Registers the report layout scripts.
        /// </summary>
        /// <param name="bundles">The bundles.</param>
        private static void RegisterReportLayoutScripts(BundleCollection bundles)
        {
            var bundle = new ScriptBundle("~/bundles/ReportLayoutScripts")
                .Include("~/Scripts/Report/report.js")
                .Include("~/scripts/global/Paging.js")
                .Include("~/Scripts/Plugin/DateRangePicker/date.min.js")
                .Include("~/Scripts/Plugin/MonthPickr/monpickr.js")
                .Include("~/Scripts/Plugin/DateRangePicker/daterangepicker.js")
                .Include("~/Scripts/Plugin/FlatPopup/jquery.custom-scrollbar.js")
                .Include("~/Scripts/Plugin/FlatPopup/jquery.flatpopup.js");

            bundle.Orderer = new NonOrderingBundleOrderer();
            bundles.Add(bundle);
        }

        /// <summary>
        /// Registers the report layout settled match count scripts.
        /// </summary>
        /// <param name="bundles">The bundles.</param>
        private static void RegisterReportLayoutSettledMatchCountScripts(BundleCollection bundles)
        {
            var bundle = new ScriptBundle("~/bundles/ReportLayoutSettledMatchCountScripts")
                .Include("~/Scripts/Plugin/MonthPickr/monpickr.js")
                .Include("~/Scripts/Report/report.js")
                .Include("~/scripts/global/Paging.js")
                .Include("~/Scripts/Plugin/DateRangePicker/date.min.js")
                .Include("~/Scripts/Plugin/DateRangePicker/daterangepicker.js");

            bundle.Orderer = new NonOrderingBundleOrderer();
            bundles.Add(bundle);
        }

        /// <summary>
        /// Registers the report table layout scripts.
        /// </summary>
        /// <param name="bundles">The bundles.</param>
        private static void RegisterReportTableLayoutScripts(BundleCollection bundles)
        {
            var bundle = new ScriptBundle("~/bundles/ReportTableLayoutScripts")
                .Include("~/Scripts/Plugin/DateRangePicker/date.min.js")
                .Include("~/Scripts/Plugin/MonthPickr/monpickr.js")
                .Include("~/Scripts/Report/report.js")
                .Include("~/scripts/global/Paging.js")
                .Include("~/Scripts/Plugin/DateRangePicker/daterangepicker.js")
                .Include("~/Scripts/Plugin/FlatPopup/jquery.custom-scrollbar.js")
                .Include("~/Scripts/Plugin/FlatPopup/jquery.flatpopup.js");

            bundle.Orderer = new NonOrderingBundleOrderer();
            bundles.Add(bundle);
        }

        #endregion JavaScripts Shared Layout

        #region CSS Shared Layout

        /// <summary>
        /// Registers the account layout styles.
        /// </summary>
        /// <param name="bundles">The bundles.</param>
        private static void RegisterAccountLayoutStyles(BundleCollection bundles)
        {
            var bundle = new StyleBundle("~/bundles/AccountLayoutStyles")
                .Include("~/Content/Library/start/jquery-ui.min.css", new CssRewriteUrlTransform())
                .Include("~/Content/Global/lrf.global.content.css", new CssRewriteUrlTransform())
                .Include("~/Content/FlatPopup/jquery.custom-scrollbar.css", new CssRewriteUrlTransform())
                .Include("~/Content/FlatPopup/jquery.flatpopup.css", new CssRewriteUrlTransform());

            bundle.Orderer = new NonOrderingBundleOrderer();
            bundles.Add(bundle);
        }

        /// <summary>
        /// Registers the bet list layout styles.
        /// </summary>
        /// <param name="bundles">The bundles.</param>
        private static void RegisterBetListLayoutStyles(BundleCollection bundles)
        {
            var bundle = new StyleBundle("~/bundles/BetListLayoutStyles")
                .Include("~/Content/Library/smoothness/jqgrid-custom.css", new CssRewriteUrlTransform())
                .Include("~/Content/Library/ui.jqgrid/jqgrid.extension.css", new CssRewriteUrlTransform())
                .Include("~/Content/Library/start/jquery-ui.min.css", new CssRewriteUrlTransform())
                .Include("~/Content/Library/ui.jqgrid/ui.jqgrid.css", new CssRewriteUrlTransform())
                .Include("~/Content/Global/grid26.css", new CssRewriteUrlTransform())
                .Include("~/Content/Global/lrf.global.content.css", new CssRewriteUrlTransform())
                .Include("~/Content/Plugin/MonthPickr/monPickr.css", new CssRewriteUrlTransform())
                .Include("~/Content/Global/Paging.css", new CssRewriteUrlTransform())
                .Include("~/Content/Plugin/DateRangePicker/daterangepicker.css", new CssRewriteUrlTransform())
                .Include("~/Content/BetList/betlist.core.css", new CssRewriteUrlTransform())
                .Include("~/Content/FlatPopup/jquery.custom-scrollbar.css", new CssRewriteUrlTransform())
                .Include("~/Content/FlatPopup/jquery.flatpopup.css", new CssRewriteUrlTransform());

            bundle.Orderer = new NonOrderingBundleOrderer();
            bundles.Add(bundle);
        }

        /// <summary>
        /// Registers the bet list layout no standard styles.
        /// </summary>
        /// <param name="bundles">The bundles.</param>
        private static void RegisterBetListLayoutNoStandardStyles(BundleCollection bundles)
        {
            var bundle = new StyleBundle("~/bundles/BetListLayoutNoStandardStyles")
                .Include("~/Content/Library/smoothness/jqgrid-custom.css", new CssRewriteUrlTransform())
                .Include("~/Content/Library/ui.jqgrid/jqgrid.extension.css", new CssRewriteUrlTransform())
                .Include("~/Content/Library/start/jquery-ui.min.css", new CssRewriteUrlTransform())
                .Include("~/Content/Library/ui.jqgrid/ui.jqgrid.css", new CssRewriteUrlTransform())
                .Include("~/Content/Global/grid26.css", new CssRewriteUrlTransform())
                .Include("~/Content/Global/lrf.global.content.css", new CssRewriteUrlTransform())
                .Include("~/Content/Plugin/MonthPickr/monPickr.css", new CssRewriteUrlTransform())
                .Include("~/Content/Global/Paging.css", new CssRewriteUrlTransform())
                .Include("~/Content/Plugin/DateRangePicker/daterangepicker.css", new CssRewriteUrlTransform())
                .Include("~/Content/BetList/betlist.core.css", new CssRewriteUrlTransform());

            bundle.Orderer = new NonOrderingBundleOrderer();
            bundles.Add(bundle);
        }

        /// <summary>
        /// Registers the bet list simple layout styles.
        /// </summary>
        /// <param name="bundles">The bundles.</param>
        private static void RegisterBetListSimpleLayoutStyles(BundleCollection bundles)
        {
            var bundle = new StyleBundle("~/bundles/BetListSimpleLayoutStyles")
                .Include("~/Content/Library/smoothness/jqgrid-custom.css", new CssRewriteUrlTransform())
                .Include("~/Content/Library/ui.jqgrid/jqgrid.extension.css", new CssRewriteUrlTransform())
                .Include("~/Content/Library/start/jquery-ui.min.css", new CssRewriteUrlTransform())
                .Include("~/Content/Library/ui.jqgrid/ui.jqgrid.css", new CssRewriteUrlTransform())
                .Include("~/Content/Global/grid26.css", new CssRewriteUrlTransform())
                .Include("~/Content/Global/lrf.global.content.css", new CssRewriteUrlTransform())
                .Include("~/Content/Plugin/MonthPickr/monPickr.css", new CssRewriteUrlTransform())
                .Include("~/Content/Global/Paging.css", new CssRewriteUrlTransform())
                .Include("~/Content/Plugin/DateRangePicker/daterangepicker.css", new CssRewriteUrlTransform())
                .Include("~/Content/BetList/betlist.core.css", new CssRewriteUrlTransform());

            bundle.Orderer = new NonOrderingBundleOrderer();
            bundles.Add(bundle);
        }

        /// <summary>
        /// Registers the bet setting layout styles.
        /// </summary>
        /// <param name="bundles">The bundles.</param>
        private static void RegisterBetSettingLayoutStyles(BundleCollection bundles)
        {
            var bundle = new StyleBundle("~/bundles/BetSettingLayoutStyles")
                .Include("~/Content/Library/start/jquery-ui.min.css", new CssRewriteUrlTransform())
                .Include("~/Content/Global/grid26.css", new CssRewriteUrlTransform())
                .Include("~/Content/Global/lrf.global.grid.css", new CssRewriteUrlTransform())
                .Include("~/Content/Global/lrf.global.content.css", new CssRewriteUrlTransform())
                .Include("~/Content/Global/lrf.global.reportable.css", new CssRewriteUrlTransform())
                .Include("~/Content/Customer/BetSetting/BetSetting.css", new CssRewriteUrlTransform());

            bundle.Orderer = new NonOrderingBundleOrderer();
            bundles.Add(bundle);
        }

        /// <summary>
        /// Registers the customer log layout styles.
        /// </summary>
        /// <param name="bundles">The bundles.</param>
        private static void RegisterCustomerLogLayoutStyles(BundleCollection bundles)
        {
            var bundle = new StyleBundle("~/bundles/CustomerLogLayoutStyles")
                .Include("~/Content/Global/grid26.css", new CssRewriteUrlTransform())
                .Include("~/Content/Global/lrf.global.content.css", new CssRewriteUrlTransform())
                .Include("~/Content/Global/lrf.global.reportable.css", new CssRewriteUrlTransform())
                .Include("~/Content/Library/start/jquery-ui.min.css", new CssRewriteUrlTransform())
                .Include("~/Content/Plugin/MonthPickr/monPickr.css", new CssRewriteUrlTransform())
                .Include("~/Content/Library/smoothness/jqgrid-custom.css", new CssRewriteUrlTransform())
                .Include("~/Content/Library/ui.jqgrid/jqgrid.extension.css", new CssRewriteUrlTransform())
                .Include("~/Content/Library/start/jquery-ui.min.css", new CssRewriteUrlTransform())
                .Include("~/Content/Library/ui.jqgrid/ui.jqgrid.css", new CssRewriteUrlTransform())
                .Include("~/Content/FlatPopup/jquery.custom-scrollbar.css", new CssRewriteUrlTransform())
                .Include("~/Content/FlatPopup/jquery.flatpopup.css", new CssRewriteUrlTransform())
                .Include("~/Content/Customer/BetSetting/BetSetting.css", new CssRewriteUrlTransform())
                .Include("~/Content/Customer/CustomerLog/customerlog.css", new CssRewriteUrlTransform());

            bundle.Orderer = new NonOrderingBundleOrderer();
            bundles.Add(bundle);
        }

        /// <summary>
        /// Registers the forecast layout styles.
        /// </summary>
        /// <param name="bundles">The bundles.</param>
        private static void RegisterForecastLayoutStyles(BundleCollection bundles)
        {
            var bundle = new StyleBundle("~/bundles/ForecastLayoutStyles")
                .Include("~/Content/Global/grid26.css", new CssRewriteUrlTransform())
                .Include("~/Content/Global/lrf.global.content.css", new CssRewriteUrlTransform())
                .Include("~/Content/Global/lrf.global.reportable.css", new CssRewriteUrlTransform())
                .Include("~/Content/Forecast/forecast.css", new CssRewriteUrlTransform())
                .Include("~/Content/TotalBets/refreshcountdown.css", new CssRewriteUrlTransform())
                .Include("~/Content/Library/start/jquery-ui.min.css", new CssRewriteUrlTransform())
                .Include("~/Content/FlatPopup/jquery.custom-scrollbar.css", new CssRewriteUrlTransform())
                .Include("~/Content/FlatPopup/jquery.flatpopup.css", new CssRewriteUrlTransform())
                .Include("~/Content/UserGuide/user-guide.css", new CssRewriteUrlTransform());

            bundle.Orderer = new NonOrderingBundleOrderer();
            bundles.Add(bundle);
        }

        /// <summary>
        /// Registers the group commission layout styles.
        /// </summary>
        /// <param name="bundles">The bundles.</param>
        private static void RegisterGroupCommLayoutStyles(BundleCollection bundles)
        {
            var bundle = new StyleBundle("~/bundles/GroupCommLayoutStyles")
                .Include("~/Content/Library/start/jquery-ui.min.css", new CssRewriteUrlTransform())
                .Include("~/Content/Global/lrf.global.content.css", new CssRewriteUrlTransform())
                .Include("~/Content/Global/lrf.global.reportable.css", new CssRewriteUrlTransform())
                .Include("~/Content/Customer/groupcomm.css", new CssRewriteUrlTransform())
                .Include("~/Content/Global/grid26.css", new CssRewriteUrlTransform())
                .Include("~/Content/FlatPopup/jquery.custom-scrollbar.css", new CssRewriteUrlTransform())
                .Include("~/Content/FlatPopup/jquery.flatpopup.css", new CssRewriteUrlTransform());

            bundle.Orderer = new NonOrderingBundleOrderer();
            bundles.Add(bundle);
        }

        /// <summary>
        /// Register jQuery Grid layout styles.
        /// </summary>
        /// <param name="bundles">The bundles.</param>
        private static void RegisterjqGridLayoutStyles(BundleCollection bundles)
        {
            var bundle = new StyleBundle("~/bundles/jqGridLayoutStyles")
                .Include("~/Content/Library/smoothness/jqgrid-custom.css", new CssRewriteUrlTransform())
                .Include("~/Content/Library/ui.jqgrid/jqgrid.extension.css", new CssRewriteUrlTransform())
                .Include("~/Content/Library/start/jquery-ui.min.css", new CssRewriteUrlTransform())
                .Include("~/Content/Library/ui.jqgrid/ui.jqgrid.css", new CssRewriteUrlTransform())
                .Include("~/Content/Global/grid26.css", new CssRewriteUrlTransform())
                .Include("~/Content/Global/lrf.global.content.css", new CssRewriteUrlTransform())
                .Include("~/Content/Plugin/MonthPickr/monPickr.css", new CssRewriteUrlTransform())
                .Include("~/Content/Plugin/DateRangePicker/daterangepicker.css", new CssRewriteUrlTransform())
                .Include("~/Content/FlatPopup/jquery.custom-scrollbar.css", new CssRewriteUrlTransform())
                .Include("~/Content/FlatPopup/jquery.flatpopup.css", new CssRewriteUrlTransform());

            bundle.Orderer = new NonOrderingBundleOrderer();
            bundles.Add(bundle);
        }

        /// <summary>
        /// Registers the report layout styles.
        /// </summary>
        /// <param name="bundles">The bundles.</param>
        private static void RegisterReportLayoutStyles(BundleCollection bundles)
        {
            var bundle = new StyleBundle("~/bundles/ReportLayoutStyles")
                .Include("~/Content/Library/start/jquery-ui.min.css", new CssRewriteUrlTransform())
                .Include("~/Content/Global/grid26.css", new CssRewriteUrlTransform())
                .Include("~/Content/Global/lrf.global.grid.css", new CssRewriteUrlTransform())
                .Include("~/Content/Global/lrf.global.content.css", new CssRewriteUrlTransform())
                .Include("~/Content/Plugin/MonthPickr/monPickr.css", new CssRewriteUrlTransform())
                .Include("~/Content/Global/Paging.css", new CssRewriteUrlTransform())
                .Include("~/Content/Plugin/DateRangePicker/daterangepicker.css", new CssRewriteUrlTransform())
                .Include("~/Content/FlatPopup/jquery.custom-scrollbar.css", new CssRewriteUrlTransform())
                .Include("~/Content/FlatPopup/jquery.flatpopup.css", new CssRewriteUrlTransform());

            bundle.Orderer = new NonOrderingBundleOrderer();
            bundles.Add(bundle);
        }

        /// <summary>
        /// Registers the report layout settled match count styles.
        /// </summary>
        /// <param name="bundles">The bundles.</param>
        private static void RegisterReportLayoutSettledMatchCountStyles(BundleCollection bundles)
        {
            var bundle = new StyleBundle("~/bundles/ReportLayoutSettledMatchCountStyles")
                .Include("~/Content/Library/start/jquery-ui.min.css", new CssRewriteUrlTransform())
                .Include("~/Content/Global/grid26.css", new CssRewriteUrlTransform())
                .Include("~/Content/Global/lrf.global.grid.css", new CssRewriteUrlTransform())
                .Include("~/Content/Global/lrf.global.content.css", new CssRewriteUrlTransform())
                .Include("~/Content/Plugin/MonthPickr/monPickr.css", new CssRewriteUrlTransform())
                .Include("~/Content/Global/Paging.css", new CssRewriteUrlTransform())
                .Include("~/Content/Plugin/DateRangePicker/daterangepicker.css", new CssRewriteUrlTransform());

            bundle.Orderer = new NonOrderingBundleOrderer();
            bundles.Add(bundle);
        }

        /// <summary>
        /// Registers the report table layout styles.
        /// </summary>
        /// <param name="bundles">The bundles.</param>
        private static void RegisterReportTableLayoutStyles(BundleCollection bundles)
        {
            var bundle = new StyleBundle("~/bundles/ReportTableLayoutStyles")
                .Include("~/Content/Library/start/jquery-ui.min.css", new CssRewriteUrlTransform())
                .Include("~/Content/Global/grid26.css", new CssRewriteUrlTransform())
                .Include("~/Content/Global/lrf.global.content.css", new CssRewriteUrlTransform())
                .Include("~/Content/Global/lrf.global.reportable.css", new CssRewriteUrlTransform())
                .Include("~/Content/Plugin/MonthPickr/monPickr.css", new CssRewriteUrlTransform())
                .Include("~/Content/Global/Paging.css", new CssRewriteUrlTransform())
                .Include("~/Content/Plugin/DateRangePicker/daterangepicker.css", new CssRewriteUrlTransform())
                .Include("~/Content/FlatPopup/jquery.custom-scrollbar.css", new CssRewriteUrlTransform())
                .Include("~/Content/FlatPopup/jquery.flatpopup.css", new CssRewriteUrlTransform());

            bundle.Orderer = new NonOrderingBundleOrderer();
            bundles.Add(bundle);
        }

        /// <summary>
        /// Registers the total bets layout styles.
        /// </summary>
        /// <param name="bundles">The bundles.</param>
        private static void RegisterTotalBetsLayoutStyles(BundleCollection bundles)
        {
            var bundle = new StyleBundle("~/bundles/TotalBetsLayoutStyles")
                .Include("~/Content/Global/grid26.css", new CssRewriteUrlTransform())
                .Include("~/Content/Global/lrf.global.content.css", new CssRewriteUrlTransform())
                .Include("~/Content/Global/lrf.global.reportable.css", new CssRewriteUrlTransform())
                .Include("~/Content/TotalBets/totalbetsbase.css", new CssRewriteUrlTransform())
                .Include("~/Content/TotalBets/refreshcountdown.css", new CssRewriteUrlTransform())
                .Include("~/Content/Library/start/jquery-ui.min.css", new CssRewriteUrlTransform())
                .Include("~/Content/FlatPopup/jquery.custom-scrollbar.css", new CssRewriteUrlTransform())
                .Include("~/Content/FlatPopup/jquery.flatpopup.css", new CssRewriteUrlTransform())
                .Include("~/Content/UserGuide/user-guide.css", new CssRewriteUrlTransform());

            bundle.Orderer = new NonOrderingBundleOrderer();
            bundles.Add(bundle);
        }

        /// <summary>
        /// Registers the ware house layout styles.
        /// </summary>
        /// <param name="bundles">The bundles.</param>
        private static void RegisterWareHouseLayoutStyles(BundleCollection bundles)
        {
            var bundle = new StyleBundle("~/bundles/WareHouseLayoutStyes")
                .Include("~/Content/Library/start/jquery-ui.min.css", new CssRewriteUrlTransform())
                .Include("~/Content/Global/grid26.css", new CssRewriteUrlTransform())
                .Include("~/Content/Global/lrf.global.content.css", new CssRewriteUrlTransform())
                .Include("~/Content/Global/lrf.global.grid.css", new CssRewriteUrlTransform())
                .Include("~/Content/Plugin/DateRangePicker/daterangepicker.css", new CssRewriteUrlTransform())
                .Include("~/Content/Warehouse/Warehouse.css", new CssRewriteUrlTransform());

            bundle.Orderer = new NonOrderingBundleOrderer();
            bundles.Add(bundle);
        }

        #endregion CSS Shared Layout

        #region Javascript Views

        /// <summary>
        /// Registers the last bets monitoring view scripts.
        /// </summary>
        /// <param name="bundles">The bundles.</param>
        private static void RegisterLastBetsMonitoringViewScripts(BundleCollection bundles)
        {
            var bundle = new ScriptBundle("~/bundles/LastBetsMonitoringScripts")
                .Include("~/Scripts/Plugin/LinqJs/JSLINQ.js")
                .Include("~/Scripts/Global/prototype.js")
                .Include("~/Scripts/Plugin/MultiSelect/jquery.multiselect.js")
                .Include("~/Scripts/BetList/lastbetsmonitornew.js");
            bundle.Orderer = new NonOrderingBundleOrderer();
            bundles.Add(bundle);
        }

        /// <summary>
        /// Registers the customer commission update group view scripts.
        /// </summary>
        /// <param name="bundles">The bundles.</param>
        private static void RegisterCustomerCommUpdateGroupViewScripts(BundleCollection bundles)
        {
            var bundle = new ScriptBundle("~/bundles/CustomerCommUpdateGroupScripts")
                .Include("~/Scripts/Plugin/ColorPicker/spectrum.js")
                .Include("~/Scripts/Customer/CustomerComm/updategroup.js")
                .Include("~/Scripts/Customer/CustomerComm/groupcomm.js");
            bundle.Orderer = new NonOrderingBundleOrderer();
            bundles.Add(bundle);
        }

        /// <summary>
        /// Registers the customer log customers view scripts.
        /// </summary>
        /// <param name="bundles">The bundles.</param>
        private static void RegisterCustomerLogCustomersViewScripts(BundleCollection bundles)
        {
            var bundle = new ScriptBundle("~/bundles/CustomerLogCustomersScripts")
                .Include("~/Scripts/Plugin/MultiSelect/jquery.multiselect.js")
                .Include("~/Scripts/Global/Paging.js")
                .Include("~/Scripts/Customer/Customers.option.js")
                .Include("~/Scripts/Customer/Customers.js")
                .Include("~/Scripts/Customer/CustomerCommList.js");
            bundle.Orderer = new NonOrderingBundleOrderer();
            bundles.Add(bundle);
        }

        /// <summary>
        /// Registers the fore cast ah view scripts.
        /// </summary>
        /// <param name="bundles">The bundles.</param>
        private static void RegisterForeCastAHViewScripts(BundleCollection bundles)
        {
            var bundle = new ScriptBundle("~/bundles/ForeCastAHScripts")
                .Include("~/Scripts/Plugin/Refresh/refreshcountdown.js")
                .Include("~/Scripts/Forecast/forecastpopup.js")
                .Include("~/Scripts/Forecast/forecast.ah.js");
            bundle.Orderer = new NonOrderingBundleOrderer();
            bundles.Add(bundle);
        }

        /// <summary>
        /// Registers the header view scripts.
        /// </summary>
        /// <param name="bundles">The bundles.</param>
        private static void RegisterHeaderViewScripts(BundleCollection bundles)
        {
            var bundle = new ScriptBundle("~/bundles/HeaderScripts")
                .Include("~/Scripts/Library/jquery.min.js")
                .Include("~/Scripts/Library/jquery.blockUI.js")
                .Include("~/Scripts/Global/site.js")
                .Include("~/Scripts/Header/header.js")
                .Include("~/Scripts/Header/jquery.jclock.js");
            bundle.Orderer = new NonOrderingBundleOrderer();
            bundles.Add(bundle);
        }

        /// <summary>
        /// Registers the home view scripts.
        /// </summary>
        /// <param name="bundles">The bundles.</param>
        private static void RegisterHomeViewScripts(BundleCollection bundles)
        {
            var bundle = new ScriptBundle("~/bundles/HomeScripts")
                .Include("~/Scripts/Library/jquery.min.js")
                .Include("~/Scripts/Library/jquery-ui.min.js")
                .Include("~/Scripts/Library/jquery.cookie.js")
                .Include("~/Scripts/Library/jquery.blockUI.js")
                .Include("~/Scripts/Global/site.js");
            bundle.Orderer = new NonOrderingBundleOrderer();
            bundles.Add(bundle);
        }

        /// <summary>
        /// Registers the menu view scripts.
        /// </summary>
        /// <param name="bundles">The bundles.</param>
        private static void RegisterMenuViewScripts(BundleCollection bundles)
        {
            var bundle = new ScriptBundle("~/bundles/MenuScripts")
                .Include("~/Scripts/Library/jquery.min.js")
                .Include("~/Scripts/Library/jquery.autocomplete.js")
                .Include("~/Scripts/Menu/Menu.js");
            bundle.Orderer = new NonOrderingBundleOrderer();
            bundles.Add(bundle);
        }

        #endregion Javascript Views

        #region CSS Views

        /// <summary>
        /// Registers the edit account view styles.
        /// </summary>
        /// <param name="bundles">The bundles.</param>
        private static void RegisterEditAccountViewStyles(BundleCollection bundles)
        {
            var bundle = new StyleBundle("~/bundles/EditAccountViewStyles")
                .Include("~/Content/Library/start/jquery-ui.min.css", new CssRewriteUrlTransform())
                .Include("~/Content/Global/Global.css", new CssRewriteUrlTransform())
                .Include("~/Content/Global/lrf.global.content.css", new CssRewriteUrlTransform())
                .Include("~/Content/Global/lrf.global.reportable.css", new CssRewriteUrlTransform())
                .Include("~/Content/Account/AccountManagement.css", new CssRewriteUrlTransform());
            bundle.Orderer = new NonOrderingBundleOrderer();
            bundles.Add(bundle);
        }

        /// <summary>
        /// Registers the reset password view styles.
        /// </summary>
        /// <param name="bundles">The bundles.</param>
        private static void RegisterResetPasswordViewStyles(BundleCollection bundles)
        {
            var bundle = new StyleBundle("~/bundles/ResetPasswordViewStyles")
                .Include("~/Content/Library/start/jquery-ui.min.css", new CssRewriteUrlTransform())
                .Include("~/Content/Global/Global.css", new CssRewriteUrlTransform())
                .Include("~/Content/Global/lrf.global.content.css", new CssRewriteUrlTransform())
                .Include("~/Content/Global/lrf.global.reportable.css", new CssRewriteUrlTransform())
                .Include("~/Content/Account/AccountManagement.css", new CssRewriteUrlTransform());
            bundle.Orderer = new NonOrderingBundleOrderer();
            bundles.Add(bundle);
        }

        /// <summary>
        /// Registers the last bets monitoring view styles.
        /// </summary>
        /// <param name="bundles">The bundles.</param>
        private static void RegisterLastBetsMonitoringViewStyles(BundleCollection bundles)
        {
            var bundle = new StyleBundle("~/bundles/LastBetsMonitoringViewStyles")
                .Include("~/Content/Plugin/MultiSelect/jquery.multiselect.css", new CssRewriteUrlTransform())
                .Include("~/Content/BetList/lastbetmonitornew.css", new CssRewriteUrlTransform())
                .Include("~/Content/BetList/lastbetmonitor.table.css", new CssRewriteUrlTransform());
            bundle.Orderer = new NonOrderingBundleOrderer();
            bundles.Add(bundle);
        }

        /// <summary>
        /// Registers the customer commission update group view styles.
        /// </summary>
        /// <param name="bundles">The bundles.</param>
        private static void RegisterCustomerCommUpdateGroupViewStyles(BundleCollection bundles)
        {
            var bundle = new StyleBundle("~/bundles/CustomerCommUpdateGroupViewStyles")
                .Include("~/Content/Plugin/ColorPicker/Spectrum.css", new CssRewriteUrlTransform())
                .Include("~/Content/Global/grid26.css", new CssRewriteUrlTransform());
            bundle.Orderer = new NonOrderingBundleOrderer();
            bundles.Add(bundle);
        }

        /// <summary>
        /// Registers the customer log customers view styles.
        /// </summary>
        /// <param name="bundles">The bundles.</param>
        private static void RegisterCustomerLogCustomersViewStyles(BundleCollection bundles)
        {
            var bundle = new StyleBundle("~/bundles/CustomerLogCustomersViewStyles")
                .Include("~/Content/Customer/CustomerList.css", new CssRewriteUrlTransform())
                .Include("~/Content/Plugin/MultiSelect/jquery.multiselect.css", new CssRewriteUrlTransform())
                .Include("~/Content/Global/Paging.css", new CssRewriteUrlTransform());
            bundle.Orderer = new NonOrderingBundleOrderer();
            bundles.Add(bundle);
        }

        /// <summary>
        /// Registers the fore cast ah view styles.
        /// </summary>
        /// <param name="bundles">The bundles.</param>
        private static void RegisterForeCastAHViewStyles(BundleCollection bundles)
        {
            var bundle = new StyleBundle("~/bundles/ForeCastAHViewStyles")
                .Include("~/Content/Global/grid26.css", new CssRewriteUrlTransform())
                .Include("~/Content/Global/lrf.global.content.css", new CssRewriteUrlTransform())
                .Include("~/Content/Global/lrf.global.reportable.css", new CssRewriteUrlTransform())
                .Include("~/Content/Forecast/forecast.css", new CssRewriteUrlTransform())
                .Include("~/Content/Forecast/forecast.ah.css", new CssRewriteUrlTransform())
                .Include("~/Content/TotalBets/refreshcountdown.css", new CssRewriteUrlTransform())
                .Include("~/Content/Library/start/jquery-ui.min.css", new CssRewriteUrlTransform());
            bundle.Orderer = new NonOrderingBundleOrderer();
            bundles.Add(bundle);
        }

        /// <summary>
        /// Registers the header view styles.
        /// </summary>
        /// <param name="bundles">The bundles.</param>
        private static void RegisterHeaderViewStyles(BundleCollection bundles)
        {
            var bundle = new StyleBundle("~/bundles/HeaderViewStyles")
                .Include("~/Content/Themes/12Bet/Site.css", new CssRewriteUrlTransform())
                .Include("~/Content/Global/Global.css", new CssRewriteUrlTransform());
            bundle.Orderer = new NonOrderingBundleOrderer();
            bundles.Add(bundle);
        }

        /// <summary>
        /// Registers the home view styles.
        /// </summary>
        /// <param name="bundles">The bundles.</param>
        private static void RegisterHomeViewStyles(BundleCollection bundles)
        {
            var bundle = new StyleBundle("~/bundles/HomeViewStyles")
                .Include("~/Content/Global/Global.css", new CssRewriteUrlTransform())
                .Include("~/Content/Themes/12Bet/Site.css", new CssRewriteUrlTransform())
                .Include("~/Content/Layout/master.css", new CssRewriteUrlTransform());
            bundle.Orderer = new NonOrderingBundleOrderer();
            bundles.Add(bundle);
        }

        /// <summary>
        /// Registers the menu view CSS.
        /// </summary>
        /// <param name="bundles">The bundles.</param>
        private static void RegisterMenuViewCss(BundleCollection bundles)
        {
            var bundle = new StyleBundle("~/bundles/MenuViewCss")
                .Include("~/Content/Global/Global.css", new CssRewriteUrlTransform())
                .Include("~/Content/Themes/12Bet/Site.css", new CssRewriteUrlTransform());
            bundle.Orderer = new NonOrderingBundleOrderer();
            bundles.Add(bundle);
        }

        #endregion CSS Views
    }
}