using System;

namespace Fanex.Athena.BetList.Service.Interfaces
{
    [CLSCompliant(false)]
    public interface IIPService
    {
        bool IsVNIP(string ipAddress);

        bool IsAlwaysIpvn(string ipAddress);
    }
}