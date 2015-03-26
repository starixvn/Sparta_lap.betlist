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
    }
}