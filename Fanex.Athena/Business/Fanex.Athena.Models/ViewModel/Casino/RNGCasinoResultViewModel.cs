// -----------------------------------------------------------------------
// <copyright file="RNGCasinoResultViewModel.cs" company="Nexcel">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Models.ViewModel.Casino
{
    using System.Collections.Generic;
    using Fanex.Athena.Models.DTO.Casino;
    using Fanex.Athena.Models.MetaModel.Casino;

    /// <summary>
    /// RNGCasinoResultViewModel class.
    /// </summary>
    public class RNGCasinoResultViewModel
    {
        public RNGCasinoResultViewModel()
        {
            Data = new List<RNGCasinoResult>();
        }

        public bool IsAllowedCustomerCommission { get; set; }

        public IList<RNGCasinoResult> Data { get; set; }

        public RNGCasinoResultMetaModel Param { get; set; }

        public string ColumnName { get; set; }
    }
}