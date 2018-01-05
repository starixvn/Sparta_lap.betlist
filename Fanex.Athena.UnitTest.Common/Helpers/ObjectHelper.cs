using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Fanex.Athena.UnitTest.Common.Helpers
{
    public static class ObjectHelper
    {
        public static IDictionary<string, object> ToDictionary(object value)
        {
            return value.GetType()
                    .GetProperties(BindingFlags.Instance | BindingFlags.Public)
                    .ToDictionary(prop => prop.Name, prop => prop.GetValue(value, null));
        }
    }
}