using System.Web.Http.Dependencies;

namespace Fanex.Athena.Api.Base.Attributes.Extensions
{
    public static class DependencyScopeExtension
    {
        public static T Resolve<T>(this IDependencyScope dependencyScope) where T : class
        {
            return dependencyScope.GetService(typeof(T)) as T;
        }
    }
}