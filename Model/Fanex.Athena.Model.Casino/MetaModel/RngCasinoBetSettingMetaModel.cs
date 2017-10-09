// -----------------------------------------------------------------------
// <copyright file="RngCasinoBetSettingMetaModel.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Model.Casino.MetaModel
{
    using System.Data;
    using Customer.MetaModel;

    /// <summary>
    /// RNGCasinoBetSettingMetaModel class.
    /// </summary>
    public class RngCasinoBetSettingMetaModel : CustomerStatusMetaModel
    {
        public DataTable TableParam { get; set; }
    }
}