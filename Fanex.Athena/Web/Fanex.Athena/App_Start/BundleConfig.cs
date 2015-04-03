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
        /// Register Bundles.
        /// </summary>
        /// <param name="bundles">Bundles value.</param>
        public static void RegisterBundles(BundleCollection bundles)
        {
            RegisterCoreJs(bundles);
            RegisterAccountLayoutJs(bundles);
            RegisterBetSettingLayoutJs(bundles);
            RegisterForecastLayoutJs(bundles);
            RegisterGroupCommLayoutJs(bundles);
            RegisterTotalBetsLayoutJs(bundles);
            RegisterJqgridLayoutJs(bundles);
            RegisterBetListLayoutJs(bundles);
            RegisterBetListLayoutNoStandardJs(bundles);
            RegisterBetListSimpleLayoutJs(bundles);
            RegisterCustomerLogLayoutJs(bundles);
            RegisterReportLayoutJs(bundles);
            RegisterReportLayoutSettledMatchCountJs(bundles);
            RegisterReportTableLayoutJs(bundles);
            RegisterWareHouseLayoutJs(bundles);

            RegisterAccountLayoutCss(bundles);
            RegisterBetListtLayoutCss(bundles);
            RegisterBetListLayoutNoStandardCss(bundles);
            RegisterBetListSimpleLayoutCss(bundles);
            RegisterBetSettingLayoutCss(bundles);
            RegisterCustomerLogLayoutCss(bundles);
            RegisterForecastLayoutCss(bundles);
            RegisterGroupCommLayoutCss(bundles);
            RegisterjqGridLayoutCss(bundles);
            RegisterReportLayoutCss(bundles);
            RegisterReportLayoutSettledMatchCountCss(bundles);
            RegisterReportTableLayoutCss(bundles);
            RegisterTotalBetsLayoutCss(bundles);
            RegisterWareHouseLayoutCss(bundles);

            BundleTable.EnableOptimizations = WebConfigurationManager.AppSettings["EnableBundle"] == "1";
        }
        #region JavaScripts

        /// <summary>
        /// Registers the core java script.
        /// </summary>
        /// <param name="bundles">The bundles.</param>
        private static void RegisterCoreJs(BundleCollection bundles)
        {
            var bundle = new ScriptBundle("~/bundles/CoreJs")
                .Include("~/Scripts/Library/jquery.min.js")
                .Include("~/Scripts/Library/jquery-ui.min.js")
                .Include("~/Scripts/Library/BigDecimal-all-last.min.js")
                .Include("~/Scripts/Library/jquery.cookie.js")
                .Include("~/Scripts/Library/jquery.blockUI.js")
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
        /// Registers the account layout JS.
        /// </summary>
        /// <param name="bundles">The bundles.</param>
        private static void RegisterAccountLayoutJs(BundleCollection bundles)
        {
            var bundle = new ScriptBundle("~/bundles/AccountLayoutScripts")
                .Include("~/Scripts/Plugin/FlatPopup/jquery.custom-scrollbar.js")
                .Include("~/Scripts/Plugin/FlatPopup/jquery.flatpopup.js");
            bundle.Orderer = new NonOrderingBundleOrderer();
            bundles.Add(bundle);
        }

        /// <summary>
        /// Registers the bet setting layout JS.
        /// </summary>
        /// <param name="bundles">The bundles.</param>
        private static void RegisterBetSettingLayoutJs(BundleCollection bundles)
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
        /// Registers the forecast layout JS.
        /// </summary>
        /// <param name="bundles">The bundles.</param>
        private static void RegisterForecastLayoutJs(BundleCollection bundles)
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
        /// Registers the group commission layout JS.
        /// </summary>
        /// <param name="bundles">The bundles.</param>
        private static void RegisterGroupCommLayoutJs(BundleCollection bundles)
        {
            var bundle = new ScriptBundle("~/bundles/GroupCommLayoutScripts")
                .Include("~/Scripts/Plugin/FlatPopup/jquery.custom-scrollbar.js")
                .Include("~/Scripts/Plugin/FlatPopup/jquery.flatpopup.js");
            bundle.Orderer = new NonOrderingBundleOrderer();
            bundles.Add(bundle);
        }

        /// <summary>
        /// Registers the total bets layout JS.
        /// </summary>
        /// <param name="bundles">The bundles.</param>
        private static void RegisterTotalBetsLayoutJs(BundleCollection bundles)
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
        /// Registers the ware house layout JS.
        /// </summary>
        /// <param name="bundles">The bundles.</param>
        private static void RegisterWareHouseLayoutJs(BundleCollection bundles)
        {
            var bundle = new ScriptBundle("~/bundles/WareHouseLayoutScripts")
                .Include("~/Scripts/Plugin/DateRangePicker/date.min.js")
                .Include("~/Scripts/Plugin/DateRangePicker/daterangepicker.js");
            bundle.Orderer = new NonOrderingBundleOrderer();
            bundles.Add(bundle);
        }

        /// <summary>
        /// Registers the jQuery grid layout JS.
        /// </summary>
        /// <param name="bundles">The bundles.</param>
        private static void RegisterJqgridLayoutJs(BundleCollection bundles)
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
        /// Registers the bet list layout JS.
        /// </summary>
        /// <param name="bundles">The bundles.</param>
        private static void RegisterBetListLayoutJs(BundleCollection bundles)
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
        /// Registers the bet list layout no standard JS.
        /// </summary>
        /// <param name="bundles">The bundles.</param>
        private static void RegisterBetListLayoutNoStandardJs(BundleCollection bundles)
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
        /// Registers the bet list simple layout JS.
        /// </summary>
        /// <param name="bundles">The bundles.</param>
        private static void RegisterBetListSimpleLayoutJs(BundleCollection bundles)
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
        /// Registers the customer log layout JS.
        /// </summary>
        /// <param name="bundles">The bundles.</param>
        private static void RegisterCustomerLogLayoutJs(BundleCollection bundles)
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
        /// Registers the report layout JS.
        /// </summary>
        /// <param name="bundles">The bundles.</param>
        private static void RegisterReportLayoutJs(BundleCollection bundles)
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
        /// Registers the report layout settled match count JS.
        /// </summary>
        /// <param name="bundles">The bundles.</param>
        private static void RegisterReportLayoutSettledMatchCountJs(BundleCollection bundles)
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
        /// Registers the report table layout JS.
        /// </summary>
        /// <param name="bundles">The bundles.</param>
        private static void RegisterReportTableLayoutJs(BundleCollection bundles)
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
        #endregion

        #region CSS
        /// <summary>
        /// Registers the account layout CSS.
        /// </summary>
        /// <param name="bundles">The bundles.</param>
        private static void RegisterAccountLayoutCss(BundleCollection bundles)
        {
            var bundle = new StyleBundle("~/bundles/AccountLayoutCss")
                .Include("~/Content/Library/start/jquery-ui.min.css", new CssRewriteUrlTransform())
                .Include("~/Content/Global/lrf.global.content.css", new CssRewriteUrlTransform())
                .Include("~/Content/FlatPopup/jquery.custom-scrollbar.css", new CssRewriteUrlTransform())
                .Include("~/Content/FlatPopup/jquery.flatpopup.css", new CssRewriteUrlTransform());

            bundle.Orderer = new NonOrderingBundleOrderer();
            bundles.Add(bundle);
        }

        /// <summary>
        /// Registers the bet list layout CSS.
        /// </summary>
        /// <param name="bundles">The bundles.</param>
        private static void RegisterBetListtLayoutCss(BundleCollection bundles)
        {
            var bundle = new StyleBundle("~/bundles/BetListLayoutCss")
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
        /// Registers the bet list layout no standard CSS.
        /// </summary>
        /// <param name="bundles">The bundles.</param>
        private static void RegisterBetListLayoutNoStandardCss(BundleCollection bundles)
        {
            var bundle = new StyleBundle("~/bundles/BetListLayoutNoStandardCss")
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
        /// Registers the bet list simple layout CSS.
        /// </summary>
        /// <param name="bundles">The bundles.</param>
        private static void RegisterBetListSimpleLayoutCss(BundleCollection bundles)
        {
            var bundle = new StyleBundle("~/bundles/BetListSimpleLayoutCss")
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
        /// Registers the bet setting layout CSS.
        /// </summary>
        /// <param name="bundles">The bundles.</param>
        private static void RegisterBetSettingLayoutCss(BundleCollection bundles)
        {
            var bundle = new StyleBundle("~/bundles/BetSettingLayoutCss")
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
        /// Registers the customer log layout CSS.
        /// </summary>
        /// <param name="bundles">The bundles.</param>
        private static void RegisterCustomerLogLayoutCss(BundleCollection bundles)
        {
            var bundle = new StyleBundle("~/bundles/CustomerLogLayoutCss")
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
        /// Registers the forecast layout CSS.
        /// </summary>
        /// <param name="bundles">The bundles.</param>
        private static void RegisterForecastLayoutCss(BundleCollection bundles)
        {
            var bundle = new StyleBundle("~/bundles/ForecastLayoutCss")
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
        /// Registers the group commission layout CSS.
        /// </summary>
        /// <param name="bundles">The bundles.</param>
        private static void RegisterGroupCommLayoutCss(BundleCollection bundles)
        {
            var bundle = new StyleBundle("~/bundles/GroupCommLayoutCss")
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
        /// Register jQuery grid layout CSS.
        /// </summary>
        /// <param name="bundles">The bundles.</param>
        private static void RegisterjqGridLayoutCss(BundleCollection bundles)
        {
            var bundle = new StyleBundle("~/bundles/jqGridLayoutCss")
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
        /// Registers the report layout CSS.
        /// </summary>
        /// <param name="bundles">The bundles.</param>
        private static void RegisterReportLayoutCss(BundleCollection bundles)
        {
            var bundle = new StyleBundle("~/bundles/ReportLayoutCss")
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
        /// Registers the report layout settled match count CSS.
        /// </summary>
        /// <param name="bundles">The bundles.</param>
        private static void RegisterReportLayoutSettledMatchCountCss(BundleCollection bundles)
        {
            var bundle = new StyleBundle("~/bundles/ReportLayoutSettledMatchCountCss")
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
        /// Registers the report table layout CSS.
        /// </summary>
        /// <param name="bundles">The bundles.</param>
        private static void RegisterReportTableLayoutCss(BundleCollection bundles)
        {
            var bundle = new StyleBundle("~/bundles/ReportTableLayoutCss")
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
        /// Registers the total bets layout CSS.
        /// </summary>
        /// <param name="bundles">The bundles.</param>
        private static void RegisterTotalBetsLayoutCss(BundleCollection bundles)
        {
            var bundle = new StyleBundle("~/bundles/TotalBetsLayoutCss")
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
        /// Registers the ware house layout CSS.
        /// </summary>
        /// <param name="bundles">The bundles.</param>
        private static void RegisterWareHouseLayoutCss(BundleCollection bundles)
        {
            var bundle = new StyleBundle("~/bundles/WareHouseLayoutCss")
                .Include("~/Content/Library/start/jquery-ui.min.css", new CssRewriteUrlTransform())
                .Include("~/Content/Global/grid26.css", new CssRewriteUrlTransform())
                .Include("~/Content/Global/lrf.global.content.css", new CssRewriteUrlTransform())
                .Include("~/Content/Global/lrf.global.grid.css", new CssRewriteUrlTransform())
                .Include("~/Content/Plugin/DateRangePicker/daterangepicker.css", new CssRewriteUrlTransform())
                .Include("~/Content/Warehouse/Warehouse.css", new CssRewriteUrlTransform());

            bundle.Orderer = new NonOrderingBundleOrderer();
            bundles.Add(bundle);
        }
        #endregion
    }
}