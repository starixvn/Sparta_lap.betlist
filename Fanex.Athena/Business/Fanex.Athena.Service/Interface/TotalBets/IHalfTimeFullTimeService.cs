// -----------------------------------------------------------------------
// <copyright file="IHalfTimeFullTimeService.cs" company="Nexcel">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Service.Interface.TotalBets
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using Models.DTO.TotalBets;
    using Models.MetaModel.TotalBets;

    public interface IHalfTimeFullTimeService
    {
        [SuppressMessage("Microsoft.Design", "CA1021:AvoidOutParameters", Justification = "Reviewed.")]
        IList<HalfTimeFullTime> GetHalfTimeFullTimeDataList(HalfTimeFullTimeMetaModel metaModel, out HalfTimeFullTime totalData);
    }
}