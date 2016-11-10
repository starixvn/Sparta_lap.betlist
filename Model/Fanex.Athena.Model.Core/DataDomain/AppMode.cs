using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fanex.Athena.Model.Core.DataDomain
{
    [Flags]
    public enum AppMode : byte
    {
        Self = 0,
        Parasitic = 1
    }
}
