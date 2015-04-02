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
            RegisterCore(bundles);
            RegisterJqgridLayout(bundles);
            RegisterBetListLayout(bundles);
            RegisterBetListLayoutNoStandard(bundles);
            RegisterBetListSimpleLayout(bundles);
            RegisterCustomerLogLayout(bundles);
            RegisterReportLayout(bundles);
            RegisterReportLayout(bundles);
            RegisterReportLayoutSettledMatchCount(bundles);
            RegisterReportTableLayout(bundles);


            BundleTable.EnableOptimizations = WebConfigurationManager.AppSettings["EnableBundle"] == "1";
        }

        /// <summary>
        /// Register Core.
        /// </summary>
        /// <param name="bundles">Bundles value.</param>
        private static void RegisterCore(BundleCollection bundles)
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
        /// Register script for grid layout.
        /// </summary>
        /// <param name="bundles">Bundle collection.</param>
        private static void RegisterJqgridLayout(BundleCollection bundles)
        {
            var bundle = new ScriptBundle("~/bundles/jqGridScripts")
                .Include("~/Scripts/global/jquery.jqgrid.additional.js")
                .Include("~/Scripts/Library/jquery.jqGrid.en.js")
                .Include("~/Scripts/Library/jquery.jqGrid.min.js")
                .Include("~/Scripts/Plugin/DateRangePicker/date.min.js")
                .Include("~/Scripts/Plugin/MonthPickr/monpickr.js")
                .Include("~/Scripts/Plugin/DateRangePicker/daterangepicker.js")
                .Include("~/Scripts/Plugin/MonthPickr/monpickr.js")
                .Include("~/Scripts/Plugin/LinqJs/JSLINQ.js")
                .Include("~/Scripts/Report/report.js")
                .Include("~/Scripts/global/prototype.js")
                .Include("~/Scripts/Plugin/FlatPopup/jquery.custom-scrollbar.js")
                .Include("~/Scripts/Plugin/FlatPopup/jquery.flatpopup.js");

            bundle.Orderer = new NonOrderingBundleOrderer();
            bundles.Add(bundle);
        }

        /// <summary>
        /// Register script for _BetListLayout.cshtml.
        /// </summary>
        /// <param name="bundles">Bundle collection.</param>
        private static void RegisterBetListLayout(BundleCollection bundles)
        {
            var bundle = new ScriptBundle("~/bundles/jqGridScriptsBetListLayout")
                .Include("~/Scripts/global/jquery.jqgrid.additional.js")
                .Include("~/Scripts/Library/jquery.jqGrid.en.js")
                .Include("~/Scripts/Library/jquery.jqGrid.min.js")
                .Include("~/Scripts/Plugin/MonthPickr/monpickr.js")
                .Include("~/Scripts/Plugin/FlatPopup/jquery.custom-scrollbar.js")
                .Include("~/Scripts/Plugin/FlatPopup/jquery.flatpopup.js")
                .Include("~/Scripts/Report/report.js")
                .Include("~/scripts/global/Paging.js")
                .Include("~/Scripts/Plugin/DateRangePicker/date.min.js")
                .Include("~/Scripts/Plugin/DateRangePicker/daterangepicker.js");

            bundle.Orderer = new NonOrderingBundleOrderer();
            bundles.Add(bundle);
        }

        /// <summary>
        /// Register script for _BetListLayoutNoStandard.cshtml.
        /// </summary>
        /// <param name="bundles">Bundle collection.</param>
        private static void RegisterBetListLayoutNoStandard(BundleCollection bundles)
        {
            var bundle = new ScriptBundle("~/bundles/jqGridScriptsBetListLayoutNoStandard")
                .Include("~/Scripts/global/jquery.jqgrid.additional.js")
                .Include("~/Scripts/Library/jquery.jqGrid.en.js")
                .Include("~/Scripts/Library/jquery.jqGrid.min.js")
                .Include("~/Scripts/Plugin/MonthPickr/monpickr.js")
                .Include("~/Scripts/Report/report.js")
                .Include("~/scripts/global/Paging.js")
                .Include("~/Scripts/Plugin/DateRangePicker/date.min.js")
                .Include("~/Scripts/Plugin/DateRangePicker/daterangepicker.js");

            bundle.Orderer = new NonOrderingBundleOrderer();
            bundles.Add(bundle);
        }

        /// <summary>
        /// Register script for _BetListSimpleLayout.cshtml.
        /// </summary>
        /// <param name="bundles">Bundle collection.</param>
        private static void RegisterBetListSimpleLayout(BundleCollection bundles)
        {
            var bundle = new ScriptBundle("~/bundles/jqGridScriptsBetListSimpleLayout")
                .Include("~/Scripts/global/jquery.jqgrid.additional.js")
                .Include("~/Scripts/Library/jquery.jqGrid.en.js")
                .Include("~/Scripts/Library/jquery.jqGrid.min.js")
                .Include("~/Scripts/Plugin/MonthPickr/monpickr.js")
                .Include("~/Scripts/Report/report.js")
                .Include("~/scripts/global/Paging.js")
                .Include("~/Scripts/Plugin/DateRangePicker/date.min.js")
                .Include("~/Scripts/Plugin/DateRangePicker/daterangepicker.js");

            bundle.Orderer = new NonOrderingBundleOrderer();
            bundles.Add(bundle);
        }

        /// <summary>
        /// Register script for _CustomerLogLayout.cshtml.
        /// </summary>
        /// <param name="bundles">Bundle collection.</param>
        private static void RegisterCustomerLogLayout(BundleCollection bundles)
        {
            var bundle = new ScriptBundle("~/bundles/jqGridScriptsCustomerLogLayout")
                .Include("~/Scripts/Plugin/MonthPickr/monpickr.js")
                .Include("~/Scripts/global/jquery.jqgrid.additional.js")
                .Include("~/Scripts/Library/jquery.jqGrid.en.js")
                .Include("~/Scripts/Library/jquery.jqGrid.min.js")
                .Include("~/Scripts/Plugin/FlatPopup/jquery.custom-scrollbar.js")
                .Include("~/Scripts/Plugin/FlatPopup/jquery.flatpopup.js");

            bundle.Orderer = new NonOrderingBundleOrderer();
            bundles.Add(bundle);
        }

        /// <summary>
        /// Register script for _ReportLayout.cshtml.
        /// </summary>
        /// <param name="bundles">Bundle collection.</param>
        private static void RegisterReportLayout(BundleCollection bundles)
        {
            var bundle = new ScriptBundle("~/bundles/jqGridScriptsReportLayout")
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
        /// Register script for ReportLayout.SettledMatchCount.cshtml.
        /// </summary>
        /// <param name="bundles">Bundle collection.</param>
        private static void RegisterReportLayoutSettledMatchCount(BundleCollection bundles)
        {
            var bundle = new ScriptBundle("~/bundles/jqGridScriptsReportLayoutSettledMatchCount")
                .Include("~/Scripts/Plugin/MonthPickr/monpickr.js")
                .Include("~/Scripts/Report/report.js")
                .Include("~/scripts/global/Paging.js")
                .Include("~/Scripts/Plugin/DateRangePicker/date.min.js")
                .Include("~/Scripts/Plugin/DateRangePicker/daterangepicker.js");

            bundle.Orderer = new NonOrderingBundleOrderer();
            bundles.Add(bundle);
        }

        /// <summary>
        /// Register script for _ReportTableLayout.cshtml.
        /// </summary>
        /// <param name="bundles">Bundle collection.</param>
        private static void RegisterReportTableLayout(BundleCollection bundles)
        {
            var bundle = new ScriptBundle("~/bundles/jqGridScriptsReportTableLayout")
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
    }
}