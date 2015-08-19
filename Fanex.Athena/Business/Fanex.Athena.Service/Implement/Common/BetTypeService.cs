// -----------------------------------------------------------------------
// <copyright file="BetTypeService.cs" company="Nexcel">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Service.Implement.Common
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Fanex.Athena.DataAccess.Implement.Common;
    using Fanex.Athena.DataAccess.Interface.Common;
    using Fanex.Athena.Models.DTO.Common;
    using Fanex.Athena.Models.MetaModel.Product;
    using Fanex.Athena.Service.Interface.Common;

    /// <summary>
    ///     TODO: Update summary.
    /// </summary>
    public class BetTypeService : IBetTypeService
    {
        private readonly IBetTypeDataAccess _betTypeDataAccess = new BetTypeDataAccess();

        public IDictionary<string, int> GetAllBettypes(IList<ProductOption> productOptions)
        {
            return _betTypeDataAccess.GetAllBettypes(productOptions);
        }

        /// <summary>
        /// Get bet type.
        /// </summary>
        /// <param name="licenseeId">The licensee id.</param>
        /// <param name="sports">The sports.</param>
        /// <param name="languageCode">The language code.</param>
        /// <returns>The bet type list.</returns>
        public IList<CheckedItem> GetBettypes(int licenseeId, IList<SportProductItem> sports, string languageCode)
        {
            const string SportXMLItemPattern = "<product ID=\"{0}\" Sport=\"{1}\"> </product>";
            var sportItemXML = new StringBuilder();

            if (sports != null && sports.Count > 0 && licenseeId > 0)
            {
                var products = sports.Where(i => i.ParentId == -1);

                foreach (var product in products)
                {
                    var childSports = sports.Where(i => i.ParentId == product.ID && i.ParentId > -1)
                        .Select(i => i.ID).ToArray();

                    sportItemXML.Append(string.Format(SportXMLItemPattern, product.ID, string.Join(",", childSports)));
                }

                var sportsParameters = "<products>" + sportItemXML.ToString() + "</products>";
                return _betTypeDataAccess.GetBettypes(licenseeId, sportsParameters, languageCode);
            }

            return new List<CheckedItem>();
        }
    }
}