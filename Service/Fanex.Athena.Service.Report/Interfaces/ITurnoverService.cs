// -----------------------------------------------------------------------
// <copyright file="ITurnoverService.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Service.Report.Interfaces
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Fanex.Athena.Model.Report.DTO;
    using Fanex.Athena.Model.Report.MetaModel;

    public interface ITurnoverService
    {
        Task<IList<TurnoverDTO>> GetDataList(TurnoverMetaModel metaModel);

        TurnoverDTO SumTotalData(IList<TurnoverDTO> dataList);
    }
}