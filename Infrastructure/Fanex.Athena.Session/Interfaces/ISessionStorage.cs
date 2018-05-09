using Fanex.Athena.Session.Model;

namespace Fanex.Athena.Session.Interfaces
{
    public interface ISessionStorage
    {
        string CaptchaCode { get; set; }

        LicenseeSession LicenseeSession { get; }

        UserSession UserSession { get; }

        void AssignSessions(UserSession userSession, LicenseeSession licenseeSession);
    }
}