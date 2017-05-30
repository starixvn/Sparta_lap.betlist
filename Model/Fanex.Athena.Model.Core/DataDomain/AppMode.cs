namespace Fanex.Athena.Model.Core.DataDomain
{
    using System;

    [Flags]
    public enum AppMode : int
    {
        Self = 0,
        Parasitic = 1
    }
}