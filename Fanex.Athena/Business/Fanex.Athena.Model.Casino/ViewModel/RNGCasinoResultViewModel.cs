// -----------------------------------------------------------------------
// <copyright file="RNGCasinoResultViewModel.cs" company="Nexcel">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Model.Casino.ViewModel
{
    using System.Collections.Generic;
    using Fanex.Athena.Model.Casino.DTO;
    using Fanex.Athena.Model.Casino.MetaModel;

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