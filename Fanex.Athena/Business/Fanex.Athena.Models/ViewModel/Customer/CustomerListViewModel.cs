// -----------------------------------------------------------------------
// <copyright file="CustomerListViewModel.cs" company="Nexcel">
// warehouse winloss form parameters
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Models.ViewModel.Customer
{
    using System.Collections.Generic;
    using Fanex.Athena.Models.DTO.Customer;
    using Fanex.Athena.Models.MetaModel.Customer;

    /// <summary>
    /// CustomerListViewModel class.
    /// </summary>
    public class CustomerListViewModel : BaseViewModel
    {
        public CustomerListMetaModel Param { get; set; }

        public IList<CustomerDTO> DataList { get; set; }

        public int TotalRows { get; set; }

        public GroupCommMetaModel CurrentGroupComm { get; set; }

        public IList<GroupCommMetaModel> GroupCommDataList { get; set; }

        public string BATimeCheck { get; set; }

        public bool CanUpdateBA { get; set; }

        public string CanUpdateBAMessage { get; set; }

        public bool IsAllowedUserEditComm { get; set; }

        public bool IsAllowedUserChangeBA { get; set; }
    }
}