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

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", 
            Justification = "Reviewed. Suppression is OK here.")]
        public IDictionary<int, IList<Function>> UserDetailFunctions { get; set; }

        public bool IsAllowedCustonmerCommission { get; set; }

        public bool IsInternalLogin { get; set; }

        public bool IsAllowedManagementMember { get; set; }
    }
}