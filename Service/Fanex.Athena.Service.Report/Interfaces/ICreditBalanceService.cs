// -----------------------------------------------------------------------
// <copyright file="ICreditBalanceService.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Service.Report.Interfaces
{
    using Fanex.Athena.Model.Report.DTO;
    using Fanex.Athena.Model.Report.MetaModel;

    /// <summary>
    ///     Credit Balance.
    /// </summary>
    public interface ICreditBalanceService
    {
        Balance GetReportData(BalanceMetaModel metaModel);
    }
}