using System;

namespace Fanex.Athena.Result.Controller.UnitTests.ResultPopup.TestHelper
{
    public static class InvokeTestHelper
    {
        public static object Invoke(object instance, string methodName, params object[] parameters)
        {
            var type = instance.GetType();
            var method = type.GetMethod(methodName);

            if (method != null)
            {
                return method.Invoke(instance, parameters);
            }

            throw new InvalidOperationException($"Method {methodName} not found.");
        }
    }
}
