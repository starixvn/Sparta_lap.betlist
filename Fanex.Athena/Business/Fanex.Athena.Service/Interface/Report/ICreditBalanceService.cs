// -----------------------------------------------------------------------
// <copyright file="ICreditBalanceService.cs" company="Nexcel">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Service.Interface.Report
{
    using Fanex.Athena.Models.DTO.Report;
    using Fanex.Athena.Models.MetaModel.Report;

    /// <summary>
    ///     Credit Balance.
    /// </summary>
    public interface ICreditBalanceService
    {
        Balance GetReportData(BalanceMetaModel metaModel);
    }
}