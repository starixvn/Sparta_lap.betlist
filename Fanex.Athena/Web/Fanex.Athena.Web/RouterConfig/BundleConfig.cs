// -----------------------------------------------------------------------
// <copyright file="BundleConfig.cs" company="Nexecl">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Web.RouterConfig
{
    using Microsoft.Web.Optimization;

    /// <summary>
    /// Bundle Config class.
    /// </summary>
    public class BundleConfig
    {
        /// <summary>
        /// Register the bundles.
        /// </summary>
        /// <param name="bundles">The bundle collection.</param>
        public static void RegisterBundles(BundleCollection bundles)
        {
            /* var jQueryBundles = new ScriptBundle("~/bundles/jQuery")
                                      .IncludeDirectory("~/Scripts/libraries", "*.js", true);

             var jsBundles = new ScriptBundle("~/bundles/scripts")
                                     .IncludeDirectory("~/Scripts/extension", "*.js", true)
                                     .IncludeDirectory("~/Scripts/plugin", "*.js", true)
                                     .IncludeDirectory("~/Scripts/sites", "*.js", true);

             var cssBundles = new StyleBundle("~/Content/css")
                                    .IncludeDirectory("~/Content/libraries", "*.css", true)
                                    .IncludeDirectory("~/Content/extension", "*.css", true)
                                    .IncludeDirectory("~/Content/plugin", "*.css", true)
                                    .IncludeDirectory("~/Content/sites", "*.css", true);

             // Add the new bundle to Bundles collection in BundleTable
             bundles.Add(jQueryBundles);
             bundles.Add(jsBundles);
             bundles.Add(cssBundles);*/
        }
    }
}