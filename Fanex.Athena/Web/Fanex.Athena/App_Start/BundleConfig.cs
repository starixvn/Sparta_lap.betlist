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
                .Include("~/Scripts/Library/jquery.blockUI.js");

            bundle.Orderer = new NonOrderingBundleOrderer();
            bundles.Add(bundle);
        }
    }
}