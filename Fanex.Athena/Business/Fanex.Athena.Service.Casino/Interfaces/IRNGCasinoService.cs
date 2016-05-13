// -----------------------------------------------------------------------
// <copyright file="IRNGCasinoService.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Service.Casino.Interfaces
{
    using System.Collections.Generic;
    using Fanex.Athena.Model.Casino.DTO;
    using Fanex.Athena.Model.Casino.MetaModel;
    using Fanex.Athena.Model.Casino.ViewModel;

    public interface IRNGCasinoService
    {
        List<RNGCasinoResult> GetRNGCasinoResult(RNGCasinoResultMetaModel metaModel);

        CasinoHandHistoryDetailViewModel GetRNGCasinoResultDetail(RNGCasinoResultDetailMetaModel metaModel);

        bool UpdateMemberCommission(RNGCasinoUpdateMemberComMetaModel metaModel);
    }
}