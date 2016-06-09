// -----------------------------------------------------------------------
// <copyright file="CasinoStatusMetaModel.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Model.Casino.MetaModel
{
    #region Using

    using Fanex.Athena.Model.Customer.MetaModel;

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