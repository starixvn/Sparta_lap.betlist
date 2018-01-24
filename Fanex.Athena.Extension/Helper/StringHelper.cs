namespace Fanex.Athena.Extension.Helper
{
    public static class StringHelper
    {
        public static bool IsAnyNullOrEmpty(params string[] values)
        {
            foreach (var item in values)
            {
                if (string.IsNullOrEmpty(item))
                {
                    return true;
                }
            }

            return false;
        }
    }
}