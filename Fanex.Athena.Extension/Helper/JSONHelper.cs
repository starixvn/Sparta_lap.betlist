// <copyright file="JsonHelper.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Extension.Helper
{
    using System.Web.Script.Serialization;

    public static class JsonHelper
    {
        public static string ToJson(this object value)
        {
            var serializer = new JavaScriptSerializer();

            return serializer.Serialize(value);
        }
    }
}