// -----------------------------------------------------------------------
// <copyright file="MenuViewModel.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Model.Core.ViewModel
{
    using System.Collections.Generic;
    using Fanex.Athena.Model.Core.DTO.Common;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class MenuViewModel
    {
        public IEnumerable<Function> UserFunctions { get; set; }

        public IEnumerable<Function> MasterFunctions { get; set; }

        public IDictionary<int, IEnumerable<Function>> UserDetailFunctions { get; set; }

        public bool IsAllowedCustonmerCommission { get; set; }

        public bool IsInternalLogin { get; set; }

        public bool IsAllowedManagementMember { get; set; }

        public bool IsFakeLicensee { get; set; }
    }
}