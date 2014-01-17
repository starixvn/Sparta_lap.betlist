// -----------------------------------------------------------------------
// <copyright file="IOutrightService.cs" company="Nexcel">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Service.Interface.TotalBets
{
    using System.Collections.Generic;
    using Models.DTO.TotalBets;
    using Models.MetaModel.TotalBets;

    public interface IOutrightService
    {
        IList<Outright> GetOutrightDataList(OutrightMetaModel metaModel);
    }
}