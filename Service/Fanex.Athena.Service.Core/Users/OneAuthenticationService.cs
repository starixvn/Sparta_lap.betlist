using System;
using System.IO;
using System.Web.Hosting;
using Fanex.Athena.Model.Core.DTO.Common;
using Fanex.Athena.Service.Core.Users.Interfaces;
using Newtonsoft.Json;

namespace Fanex.Athena.Service.Core.Users
{
    public class OneAuthenticationService : IOneAuthenticationService
    {
        public IResultInfo InsertLicenseeUser(OneAuthenticationToken tokenInfo)
        {
            throw new NotImplementedException();
        }

        public IResultInfo SignIn(int licenseeId, OneAuthenticationToken tokenInfo)
        {
            throw new NotImplementedException();
        }

        public LicenseeSiteName SingleLicenseeSiteName(string siteName)
        {
            const string siteNameFilePath = "~/App_Data/LicenseeSiteName.json";
            if (!string.IsNullOrWhiteSpace(siteName))
            {
                using (var _textReader = GetStreamReader(HostingEnvironment.MapPath(siteNameFilePath)))
                {
                    var temp = _textReader.ReadToEnd();
                    if (!string.IsNullOrEmpty(temp))
                    {

                    }
                }
            }

            return new LicenseeSiteName();
        }

        internal virtual TextReader GetStreamReader(string path)
        {
            return new StreamReader(path);
        }
    }
}
