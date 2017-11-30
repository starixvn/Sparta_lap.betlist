namespace Fanex.Athena.UnitTest.Common.Extensions
{
    public static class ObjectExtensions
    {
        public static object GetValue(this object item, string property)
        {
            return item.GetType().GetProperty(property).GetValue(item, null);
        }
    }
}