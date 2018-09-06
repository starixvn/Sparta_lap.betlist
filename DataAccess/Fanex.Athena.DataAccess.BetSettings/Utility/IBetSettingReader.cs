using System.Data;
using Fanex.Athena.Model.BetSettings.DTO;

namespace Fanex.Athena.DataAccess.BetSettings.Utility
{
    public interface IBetSettingReader
    {
        BetSetting ReadForSport(IDataReader reader, string sportId, bool isNumberGame = false);
    }
}