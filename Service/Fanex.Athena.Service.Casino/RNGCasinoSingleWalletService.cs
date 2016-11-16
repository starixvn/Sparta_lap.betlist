// -----------------------------------------------------------------------
// <copyright file="RGNCasinoSingleWallet.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Service.Casino
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;
    using System.Net;
    using System.Web;
    using Fanex.Athena.Extension.Helper;
    using Fanex.Athena.Extension.Utility;
    using Fanex.Athena.Service.Casino.Interfaces;
    using Fanex.Athena.Service.Core.Helpers;
    using Newtonsoft.Json;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class RngCasinoSingleWalletService : IRNGCasinoSingleWalletService
    {
        private static readonly IDictionary<string, string> MapLanguages = new Dictionary<string, string>
            {
                { "en-US", "en" },
                { "zh-TW", "ch" },
                { "zh-CN", "cs" },
                { "zh-Hans", "zhcn" }
            };

        private readonly IAppSettings _appSettings;

        public RngCasinoSingleWalletService()
        {
            _appSettings = new AppSettings();
        }

        public dynamic GetResultInfo(string refNo)
        {
            var authenticatedToken = GetRngCasinoServerToken();

            var authenticatedTokenInJson = JsonConvert.DeserializeObject<dynamic>(authenticatedToken);

            var result = authenticatedTokenInJson.Result;

            if (result.ErrorCode == 0)
            {
                return new
                {
                    Success = true,
                    Result = GetRngCasinoPopupUrl(result.Token.ToString(), refNo)
                };
            }

            return new
            {
                Success = false,
                Result = new
                {
                    ErrorCode = result.ErrorCode.ToString(),
                    ErrorDescription = result.ErrorDescription.ToString()
                }
            };
        }

        private static string EncryptQueryString(string parameters, string encryptedKey)
        {
            return IBCCrypt.IBCCrypt.Encrypt(parameters, encryptedKey);
        }

        private static string GetLanguageCode()
        {
            var languageCode = CultureInfo.CurrentUICulture.IetfLanguageTag;
            string result;
            if (!MapLanguages.TryGetValue(languageCode, out result))
            {
                return "en";
            }

            return result;
        }

        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Justification = "Reviewed.")]
        private string GetRngCasinoServerToken()
        {
            var uri = new Uri(_appSettings.GetValue("RNGCasinoGetServerTokenAPIUrl"));
            var paramString = CreateServerTokenParameters();
            var serverResponse = string.Empty;

            try
            {
                using (WebClient webClient = new WebClient())
                {
                    webClient.Headers[HttpRequestHeader.ContentType] = "application/json";

                    serverResponse = webClient.UploadString(uri, paramString);
                }
            }
            catch (Exception exception)
            {
                LogHelper.WriteError(exception);
            }

            return serverResponse;
        }

        private string CreateServerTokenParameters()
        {
            var timeStamp = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss.fff");
            var login = _appSettings.GetValue("RNGCasinoLogin");
            var password = _appSettings.GetValue("RNGCasinoPassword");
            var siteId = int.Parse(_appSettings.GetValue("RNGCasinoSiteId"));
            var seq = _appSettings.GetValue("RNGCasinoSeq");
            var vendorId = int.Parse(_appSettings.GetValue("RNGCasinoVendorId"));

            var auth = new { Login = login, Password = password, SiteId = siteId };
            var call = new { Seq = seq, VendorId = vendorId };

            return new
            {
                TimeStamp = timeStamp,
                Auth = auth,
                Call = call
            }.ToJson();
        }

        private string GetRngCasinoPopupUrl(string token, string refNo)
        {
            var queryString = GetQueryString(token, refNo);
            var encryptedKey = _appSettings.GetValue("RNGCasinoEncryptedKey");
            var encryptParam = EncryptQueryString(queryString, encryptedKey);
            var paramEncode = HttpUtility.UrlEncode(encryptParam);
            var popupUrl = _appSettings.GetValue("RNGCasinoResultPopupUrl");

            return popupUrl + "?o=" + paramEncode;
        }

        private string GetQueryString(string token, string refNo)
        {
            var languageCode = GetLanguageCode();
            var template = _appSettings.GetValue("RNGCasinoTemplate");
            var role = _appSettings.GetValue("RNGCasinoRole");

            var queryString = $"Token={token}&RefID={refNo}&Lang={languageCode}&Template={template}&Role={role}";

            return queryString;
        }
    }
}