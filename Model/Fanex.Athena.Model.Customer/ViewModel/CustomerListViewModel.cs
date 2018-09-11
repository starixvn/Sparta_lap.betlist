// -----------------------------------------------------------------------
// <copyright file="CustomerListViewModel.cs" company="">
// warehouse winloss form parameters
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Model.Customer.ViewModel
{
    using System.Collections.Generic;
    using Core.DTO.Common;
    using Fanex.Athena.Model.Customer.DTO;
    using Fanex.Athena.Model.Customer.MetaModel;
    using Model.Core.ViewModel;

    public class CustomerListViewModel : BaseViewModel
    {
        public CustomerListMetaModel Param { get; set; }

        public IEnumerable<CustomerDTO> DataList { get; set; }

        public int TotalRows { get; set; }

        public GroupCommMetaModel CurrentGroupComm { get; set; }

        public IList<GroupCommMetaModel> GroupCommDataList { get; set; }

        public string BATimeCheck { get; set; }

        public bool CanUpdateBA { get; set; }

        public string CanUpdateBAMessage { get; set; }

        public bool IsAllowedUserEditComm { get; set; }

        public bool IsAllowedUserChangeBA { get; set; }

        public bool IsFakeLicensee { get; set; }

        public bool IsSubsidiary { get; set; }

        public bool HasSubsidiary { get; set; }

        public IEnumerable<CheckedItem> SubsidiaryList { get; set; }
    }
}