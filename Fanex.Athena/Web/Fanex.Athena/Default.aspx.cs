// --------------------------------------------------------------------------
// <copyright file="Default.aspx.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Dave.Bui" date="2012.4.11">Created</change>
// </history>

namespace Fanex.Athena
{
    using System;
    using System.Globalization;
    using System.Threading;
    using System.Web.Security;
    using Fanex.Athena.Service.Implement.Users;
    using Fanex.Athena.Service.Interface.Users;

    /// <summary>
    /// Test class.
    /// </summary>
    public partial class Default : System.Web.UI.Page
    {
        private ISessionService _sessionService;

        public void SetLanguage(string languageCode)
        {
            Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo(languageCode);
            Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo(languageCode);
        }

        public void SetDefault(int licenseeid = 1, string lang = "en-US")
        {
            _sessionService = new SessionService();

            _sessionService.CreateNewSession(5, 5, "pgmjacob", false, licenseeid, lang);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            var licenseeId = Convert.ToInt32(Request["licenseeid"]);
            var lang = Convert.ToString(Request["lang"]);
            SetLanguage(string.IsNullOrEmpty(lang) ? "en-US" : lang);
            SetDefault(licenseeId != 0 ? licenseeId : 1, string.IsNullOrEmpty(lang) ? "en-US" : lang);
            FormsAuthentication.SetAuthCookie("pgmjacob", false);
            Response.Redirect("~/Report/Balance");
        }
    }
}