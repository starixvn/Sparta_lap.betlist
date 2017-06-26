namespace Fanex.Athena.DataAccess.BetSettings.Interfaces
{
    using System.Collections.Generic;
    using Fanex.Athena.Model.BetSettings.DTO;

    public interface IBetradarVsBetSettingDataAccess
    {
        IList<BetSetting> GetBetSetting(int custId);
    }
}