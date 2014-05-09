// -----------------------------------------------------------------------
// <copyright file="SSOService.cs" company="Nexcel">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Service.Implement.Integration
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Fanex.Athena.Service.Interface.Integration;
using System.Web;
using Fanex.Athena.Service.Interface.Users;
using Fanex.Athena.Service.Implement.Users;
    using Fanex.Athena.Extension.Utility;
    using System.Configuration;
    using Fanex.Athena.Models.DTO.Common;
    using Fanex.Athena.Models.DTO.Users;
    using System.IO;
    using RSACryptoPad;
    using System.Collections.Specialized;
    using Fanex.Athena.Service.Implement.Common;
    using System.Web.Security;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class SSOService:ISSOService
    {
       private const int DisableSso = -3;
    private const int InValidParam = -1;

    private const int NotActive = -6;
    private const int NotFoundLicensee = -7;
    private const int NotFoundUser = -5;

    private const string PrivateKey = "12bet.snk";
    private const int TooManyLicensee = -8;
    private const string DefaultLanguage = "en-US";

    private readonly HttpContext _context;
    private readonly string _keyPath = string.Empty;
    private readonly string _unAccessPath = string.Empty;
    private readonly ISessionService _sessionService = new SessionService();
    public SSOService()
    {
        _context = HttpContext.Current;
        _unAccessPath = ConfigurationManager.AppSettings["SSOUnAuthorizePath"];
        _keyPath = ConfigurationManager.AppSettings["SSOKeyPath"] ?? string.Empty;
    }
    private void InternalLogin(string licname, string userName, string passWord, long timeSpan, string siteOutUrl,
        string languageCode,
                               out IResultInfo resultInfo)
    {
        var userService = new UserService();
        var requestUser = new User
        {
            Username = userName,
            Password = passWord,
            Request = new Request
            {
                Ip = _context.Request.UserHostAddress,
                Browser = _context.Request.Browser.Browser,
                Platform = _context.Request.Browser.Platform,
                Domain = siteOutUrl,
            }
        };
        IResultInfo loginResultInfo;
        User user = userService.ValidateInternalUser(requestUser, licname, timeSpan, out loginResultInfo);

        if (loginResultInfo.IsSuccessful)
        {
            _sessionService.CreateNewSession(user.Id, userService.LoginOnUser(user, _context.Session.SessionID, user.ServerId, _context.Request.UserHostAddress), user.Username,
                true, user.Licensee.Id, languageCode);
        }

        resultInfo = loginResultInfo;
    }

    public void SSO()
    {
        string parameter = HttpContext.Current.Request["key"] ?? string.Empty;
        if (!string.IsNullOrEmpty(parameter))
        {
            if (!string.IsNullOrEmpty(_keyPath))
            {
                string privateKeyPath = _context.Server.MapPath("~/" + _keyPath + PrivateKey);
                if (File.Exists(privateKeyPath))
                {
                    var decryption = new EncryptionThread();

                    string parameterTrash = parameter.Replace(" ", "+");
                    string descryptParam = decryption.DecryptString(parameterTrash, 1024, privateKeyPath);

                    NameValueCollection queryParams = HttpUtility.ParseQueryString(descryptParam);
                    string licname = queryParams["licname"] ?? string.Empty;
                    string userName = queryParams["username"] ?? string.Empty;
                    string passWord = queryParams["pwd"] ?? string.Empty;
                    string siteOutUrl = queryParams["loginurl"] ?? string.Empty;
                    string languageCode = queryParams["lang"] ?? DefaultLanguage;
                    if(!Util.IsValidLanguage(languageCode))
                    {
                        languageCode=DefaultLanguage;
                    }
                    if (IsUserAuthenticated)
                    {
                        if (userName == CurrentUserName)
                        {
                            //update language code
                            var langService = new LanguageService();
                            langService.ChangeLanguage(languageCode);
                            FormsAuthentication.SetAuthCookie(userName, false);
                            _context.Response.Redirect("~/Home/Index");
                            return;
                        }
                    }

                    long timeSpan;
                    long.TryParse(queryParams["timestamp"], out timeSpan);
                    IResultInfo resultInfo;
                    if (!string.IsNullOrEmpty(userName) && !string.IsNullOrEmpty(passWord))
                    {
                        InternalLogin(licname, userName, passWord, timeSpan, siteOutUrl,languageCode, out resultInfo);
                        if (resultInfo.IsSuccessful)
                        {
                            FormsAuthentication.SetAuthCookie(userName, false);
                            _context.Response.Redirect("~/Home/Index");
                        }
                        else
                        {
                            //check error code
                            switch (resultInfo.Code)
                            {
                                case 101:
                                    _context.Response.Redirect(Util.GetRootPath() +
                                                               string.Format(_unAccessPath, NotFoundUser, resultInfo.Message));

                                    break;
                                case 102:
                                    _context.Response.Redirect(Util.GetRootPath() +
                                                               string.Format(_unAccessPath, NotActive, resultInfo.Message));

                                    break;
                                case 103:
                                    _context.Response.Redirect(Util.GetRootPath() +
                                                               string.Format(_unAccessPath, TooManyLicensee,
                                                                             resultInfo.Message));

                                    break;
                                case 104:
                                    _context.Response.Redirect(Util.GetRootPath() +
                                                               string.Format(_unAccessPath, NotFoundLicensee, resultInfo));

                                    break;
                            }
                        }
                    }
                    else
                    {
                        _context.Response.Redirect(Util.GetRootPath() +
                                                   string.Format(_unAccessPath, InValidParam, string.Empty));
                    }
                }
                else
                {
                    _context.Response.Redirect(Util.GetRootPath() +
                                               string.Format(_unAccessPath, DisableSso, string.Empty));
                }
            }
            else
            {
                _context.Response.Redirect(Util.GetRootPath() +
                                           string.Format(_unAccessPath, DisableSso, string.Empty));
            }
        }
        else
        {
            _context.Response.Redirect(Util.GetRootPath() +
                                       string.Format(_unAccessPath, InValidParam, string.Empty));
        }
    }

    private string CurrentUserName
    {
        get
        {
            var user = System.Web.HttpContext.Current.User;
            if (user != null)
            {
                return user.Identity == null ? string.Empty : user.Identity.Name;
            }
            return string.Empty;
        }

    }
  
    private bool IsUserAuthenticated
    {
        get
        {
            var context = System.Web.HttpContext.Current;
            var isAuthenticated = context.User != null
                && context.User.Identity != null
                && context.User.Identity.IsAuthenticated
                && context.Request.IsAuthenticated;

            var isSessionValid = _sessionService.UserSession != null
                && _sessionService.UserSession.UserId > 0
                && _sessionService.LicenseeSession != null
                && _sessionService.LicenseeSession.LicenseeId > 0;

            return isAuthenticated && isSessionValid;
        }
    }
    }
}
