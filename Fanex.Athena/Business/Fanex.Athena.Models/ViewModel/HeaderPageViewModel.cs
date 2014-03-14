// -----------------------------------------------------------------------
// <copyright file="HeaderPageViewModel.cs" company="Nexcel">
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
    public class HeaderPageViewModel
    {
        public string Language { get; set; }

        public int OnUserId { get; set; }

        public IList<Licensee> LicenseeList { get; set; }

        public bool IsInternalLogin { get; set; }

        public bool IsDisabledRecheckIn { get; set; }

        public IDictionary<string, string> SupportedLanguageList { get; set; }

        public string UserName { get; set; }

        public int CurrentLicenseeId { get; set; }
    }
}