// -----------------------------------------------------------------------
// <copyright file="IBetTypeService.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Service.Core.Common.Interfaces
{
    using System.Collections.Generic;
    using Fanex.Athena.Model.Core.DTO.Common;
    using Fanex.Athena.Models.MetaModel.Product;

    /// <summary>
    ///     TODO: Update summary.
    /// </summary>
    public interface IBetTypeService
    {
        IDictionary<string, int> GetAllBettypes(IList<ProductOption> productOptions);

        IList<CheckedItem> GetBettypes(int licenseeId, IList<SportProductItem> sports, string languageCode);
    }
}