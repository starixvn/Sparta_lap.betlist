// -----------------------------------------------------------------------
// <copyright file="ISettledMatchCountService.cs" company="Nexcel">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Service.Report.Interfaces
{
    using System.Collections.Generic;
    using Fanex.Athena.Model.Core.DTO.Common;
    using Fanex.Athena.Model.Report.ViewModel;

    /// <summary>
    ///     TODO: Update summary.
    /// </summary>
    public interface ISettledMatchCountService
    {
        IList<SettledMatchCountViewModel> GetDataList(bool isShowAll, int licenseeId, ProductAccess producAccess);
    }
}