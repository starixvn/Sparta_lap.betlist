// <copyright file="JsonHelper.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Extension.Helper
{
    using System.Web.Script.Serialization;

    public static class JsonHelper
    {
#pragma warning disable S4225 // Extension methods should not extend "object"
        public static string ToJson(this object value)
#pragma warning restore S4225 // Extension methods should not extend "object"
        {
            var serializer = new JavaScriptSerializer();

            return serializer.Serialize(value);
        }
    }
}