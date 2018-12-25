// -----------------------------------------------------------------------
// <copyright file="BaseDataAccess.cs" company="">
// TODO: Update copyright text.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Fanex.Athena.DataAccess.Core
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using Extension.Utility;
    using Fanex.Athena.Model.Core.DTO.Common;
    using Fanex.Common;
    using Fanex.Data;
    using Session.Interfaces;

    public class BaseDataAccess
    {
        [CLSCompliant(false)]
        protected static void AddResultParam(IObjectDb db)
        {
            db.AddOutParameter("ErrCode", DbType.Int32, 4);
            db.AddOutParameter("ErrMsg", DbType.String, 4000);
        }

        [CLSCompliant(false)]
        protected static IResultInfo GetResult(IObjectDb db)
        {
            var result = new ResultInfo();
            AssignResult(db, result);
            return result;
        }

        [CLSCompliant(false)]
        protected static IResultInfo<T> GetResult<T>(IObjectDb db)
        {
            var result = new ResultInfo<T>();
            AssignResult(db, result);
            return result;
        }

        [CLSCompliant(false)]
        protected internal virtual IObjectDb GetObjectDb(string spname, int timeout = 120)
        {
            return new ObjectDb(spname, timeout);
        }

        private static void AssignResult(IObjectDb db, IResultInfo result)
        {
            var errorCode = db.GetParameterValue("ErrCode");
            var errorMessage = db.GetParameterValue("ErrMsg");

            result.Code = Converter.ToInt(errorCode);
            result.Message = Converter.ToString2(errorMessage);
        }
    }
}