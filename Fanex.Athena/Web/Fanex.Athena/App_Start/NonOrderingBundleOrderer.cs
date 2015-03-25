namespace Fanex.Athena
{
    using System.Collections.Generic;
    using System.IO;
    using System.Web.Optimization;

    /// <summary>
    /// Non ordering bundle order.
    /// </summary>
    internal class NonOrderingBundleOrderer : IBundleOrderer
    {
        #region IBundleOrderer Members

        public IEnumerable<BundleFile> OrderFiles(BundleContext context, IEnumerable<BundleFile> files)
        {
            return files;
        }

        #endregion
    }
}