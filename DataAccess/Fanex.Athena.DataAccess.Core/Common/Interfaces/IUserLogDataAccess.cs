// -----------------------------------------------------------------------
// <copyright file="IUserLogDataAccess.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

using System.Threading.Tasks;
using Fanex.Athena.Model.Core.MetaModel.Common;

namespace Fanex.Athena.DataAccess.Core.Common.Interfaces
{
    public interface IUserLogDataAccess
    {
        Task WriteLog(UserLogMetaModel metaModel);
    }
}