// -----------------------------------------------------------------------
// <copyright file="RGNCasinoSingleWallet.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Service.Implement.Casino
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Net;
    using System.Web;
    using Fanex.Athena.Extension.Helper;
    using Fanex.Athena.Extension.Utility;
    using Fanex.Athena.Service.Helpers;
    using Newtonsoft.Json;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class RGNCasinoSingleWallet : IRNGCasinoSingleWallet
    {
        public dynamic GetReultInfo(string refNo)
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

        private static IDictionary<string, string> _mapLanguages = new Dictionary<string, string>()
            {
                { "en-US", "en" },
                { "zh-TW", "ch" },
                { "zh-CN", "cs" },
                { "zh-Hans", "zhcn" }
            };

        private static string EncryptQueryString(string parameters, string encryptedKey)
        {
            return IBCCrypt.IBCCrypt.Encrypt(parameters, encryptedKey);
        }

        private static string GetRngCasinoServerToken()
        {
            var uri = new Uri(AppSettings.GetValue("RNGCasinoGetServerTokenAPIUrl"));
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

        private static string CreateServerTokenParameters()
        {
            var timeStamp = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss.fff");
            var login = AppSettings.GetValue("RNGCasinoLogin");
            var password = AppSettings.GetValue("RNGCasinoPassword");
            var siteId = int.Parse(AppSettings.GetValue("RNGCasinoSiteId"));
            var seq = AppSettings.GetValue("RNGCasinoSeq");
            var vendorId = int.Parse(AppSettings.GetValue("RNGCasinoVendorId"));

            var auth = new { Login = login, Password = password, SiteId = siteId };
            var call = new { Seq = seq, VendorId = vendorId };

            return new
            {
                TimeStamp = timeStamp,
                Auth = auth,
                Call = call
            }.ToJson();
        }

        private static string GetRngCasinoPopupUrl(string token, string refNo)
        {
            var queryString = GetQueryString(token, refNo);
            var encryptedKey = AppSettings.GetValue("RNGCasinoEncryptedKey");
            var encryptParam = EncryptQueryString(queryString, encryptedKey);
            var paramEncode = HttpUtility.UrlEncode(encryptParam);
            var popupUrl = AppSettings.GetValue("RNGCasinoResultPopupUrl");

            return popupUrl + "?o=" + paramEncode;
        }

        private static string GetQueryString(string token, string refNo)
        {
            var languageCode = GetLanguageCode();
            var template = AppSettings.GetValue("RNGCasinoTemplate");
            var role = AppSettings.GetValue("RNGCasinoRole");

            var queryString = string.Format(
                "Token={0}&RefID={1}&Lang={2}&Template={3}&Role={4}", token, refNo, languageCode, template, role);

            return queryString;
        }

        private static string GetLanguageCode()
        {
            var languageCode = CultureInfo.CurrentUICulture.IetfLanguageTag;
            string result;
            if (!_mapLanguages.TryGetValue(languageCode, out result))
            {
                return "en";
            }

            return result;
        }
    }
}