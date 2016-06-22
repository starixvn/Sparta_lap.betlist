// -----------------------------------------------------------------------
// <copyright file="CasinoHandHistoryDetailViewModel.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Model.Casino.ViewModel
{
    using System.Collections.Generic;
    using Fanex.Athena.Model.Casino.DTO.RNGResultDetail;
    using Fanex.Athena.Model.Casino.MetaModel;

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