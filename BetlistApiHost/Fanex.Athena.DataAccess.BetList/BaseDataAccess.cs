namespace Fanex.Athena.DataAccess.BetList
{
    using System;
    using Fanex.Data;

    public class BaseDataAccess
    {
        [CLSCompliant(false)]
        protected internal virtual IObjectDb GetObjectDb(string spname, int timeout = 120)
        {
            return new ObjectDb(spname, timeout);
        }
    }
}