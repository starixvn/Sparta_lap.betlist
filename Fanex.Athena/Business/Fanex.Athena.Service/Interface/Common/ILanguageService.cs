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
        string GetCurrentLanguage();

        /// <summary>
        /// Support to change language.
        /// </summary>
        /// <param name="languageCode">The language Code.</param>
        /// <returns>True if processing is successful.</returns>
        bool ChangeLanguage(string languageCode);

        void SetPageCulture(string languageCode);

        IList<Function> MapWithLanguage(IList<Function> list);
    }
}