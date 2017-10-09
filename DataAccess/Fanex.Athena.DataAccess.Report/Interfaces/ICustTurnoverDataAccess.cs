// -----------------------------------------------------------------------
// <copyright file="ICustTurnoverDataAccess.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.DataAccess.Report.Interfaces
{
    using Fanex.Athena.Model.Report.DTO;
    using Fanex.Athena.Model.Report.MetaModel;
    using Model.Core.ResultModel;

    public interface ICustTurnoverDataAccess
    {
        ReportResultModel<CustTurnover> GetDataList(CustomerTurnoverMetaModel metaModel, string languageCode);
    }
}