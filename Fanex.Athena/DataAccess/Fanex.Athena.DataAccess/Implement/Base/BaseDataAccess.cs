// -----------------------------------------------------------------------
// <copyright file="BaseDataAccess.cs" company="Nexcel Solution">
// TODO: Update copyright text.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
// -----------------------------------------------------------------------
// <history>
//  <change who="Bravo" date="2013.09.19">Apply Style Cop</change>
// </history>
// -----------------------------------------------------------------------

namespace Fanex.Athena.DataAccess.Implement.Base
{
    using System.Data;
    using Fanex.Athena.Models.DTO.Common;
    using Fanex.Athena.Models.DTO.Configuration;
    using Fanex.Common;
    using Fanex.Data;

    /// <summary>
    ///   The data access base.
    /// </summary>
    public abstract class BaseDataAccess
    {
        #region Constructors and Destructors

        protected BaseDataAccess()
        {
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref="BaseDataAccess" /> class.
        /// </summary>
        /// <param name="balanceUp"> The balance up. </param>
        protected BaseDataAccess(BalanceUp balanceUp)
        {
            this.BalanceUp = balanceUp;
        }

        #endregion Constructors and Destructors

        #region Public Properties

        public BalanceUp BalanceUp { get; set; }

        #endregion Public Properties

        #region Protected

        #region Get result DB

        /// <summary>
        ///  The add result parameter.
        /// </summary>
        /// <param name="db"> The database. </param>
        [System.CLSCompliant(false)]
        protected static void AddResultParam(ObjectDb db)
        {
            db.AddOutParameter("ErrCode", DbType.Int32, 4);
            db.AddOutParameter("ErrMsg", DbType.String, 4000);
        }

        /// <summary>
        ///   Get result.
        /// </summary>
        /// <param name="db"> The database. </param>
        /// <returns> Result value.</returns>
        [System.CLSCompliant(false)]
        protected static IResultInfo GetResult(ObjectDb db)
        {
            var errorCode = db.GetParameterValue("ErrCode");
            var errorMessage = db.GetParameterValue("ErrMsg");

            return new ResultInfo
            {
                Code = Converter.ToInt(errorCode),
                Message = Converter.ToString2(errorMessage)
            };
        }

        #endregion Get result DB

        #endregion Protected
    }
}