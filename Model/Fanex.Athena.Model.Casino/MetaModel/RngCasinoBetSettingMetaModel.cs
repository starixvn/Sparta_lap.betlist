// -----------------------------------------------------------------------
// <copyright file="RngCasinoBetSettingMetaModel.cs" company="Nexcel">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Model.Casino.MetaModel
{
    using System.Data;

    /// <summary>
    /// RNGCasinoBetSettingMetaModel class.
    /// </summary>
    public class RngCasinoBetSettingMetaModel : CasinoStatusMetaModel
    {
        public int BetTypeId { get; set; }

        public int RoleId
        {
            get
            {
                return 1;
            }
        }

        public string StakeLevelList { get; set; }

        public DataTable TableParam { get; set; }
    }
}