// -----------------------------------------------------------------------
// <copyright file="HeaderPageViewModel.cs" company="">
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
    public class HeaderPageViewModel
    {
        public string Language { get; set; }

        public int OnUserId { get; set; }

        public IList<Licensee> LicenseeList { get; set; }

        public bool IsInternalLogin { get; set; }

        public bool HasRecheckIn { get; set; }

        public IDictionary<string, string> SupportedLanguageList { get; set; }

        public string UserName { get; set; }

        public int CurrentLicenseeId { get; set; }

        public bool IsAllowedManagementMember { get; set; }
    }
}