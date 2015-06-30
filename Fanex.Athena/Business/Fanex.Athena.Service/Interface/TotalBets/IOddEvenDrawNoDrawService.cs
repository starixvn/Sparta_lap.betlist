// -----------------------------------------------------------------------
// <copyright file="IOddEvenDrawNoDrawService.cs" company="Nexcel">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Service.Interface.TotalBets
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using Models.DTO.TotalBets;
    using Models.MetaModel.TotalBets;

    public interface IOddEvenDrawNoDrawService
    {
        [SuppressMessage("Microsoft.Design", "CA1021:AvoidOutParameters", Justification = "Reviewd.")]
        IList<OddEvenDrawNoDraw> GetOddEvenDrawNoDrawDataList(OddEvenDrawNoDrawMetaModel metaModel, out OddEvenDrawNoDraw totalData);
    }
}