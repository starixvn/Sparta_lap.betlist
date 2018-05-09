namespace Fanex.Athena.Session
{
    using System.Web;
    using Interfaces;
    using Model;

    public class SessionStorage : ISessionStorage
    {
        private const string LicenseeSessionKey = "LicenseeSessionKey";

        private const string UserSessionKey = "UserSessionKey";
        private const string CaptchaCodeKey = "CaptchaCodeKey";

        public SessionStorage()
        {
        }

        public LicenseeSession LicenseeSession
        {
            get
            {
                var temp = HttpContext.Current.Session[LicenseeSessionKey];
                return temp != null ? (LicenseeSession)temp : null;
            }
        }

        public UserSession UserSession
        {
            get
            {
                var temp = HttpContext.Current.Session[UserSessionKey];
                return temp != null ? (UserSession)temp : null;
            }
        }

        public string CaptchaCode
        {
            get { return HttpContext.Current.Session[CaptchaCodeKey] as string; }
            set { HttpContext.Current.Session[CaptchaCodeKey] = value; }
        }

        public void AssignSessions(UserSession userSession, LicenseeSession licenseeSession)
        {
            HttpContext.Current.Session[UserSessionKey] = userSession;
            HttpContext.Current.Session[LicenseeSessionKey] = licenseeSession;
        }
    }
}