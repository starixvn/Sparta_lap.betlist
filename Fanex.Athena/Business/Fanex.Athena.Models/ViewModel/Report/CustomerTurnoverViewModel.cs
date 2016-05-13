// -----------------------------------------------------------------------
// <copyright file="CustomerTurnoverViewModel.cs" company="Nexcel">
// warehouse winloss form parameters
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Models.ViewModel.Report
{
    using System.Collections.Generic;
    using Fanex.Athena.Models.DTO.Report;
    using Fanex.Athena.Models.MetaModel.Report;
    using Model.Core.ViewModel;

    /// <summary>
    /// CustomerTurnoverViewModel class.
    /// </summary>
    public class CustomerTurnoverViewModel : BaseViewModel
    {
        public IList<CustTurnover> DataList { get; set; }

        public CustTurnover Total { get; set; }

        public CustomerTurnoverMetaModel Param { get; set; }

        public IDictionary<string, int> SportsList { get; set; }

        public IDictionary<string, int> BettypeList { get; set; }
    }
}