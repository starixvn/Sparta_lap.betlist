// -----------------------------------------------------------------------
// <copyright file="ILanguageService.cs" company="Nexcel">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Service.Interface.Common
{
    using System.Collections.Generic;
    using Fanex.Athena.Models.DTO.Common;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface ILanguageService
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate",
            Justification = "Reviewed. Suppression is OK here.")]
        string GetCurrentLanguage();

        /// <summary>
        /// Support to change language.
        /// </summary>
        /// <param name="languageCode">The language Code.</param>
        /// <returns>True if processing is successful.</returns>
        bool ChangeLanguage(string languageCode);

        void SetPageCulture(string languageCode);
    }
}