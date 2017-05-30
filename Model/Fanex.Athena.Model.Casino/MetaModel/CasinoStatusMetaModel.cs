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