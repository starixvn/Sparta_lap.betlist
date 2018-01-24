namespace Fanex.Athena.Model.Core.MetaModel.Authorization
{
    public class SsoMetaModel
    {
        public string LicenseeName { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }

#pragma warning disable S3996 // URI properties should not be strings
        public string LoginUrl { get; set; }
#pragma warning restore S3996 // URI properties should not be strings

        public string LanguageCode { get; set; }

        public long TimeStamp { get; set; }

        public string IdentityName
        {
            get
            {
                return LicenseeName + "_" + UserName;
            }
        }
    }
}