// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ISessionService.cs" company="Nexcel">
// The  session service.
// </copyright>
// <summary>
//   The  session service.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Fanex.Athena.Service.Core.Users.Interfaces
{
    using Fanex.Athena.Model.Core.DTO.Users;

    /// <summary>
    ///     The i session service.
    /// </summary>
    public interface ISessionService
    {
        #region Public Properties

        string CaptchaCode { get; set; }

        LicenseeSession LicenseeSession { get; }

        UserSession UserSession { get; }

        void CreateNewSession(
            int userId,
            int onuserId,
            string userName,
            bool isInternal,
            int licenseeId = 0,
            string language = "en-US");

        /// <summary>
        ///     Only use for user that have multiple licensee.
        /// </summary>
        /// <param name="licenseeId">The licensee id.</param>
        void UpdateLiceseeSession(int licenseeId);

        bool UpdateLanguage(string languageCode);

        #endregion Public Properties
    }
}