namespace Fanex.Athena.Model.Core.DataDomain
{
    using System;

    [Flags]
    public enum AppMode : byte
    {
        Self = 0x0,
        Parasitic = 0x1
    }
}