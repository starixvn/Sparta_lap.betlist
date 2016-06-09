// -----------------------------------------------------------------------
// <copyright file="IHalfTimeFullTimeService.cs" company="Nexcel">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Service.TotalBet.Interfaces
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using Model.TotalBet.DTO;
    using Model.TotalBet.MetaModel;

    public interface IHalfTimeFullTimeService
    {
        [SuppressMessage("Microsoft.Design", "CA1021:AvoidOutParameters", Justification = "Reviewed.")]
        IList<HalfTimeFullTime> GetHalfTimeFullTimeDataList(HalfTimeFullTimeMetaModel metaModel, out HalfTimeFullTime totalData);
    }
}