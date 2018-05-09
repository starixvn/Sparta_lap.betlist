// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ISessionService.cs" company="">
// The  session service.
// </copyright>
// <summary>
//   The  session service.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Fanex.Athena.Service.Core.Users.Interfaces
{
    using System.Collections.Generic;
    using Fanex.Athena.Model.Core.DTO.Users;
    using Session.Enum;
    using Session.Model;

    public interface ISessionService
    {
        string CaptchaCode { get; set; }

        LicenseeSession LicenseeSession { get; }

        UserSession UserSession { get; }

        void AssignSessions(UserSession userSession, LicenseeSession licenseeSession);

        void CreateNewSession(
            int userId,
            int onuserId,
            string userName,
            bool isInternal,
            int licenseeId = 0,
            string language = "en-US",
             ClientViewType viewType = ClientViewType.None);

        void UpdateLiceseeSession(int licenseeId);

        bool UpdateLanguage(string languageCode);

        string GenerateAgentListXml(int currencyId = 0, bool isExcludeTestData = false);

        string GenerateAgentListXml(IEnumerable<int> currencyIdList, bool isExcludeTestData = false);

        bool IsAllowedEditCommission(UserSession user);

        bool IsAllowedSpecialBA(UserSession user);

        string GenerateAgentListXml(IDictionary<int, IEnumerable<int>> currencyAgentList);

        void AssignCurrenciesToUserSession(UserSession userSession, int licenseeId, IEnumerable<CurrencyUser> currencyUserList = null);
    }
}