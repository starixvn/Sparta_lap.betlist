// -----------------------------------------------------------------------
// <copyright file="ITurnoverService.cs" company="Nexcel">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Service.Interface.Report
{
    using System.Collections.Generic;
    using Fanex.Athena.Model.Report.DTO;
    using Fanex.Athena.Model.Report.MetaModel;

    public interface ITurnoverService
    {
        IList<TurnoverDTO> GetDataList(TurnoverMetaModel metaModel);

        TurnoverDTO SumTotalData(IList<TurnoverDTO> dataList);
    }
}