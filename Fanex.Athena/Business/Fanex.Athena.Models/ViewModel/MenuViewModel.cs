// -----------------------------------------------------------------------
// <copyright file="MenuViewModel.cs" company="Nexcel">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Models.ViewModel
{
    using System.Collections.Generic;
    using Fanex.Athena.Models.DTO.Common;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class MenuViewModel
    {
        public IList<Function> UserFunctions { get; set; }

        public IList<Function> MasterFunctions { get; set; }

        public IDictionary<int, IList<Function>> UserDetailFunctions { get; set; }

        public bool IsAllowedCustonmerCommission { get; set; }

        public bool IsInternalLogin { get; set; }

        public bool IsAllowedManagementMember { get; set; }
    }
}