// -----------------------------------------------------------------------
// <copyright file="CustomerTurnoverViewModel.cs" company="">
// warehouse winloss form parameters
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Model.Report.ViewModel
{
    using System.Collections.Generic;
    using Fanex.Athena.Model.Report.DTO;
    using Fanex.Athena.Model.Report.MetaModel;
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