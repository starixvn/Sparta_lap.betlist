// -----------------------------------------------------------------------
// <copyright file="CasinoHandHistoryDetailViewModel.cs" company="Nexcel">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Models.ViewModel.Casino.RNGResultDetail
{
    using System.Collections.Generic;
    using Fanex.Athena.Models.DTO.Casino.RNGResultDetail;
    using Fanex.Athena.Models.MetaModel.Casino;

    /// <summary>
    /// CasinoHandHistoryDetailViewModel class.
    /// </summary>
    public class CasinoHandHistoryDetailViewModel
    {
        public CasinoHandHistoryDetailViewModel()
        {
            CasinoDetailList = new List<CasinoWinLossDetail>();
            CasinoDetailHeaderList = new List<CasinoWinLossDetailHeader>();
        }

        public RNGCasinoResultDetailMetaModel Param { get; set; }

        public IList<CasinoWinLossDetail> CasinoDetailList { get; set; }

        public IList<CasinoWinLossDetailHeader> CasinoDetailHeaderList { get; set; }
    }
}