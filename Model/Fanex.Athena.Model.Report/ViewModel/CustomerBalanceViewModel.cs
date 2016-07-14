// -----------------------------------------------------------------------
// <copyright file="CustomerBalanceViewModel.cs" company="">
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
    /// CustomerBalanceViewModel class.
    /// </summary>
    public class CustomerBalanceViewModel : BaseViewModel
    {
        public bool IsValidTimeRequest { get; set; }

        public IList<CustBalance> DataList { get; set; }

        public CustBalance TotalData { get; set; }

        public CustomerBalanceMetaModel Param { get; set; }
    }
}