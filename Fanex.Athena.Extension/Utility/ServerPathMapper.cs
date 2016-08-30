namespace Fanex.Athena.Extension.Utility
{
    using System.Web.Hosting;
    using Interfaces;

    public class ServerPathMapper : IServerPathMapper
    {
        public string MapPath(string virtualPath)
        {
            return HostingEnvironment.MapPath(virtualPath);
        }
    }
}