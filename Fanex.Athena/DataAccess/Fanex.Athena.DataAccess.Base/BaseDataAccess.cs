// -----------------------------------------------------------------------
// <copyright file="BaseDataAccess.cs" company="Nexcel Solution">
// TODO: Update copyright text.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Fanex.Athena.DataAccess.Core
{
    using System.Data;
    using Fanex.Athena.Models.DTO.Common;
    using Fanex.Athena.Models.DTO.Configuration;
    using Fanex.Common;
    using Fanex.Data;

    public abstract class BaseDataAccess
    {
        protected BaseDataAccess()
        {
        }

        protected BaseDataAccess(BalanceUp balanceUp)
        {
            this.BalanceUp = balanceUp;
        }

        public BalanceUp BalanceUp { get; set; }

        [System.CLSCompliant(false)]
        protected static void AddResultParam(ObjectDb db)
        {
            db.AddOutParameter("ErrCode", DbType.Int32, 4);
            db.AddOutParameter("ErrMsg", DbType.String, 4000);
        }

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
    }
}