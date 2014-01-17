// -----------------------------------------------------------------------
// <copyright file="IWinLossByBetTypeServive.cs" company="Nexcel">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Service.Interface.Report
{
    using System.Collections.Generic;
    using Fanex.Athena.Models.DTO.Report;
    using Fanex.Athena.Models.MetaModel.Report;

    /// <summary>
    ///     TODO: Update summary.
    /// </summary>
    public interface IWinLossByBetTypeServive
    {
        IList<WinLossByBetType> GetDataList(WinLossByBetTypeMetaModel metaModel);
    }
}