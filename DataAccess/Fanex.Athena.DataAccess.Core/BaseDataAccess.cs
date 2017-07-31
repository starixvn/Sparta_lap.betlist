// -----------------------------------------------------------------------
// <copyright file="BaseDataAccess.cs" company="">
// TODO: Update copyright text.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Fanex.Athena.DataAccess.Core
{
    using System;
    using System.Data;
    using Fanex.Athena.Model.Core.DTO.Common;
    using Fanex.Common;
    using Fanex.Data;

    public abstract class BaseDataAccess
    {
        protected BaseDataAccess()
        {
        }

        [CLSCompliant(false)]
        protected static void AddResultParam(IObjectDb db)
        {
            db.AddOutParameter("ErrCode", DbType.Int32, 4);
            db.AddOutParameter("ErrMsg", DbType.String, 4000);
        }

        [CLSCompliant(false)]
        protected static IResultInfo GetResult(IObjectDb db)
        {
            var errorCode = db.GetParameterValue("ErrCode");
            var errorMessage = db.GetParameterValue("ErrMsg");

            return new ResultInfo
            {
                Code = Converter.ToInt(errorCode),
                Message = Converter.ToString2(errorMessage)
            };
        }

        [CLSCompliant(false)]
        protected internal virtual IObjectDb GetObjectDb(string spname)
        {
            return new ObjectDb(spname);
        }
    }
}