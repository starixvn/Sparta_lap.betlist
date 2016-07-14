// <copyright file="JsonHelper.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Extension.Helper
{
    using System.Web.Script.Serialization;

    /// <summary>
    /// JSON helper class.
    /// </summary>
    public static class JsonHelper
    {
        public static string ToJson(this object value)
        {
            var serializer = new JavaScriptSerializer();

            return serializer.Serialize(value);
        }

        public static string ToJson(this object value, int recursionDepth)
        {
            var serializer = new JavaScriptSerializer { RecursionLimit = recursionDepth };
            return serializer.Serialize(value);
        }
    }
}