// -----------------------------------------------------------------------
// <copyright file="IBetTypeDataAccess.cs" company="Nexcel Solution">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------
// <history>
//  <change who="Bravo" date="2013.09.19">Apply Style Cop</change>
// </history>
// -----------------------------------------------------------------------
namespace Fanex.Athena.DataAccess.Interface.Common
{
    using System.Collections.Generic;
    using System.Data;

    using Fanex.Athena.Models.MetaModel.Product;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface IBetTypeDataAccess
    {
        IDictionary<string, int> GetAllBettypes(IList<ProductOption> productOptions);

        DataTable GetBettypes(int licenseeId, string sports, string languageCode);
    }
}