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
            string[] controllerFileNames = new[] { "Fanex.Athena.Controllers", "Fanex.Athena.Api" };
            var path = HttpContext.Current.Server.MapPath("~/bin");
            var filePaths = Directory.GetFiles(path, "*.dll").Where(p =>
            {
                var fileName = Path.GetFileName(p);
                return isController ?
                     controllerFileNames.Any(x => fileName.Contains(x)) :
                    (fileName.Contains("Fanex.Athena") && !controllerFileNames.Any(x => fileName.Contains(x)));
            });

            return filePaths.Select(Assembly.LoadFrom).ToArray();
        }
    }
}