// -----------------------------------------------------------------------
// <copyright file="HashtableExtensions.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Extension
{
    using System.Collections;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public static class HashtableExtensions
    {
        public static void UpdateWith(this Hashtable first, Hashtable second)
        {
            foreach (DictionaryEntry item in second)
            {
                if (first.ContainsKey(item.Key))
                {
                    first[item.Key] = item.Value;
                }
            }
        }
    }
}
