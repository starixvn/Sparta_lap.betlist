using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using Autofac;
using Autofac.Integration.Mvc;
using Autofac.Integration.WebApi;

namespace Fanex.Athena
{
    public static class DIConfig
    {
        public static void Register()
        {
            var builder = new ContainerBuilder();
            var config = GlobalConfiguration.Configuration;

            var assemblyFilePaths = GetAssemblyFilePaths();

            builder.RegisterAssemblyTypes(GetServiceAssemblies(assemblyFilePaths)).AsImplementedInterfaces();
            builder.RegisterControllers(GetControllerAssemblies(assemblyFilePaths));
            builder.RegisterApiControllers(GetApiControllerAssemblies(assemblyFilePaths));

            // Set the dependency resolver to be Autofac.
            var container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
            config.DependencyResolver = new AutofacWebApiDependencyResolver(container);
        }

        private static Assembly[] GetServiceAssemblies(string[] assemblyFilePaths)
        {
            string[] controllerFileNames = new[] { "Fanex.Athena.Controllers", "Fanex.Athena.Api" };
            var filePaths = assemblyFilePaths.Where(p =>
            {
                var fileName = Path.GetFileName(p);
                return fileName.Contains("Fanex.Athena") && !controllerFileNames.Any(x => fileName.Contains(x));
            });

            return filePaths.Select(Assembly.LoadFrom).ToArray();
        }

        private static Assembly[] GetControllerAssemblies(string[] assemblyFilePaths)
        {
            var filePaths = assemblyFilePaths.Where(p => Path.GetFileName(p).Contains("Fanex.Athena.Controllers"));
            return filePaths.Select(Assembly.LoadFrom).ToArray();
        }

        private static Assembly[] GetApiControllerAssemblies(string[] assemblyFilePaths)
        {
            var filePaths = assemblyFilePaths.Where(p => Path.GetFileName(p).Contains("Fanex.Athena.Api"));
            return filePaths.Select(Assembly.LoadFrom).ToArray();
        }

        private static string[] GetAssemblyFilePaths()
        {
            var path = HttpContext.Current.Server.MapPath("~/bin");
            return Directory.GetFiles(path, "*.dll");
        }
    }
}