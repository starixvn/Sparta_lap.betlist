// -----------------------------------------------------------------------
// <copyright file="IBetTypeDataAccess.cs" company="Nexcel Solution">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.DataAccess.Core.Common.Interfaces
{
    using System.Collections.Generic;
    using Fanex.Athena.Model.Core.DTO.Common;
    using Fanex.Athena.Model.Core.MetaModel.Product;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface IBetTypeDataAccess
    {
        IDictionary<string, int> GetAllBettypes(IList<ProductOption> productOptions);

        IList<CheckedItem> GetBettypes(int licenseeId, string sports, string languageCode);
    }
}