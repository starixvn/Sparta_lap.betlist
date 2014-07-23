// -----------------------------------------------------------------------
// <copyright file="IRNGCasinoService.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Service.Interface.Casino
{
    using System.Collections.Generic;
    using Fanex.Athena.Models.DTO.Casino;
    using Fanex.Athena.Models.MetaModel.Casino;
    using Fanex.Athena.Models.ViewModel.Casino.RNGResultDetail;

    public interface IRNGCasinoService
    {
        List<RNGCasinoResult> GetRNGCasinoResult(RNGCasinoResultMetalModel metaModel);

        CasinoHandHistoryDetailViewModel GetRNGCasinoResultDetail(RNGCasinoResultDetailMetaModel metaModel);

        bool UpdateMemberCommission(RNGCasinoUpdateMemberComMetaModel metaModel);
    }
}