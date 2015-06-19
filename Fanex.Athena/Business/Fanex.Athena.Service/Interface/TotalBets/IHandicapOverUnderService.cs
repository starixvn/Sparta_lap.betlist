// -----------------------------------------------------------------------
// <copyright file="IHandicapOverUnderService.cs" company="Nexcel">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Service.Interface.TotalBets
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using Fanex.Athena.Models.DTO.TotalBets;
    using Fanex.Athena.Models.MetaModel.TotalBets;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface IHandicapOverUnderService
    {
        [SuppressMessage("Microsoft.Design", "CA1021:AvoidOutParameters", Justification = "Reviewed.")]
        IList<HandicapOverUnder> GetHandicapOverUnderDataList(HandicapOverUnderMetaModel metaModel, out HandicapOverUnder totalData);
    }
}