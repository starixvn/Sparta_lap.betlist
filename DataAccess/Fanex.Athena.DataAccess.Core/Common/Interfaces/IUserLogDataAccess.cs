// -----------------------------------------------------------------------
// <copyright file="IUserLogDataAccess.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

using Fanex.Athena.Model.Core.MetaModel.Common;

namespace Fanex.Athena.DataAccess.Core.Common.Interfaces
{
    public interface IUserLogDataAccess
    {
        void WriteLog(UserLogMetaModel metaModel);
    }
}