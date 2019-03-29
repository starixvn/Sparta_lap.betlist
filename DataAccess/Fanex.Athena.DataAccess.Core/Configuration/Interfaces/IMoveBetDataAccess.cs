// -----------------------------------------------------------------------
// <copyright file="IMoveBetDataAccess.cs" company="">
// TODO: Update copyright text.
// </copyright>

namespace Fanex.Athena.DataAccess.Core.Configuration.Interfaces
{
    using System.Threading.Tasks;
    using Fanex.Athena.Model.Core.DTO.Configuration;

    public interface IMoveBetDataAccess
    {
        Task<MoveBetNotification> GetMoveBetNotificationAsync();
    }
}