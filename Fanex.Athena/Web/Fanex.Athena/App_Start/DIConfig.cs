using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using Autofac;
using Autofac.Integration.Mvc;

namespace Fanex.Athena
{
    public static class DIConfig
    {
        public static void Register()
        {
            var builder = new ContainerBuilder();
            builder.RegisterAssemblyTypes(GetAssemblies(false)).AsImplementedInterfaces();
            builder.RegisterControllers(GetAssemblies(true));

            // Set the dependency resolver to be Autofac.
            var container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        }

        private static Assembly[] GetAssemblies(bool isController)
        {
            var path = HttpContext.Current.Server.MapPath("~/bin");
            var filePaths = Directory.GetFiles(path, "*.dll").Where(x =>
            {
                var filename = Path.GetFileName(x);
                return isController ?
                    filename.Contains("Fanex.Athena.Controllers") :
                    filename.Contains("Fanex.Athena") && !filename.Contains(".Controllers");
            });

            return filePaths.Select(Assembly.LoadFrom).ToArray();
        }
    }
}