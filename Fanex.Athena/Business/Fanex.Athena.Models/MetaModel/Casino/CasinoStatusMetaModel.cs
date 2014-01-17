// -----------------------------------------------------------------------
// <copyright file="CasinoStatusMetaModel.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Models.MetaModel.Casino
{
    #region Using

    using Fanex.Athena.Models.MetaModel.Customer;

    #endregion Using

    /// <summary>
    /// CasinoStatusMetaModel class.
    /// </summary>
    public class CasinoStatusMetaModel : CustomerStatusMetaModel
    {
        public int SystemId
        {
            get
            {
                return 4;
            }
        }
    }
}