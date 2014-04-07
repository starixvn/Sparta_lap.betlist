// -----------------------------------------------------------------------
// <copyright file="ISportTypeService.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Service.Interface.Common
{
    #region

    using System;
    using System.Collections.Generic;
    using Fanex.Athena.Models.DTO.Common;

    #endregion

    /// <summary>
    ///     TODO: Update summary.
    /// </summary>
    public interface ISportTypeService
    {
        IDictionary<string, int> GetAllSports();
                
        IList<Sport> GetAllWHSports();

        IList<SportProductItem> GetSportsAndProducts(int licenseeId, string languageCode, int? productId = null);
    }
}