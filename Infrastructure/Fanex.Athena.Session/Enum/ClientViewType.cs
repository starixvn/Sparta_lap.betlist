namespace Fanex.Athena.Session.Enum
{
    using System;

    [Flags]
    public enum ClientViewType : int
    {
        None = 0,
        Default = 1,
        Desktop = 2
    }
}