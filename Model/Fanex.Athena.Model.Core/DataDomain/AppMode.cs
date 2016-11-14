using System;

namespace Fanex.Athena.Model.Core.DataDomain
{
    [Flags]
    public enum AppMode : byte
    {
        Self = 0,
        Parasitic = 1
    }
}
