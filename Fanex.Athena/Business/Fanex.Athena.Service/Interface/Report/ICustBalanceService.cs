// -----------------------------------------------------------------------
// <copyright file="ICustBalanceService.cs" company="Nexcel">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Service.Interface.Report
{
    using System.Collections.Generic;
    using Fanex.Athena.Models.DTO.Report;
    using Fanex.Athena.Models.MetaModel.Report;

    public interface ICustBalanceService
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1045:DoNotPassTypesByReference", 
            Justification = "Reviewed. Suppression is OK here.")]
        IList<CustBalance> GetDataList(CustomerBalanceMetaModel metaModel, ref CustBalance totalData, ref int totalRow);
    }
}