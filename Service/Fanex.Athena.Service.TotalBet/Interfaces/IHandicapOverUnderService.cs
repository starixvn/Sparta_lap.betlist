// -----------------------------------------------------------------------
// <copyright file="IHandicapOverUnderService.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Service.TotalBet.Interfaces
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using Fanex.Athena.Model.TotalBet.DTO;
    using Fanex.Athena.Model.TotalBet.MetaModel;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface IHandicapOverUnderService
    {
        [SuppressMessage("Microsoft.Design", "CA1021:AvoidOutParameters", Justification = "Reviewed.")]
        IList<HandicapOverUnder> GetHandicapOverUnderDataList(HandicapOverUnderMetaModel metaModel, out HandicapOverUnder totalData);
    }
}