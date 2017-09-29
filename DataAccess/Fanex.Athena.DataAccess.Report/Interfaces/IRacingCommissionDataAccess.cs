// -----------------------------------------------------------------------
// <copyright file="IRacingCommissionDataAccess.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.DataAccess.Report.Interfaces
{
    using Fanex.Athena.Model.Report.DTO;
    using Fanex.Athena.Model.Report.MetaModel;
    using Model.Core.ResultModel;

    public interface IRacingCommissionDataAccess
    {
        SortableReportResultModel<RacingCommission> GetData(RacingCommissionMetaModel metaModel);
    }
}