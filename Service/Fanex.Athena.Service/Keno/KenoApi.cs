// -----------------------------------------------------------------------
// <copyright file="KenoApi.cs" company="Nexcel">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Service.Report.Keno
{
    using System;
    using System.Net;
    using System.Text;
    using System.Web;
    using System.Web.Script.Serialization;
    using Fanex.Athena.Model.Report.DTO.Keno;
    using Fanex.Athena.Service.Core.Exception;
    using Fanex.Athena.Service.Report.Keno.Interfaces;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class KenoApi : IKenoApi
    {
        private const string SERVICENAME = "KenoApi";
        private const string GAMENAME = "Keno";

        /// <summary>
        /// Authorize for Keno.
        /// </summary>
        /// <param name="setting">Setting meta model.</param>
        /// <param name="userId">User Id value.</param>
        /// <returns>KenoAuthorization value.</returns>
        public Model.Report.DTO.Keno.KenoAuthorization Authorize(Model.Report.MetaModel.Keno.KenoResultMetalModel setting, string userId)
        {
            try
            {
                var authorizedUrl = string.Format(setting.AuthorizedUrl, setting.Vendor, setting.VenderKey, userId);
                var data = DownLoadData(authorizedUrl);
                if (string.IsNullOrEmpty(data))
                {
                    throw new ServiceException("Couldn't get authorized data from:" + authorizedUrl, SERVICENAME);
                }

                var serialize = new JavaScriptSerializer();
                return serialize.Deserialize<KenoAuthorization>(data);
            }
            catch (WebException exception)
            {
                throw new ServiceException(exception.Message, SERVICENAME);
            }
            catch (ArgumentNullException exception)
            {
                throw new ServiceException(exception.Message, SERVICENAME);
            }
            catch (ArgumentException exception)
            {
                throw new ServiceException(exception.Message, SERVICENAME);
            }
            catch (InvalidOperationException exception)
            {
                throw new ServiceException(exception.Message, SERVICENAME);
            }
            catch (FormatException exception)
            {
                throw new ServiceException(exception.Message, SERVICENAME);
            }
        }

        /// <summary>
        /// Extend token for Keno.
        /// </summary>
        /// <param name="setting">Setting value.</param>
        /// <param name="userId">User Id value.</param>
        /// <returns>KenoExtendedToken value.</returns>
        public Model.Report.DTO.Keno.KenoExtendedToken ExtendToken(Model.Report.MetaModel.Keno.KenoResultMetalModel setting, string userId)
        {
            try
            {
                var extendedUrl = string.Format(setting.ExtendTokenUrl, setting.Vendor, setting.VenderKey, userId);
                var data = DownLoadData(extendedUrl);
                if (string.IsNullOrEmpty(data))
                {
                    throw new ServiceException("Couldn't get authorized data from:" + extendedUrl, SERVICENAME);
                }

                var serialize = new JavaScriptSerializer();
                return serialize.Deserialize<KenoExtendedToken>(data);
            }
            catch (WebException exception)
            {
                throw new ServiceException(exception.Message, SERVICENAME);
            }
            catch (ArgumentNullException exception)
            {
                throw new ServiceException(exception.Message, SERVICENAME);
            }
            catch (ArgumentException exception)
            {
                throw new ServiceException(exception.Message, SERVICENAME);
            }
            catch (InvalidOperationException exception)
            {
                throw new ServiceException(exception.Message, SERVICENAME);
            }
            catch (FormatException exception)
            {
                throw new ServiceException(exception.Message, SERVICENAME);
            }
        }

        /// <summary>
        /// Create Keno Result Url.
        /// </summary>
        /// <param name="setting">Setting value.</param>
        /// <param name="userId">User Id value.</param>
        /// <param name="resultDate">Result date value.</param>
        /// <returns>String Url value.</returns>
        public string CreateKenoResultUrl(Model.Report.MetaModel.Keno.KenoResultMetalModel setting, string userId, DateTime resultDate)
        {
            var authorizedData = this.Authorize(setting, userId);
            if (authorizedData.ErrorCode > 0)
            {
                throw new ServiceException(authorizedData.ErrorMsg, SERVICENAME);
            }

            var kenoResultUrl = new StringBuilder(setting.ResultUrl);
            kenoResultUrl.Append("?Authorization=" + authorizedData.Data.Authorization);
            kenoResultUrl.Append("&cust=" + GAMENAME);
            kenoResultUrl.Append("&siteID=" + setting.SiteID);
            kenoResultUrl.Append("&action=R");
            kenoResultUrl.Append("&lang=" + setting.Language);
            kenoResultUrl.Append("&date=" + resultDate.ToString("yyyyMMdd"));
            kenoResultUrl.Append("&redirectURL=" + HttpUtility.UrlEncode(setting.KickOutUrl));
            return kenoResultUrl.ToString();
        }

        private static string DownLoadData(string url)
        {
            using (var client = new WebClient())
            {
                client.Headers[HttpRequestHeader.ContentType] = "application/json";
                return client.DownloadString(url);
            }
        }
    }
}