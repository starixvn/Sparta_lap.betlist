namespace Fanex.Athena.Result.Model
{
    using System.Configuration;

    public sealed class Indo4DConfig : ConfigurationSection
    {
        private const string __INDO4D_RESULT_CONFIG = "Indo4DConfig";
        private const string URLRESULT = "UrlResult";
        private const string LOGIN = "Login";
        private const string PASSWORD = "Password";
        private const string SITE_ID = "SiteId";
        private const string DEBUG_MODE = "Debug";

        private static Indo4DConfig _configs = ConfigurationManager.GetSection(__INDO4D_RESULT_CONFIG) as Indo4DConfig;

        public static Indo4DConfig Configs
        {
            get
            {
                return _configs;
            }
        }

        [ConfigurationProperty(URLRESULT, IsRequired = true)]
        public string HrefResult
        {
            get { return this[URLRESULT].ToString(); }
        }

        [ConfigurationProperty(LOGIN, IsRequired = true)]
        public string Login
        {
            get { return this[LOGIN].ToString(); }
        }

        [ConfigurationProperty(PASSWORD, IsRequired = true)]
        public string Password
        {
            get { return this[PASSWORD].ToString(); }
        }

        [ConfigurationProperty(SITE_ID, IsRequired = true)]
        public string SiteId
        {
            get { return this[SITE_ID].ToString(); }
        }

        [ConfigurationProperty(DEBUG_MODE, IsRequired = false, DefaultValue = 0)]
        public int DebugMode
        {
            get { return (int)this[DEBUG_MODE]; }
        }
    }
}