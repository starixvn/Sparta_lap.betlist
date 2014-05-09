// -----------------------------------------------------------------------
// <copyright file="MenuViewModel.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Models.ViewModel
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Fanex.Athena.Models.DTO.Common;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class MenuViewModel
    {
        public IList<Function> UserFunctions { get; set; }
        public IList<Function> MasterFunctions { get; set; }
        public IDictionary<int,IList<Function>> UserDetailFunctions { get; set; }
        public bool IsAllowedCustonmerCommission { get; set; }
        public bool IsInternalLogin { get; set; }
        public bool IsAllowedManagementMember { get; set; }
    }
}
