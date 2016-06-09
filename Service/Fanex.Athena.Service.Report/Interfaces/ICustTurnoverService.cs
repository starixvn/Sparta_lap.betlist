// -----------------------------------------------------------------------
// <copyright file="ICustTurnoverService.cs" company="Nexcel">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Service.Report.Interfaces
{
    using System.Collections.Generic;
    using Fanex.Athena.Model.Report.DTO;
    using Fanex.Athena.Model.Report.MetaModel;

    public interface ICustTurnoverService
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1021:AvoidOutParameters",
            Justification = "Reviewed. Suppression is OK here.")]
        IList<CustTurnover> GetDataList(CustomerTurnoverMetaModel metaModel, string languageCode, out CustTurnover total, out int totalRows);
    }
}