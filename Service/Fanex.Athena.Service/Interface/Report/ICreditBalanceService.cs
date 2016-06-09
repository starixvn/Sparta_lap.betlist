// -----------------------------------------------------------------------
// <copyright file="ICreditBalanceService.cs" company="Nexcel">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Service.Interface.Report
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