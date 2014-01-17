// -----------------------------------------------------------------------
// <copyright file="ITurnoverService.cs" company="Nexcel">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Service.Interface.Report
{
    using System.Collections.Generic;
    using Fanex.Athena.Models.DTO.Report;
    using Fanex.Athena.Models.MetaModel.Report;

    public interface ITurnoverService
    {
        IList<TurnoverDTO> GetDataList(TurnoverMetaModel metaModel);

        TurnoverDTO SumTotalData(IList<TurnoverDTO> dataList);
    }
}