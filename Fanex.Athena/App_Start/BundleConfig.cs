namespace Fanex.Athena
{
    using System.Web.Configuration;
    using System.Web.Optimization;

    /// <summary>
    /// Bundle config.
    /// </summary>
    public static class BundleConfig
    {
        private const string JqueryUiMinCssPath = "~/Content/Library/start/jquery-ui.min.css";

        private const string LrfGlobalContentCssPath = "~/Content/Global/lrf.global.content.css";

        private static IItemTransform cssRewriteUrlTransform;

        public static void RegisterBundles(BundleCollection bundles)
        {
            cssRewriteUrlTransform = new CssRewriteUrlTransformWrapper();
            RegisterLayoutBundles(bundles);
            RegisterViewBundles(bundles);

            BundleTable.EnableOptimizations = WebConfigurationManager.AppSettings["EnableBundle"] == "1";
        }

        private static void RegisterViewBundles(BundleCollection bundles)
        {
            RegisterEditAccountViewStyles(bundles);
            RegisterResetPasswordViewStyles(bundles);
        }

        private static void RegisterLayoutBundles(BundleCollection bundles)
        {
            RegisterLayoutScripts(bundles);
            RegisterAccountLayoutScripts(bundles);
            RegisterJqgridLayoutScripts(bundles);

            RegisterAccountLayoutStyles(bundles);

            RegisterjqGridLayoutStyles(bundles);

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
                        "~/Scripts/Languages/ViewResources.zh-Hans.js",
                        "~/Scripts/Languages/ViewResources.zh-TW.js",
                        "~/Scripts/Global/site.js",
                        "~/Scripts/Helper/ProductListHelper.js",
                        "~/Scripts/Global/query.string.js",
                        "~/Scripts/Plugin/Note/note.js");
            AddNewScriptBundle(bundles, bundle);
        }

        private static void AddNewScriptBundle(BundleCollection bundles, Bundle bundle)
        {
            bundle.Orderer = new NonOrderingBundleOrderer();
            bundles.Add(bundle);
        }

        private static void RegisterAccountLayoutScripts(BundleCollection bundles)
        {
            var bundle = new ScriptBundle("~/bundles/AccountLayoutScripts")
                .Include(
                        "~/Scripts/Plugin/FlatPopup/jquery.custom-scrollbar.js",
                        "~/Scripts/Plugin/FlatPopup/jquery.flatpopup.js");
            AddNewScriptBundle(bundles, bundle);
        }

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
                        "~/Scripts/Plugin/MultiSelect/jquery.multiselect.js",
                        "~/Scripts/Plugin/LinqJs/JSLINQ.js",
                        "~/Scripts/Report/report.js",
                        "~/Scripts/global/prototype.js");
            AddNewScriptBundle(bundles, bundle);
        }

        #endregion JavaScripts Shared Layout

        #region CSS Shared Layout

        private static void RegisterAccountLayoutStyles(BundleCollection bundles)
        {
            AddStyleBundle(
                bundles,
                "~/bundles/AccountLayoutStyles",
                JqueryUiMinCssPath,
                LrfGlobalContentCssPath,
                "~/Content/FlatPopup/jquery.custom-scrollbar.css",
                "~/Content/FlatPopup/jquery.flatpopup.css");
        }

        private static void RegisterjqGridLayoutStyles(BundleCollection bundles)
        {
            AddStyleBundle(
                bundles,
                "~/bundles/jqGridLayoutStyles",
                "~/Content/Library/smoothness/jqgrid-custom.css",
                "~/Content/Library/ui.jqgrid/jqgrid.extension.css",
                JqueryUiMinCssPath,
                "~/Content/Library/ui.jqgrid/ui.jqgrid.css",
                "~/Content/Global/grid26.css",
                LrfGlobalContentCssPath,
                "~/Content/Plugin/MonthPickr/monPickr.css",
                "~/Content/Plugin/DateRangePicker/daterangepicker.css",
                "~/Content/Plugin/MultiSelect/jquery.multiselect.css");
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
               "~/Content/Report/agcasinobalance.css",
               "~/Content/Plugin/Common/bread-crumb.css",
               "~/Content/Global/Paging.css");
        }

        #endregion Scripts & Styles Views

        #region CSS Views

        private static void RegisterEditAccountViewStyles(BundleCollection bundles)
        {
            AddStyleBundle(
                bundles,
                "~/bundles/EditAccountViewStyles",
                JqueryUiMinCssPath,
                "~/Content/Global/Global.css",
                LrfGlobalContentCssPath,
                "~/Content/Global/lrf.global.reportable.css",
                "~/Content/Account/AccountManagement.css");
        }

        private static void RegisterResetPasswordViewStyles(BundleCollection bundles)
        {
            AddStyleBundle(
                bundles,
                "~/bundles/ResetPasswordViewStyles",
                JqueryUiMinCssPath,
                "~/Content/Global/Global.css",
                LrfGlobalContentCssPath,
                "~/Content/Global/lrf.global.reportable.css",
                "~/Content/Account/AccountManagement.css");
        }

        #endregion CSS Views

        #region Private Utility Functions

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