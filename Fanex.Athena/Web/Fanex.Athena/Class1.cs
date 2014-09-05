using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Fanex.Athena
{
    enum SiteEnvironment
    {
        Local,
        Uat,
        Prod,
        SandBox
    }

    public class SiteInfo
    {
        //Web config value.
        public SiteEnvironment Environment { get; set; }

        public string LogFileValue
        {
            get
            {
                switch (Environment)
                {
                    case SiteEnvironment.Local:
                        return "E:/Licensee/OlympusRoot/ALLWebSite/LRF/Site3/log4net.txt";
                    case SiteEnvironment.Uat:
                        return "E:/3.Logs/lrf/log4net.txt";
                    case SiteEnvironment.Prod:
                        return "E:/3.Logs/lrf/log4net.txt";
                    case SiteEnvironment.SandBox:
                        return "E:/Licensee/OlympusRoot/ALLWebSite/LRF/Site3/log4net.txt";
                    default:
                        return string.Empty;
                }
            }
        }

        public string BucketServerUri
        {
             get
            {
                switch (Environment)
                {
                    case SiteEnvironment.Local:
                        return "http://10.18.200.209:8091/pools/default";
                    case SiteEnvironment.Uat:
                        return "http://10.40.40.128:8091/pools/default";
                    case SiteEnvironment.Prod:
                        return "http://10.40.40.128:8091/pools/default";
                    case SiteEnvironment.SandBox:
                        return "http://10.18.200.209:8091/pools/default";
                    default:
                        return string.Empty;
                }
            }
        }

        public string LoggingServer
        {
            get
            {
                switch (Environment)
                {
                    case SiteEnvironment.Local:
                        return "10.18.200.209";
                    case SiteEnvironment.Uat:
                        return "10.40.40.73";
                    case SiteEnvironment.Prod:
                        return "10.40.40.127";
                    case SiteEnvironment.SandBox:
                        return "10.18.200.209";
                    default:
                        return string.Empty;
                }
            }
        }
    }
}