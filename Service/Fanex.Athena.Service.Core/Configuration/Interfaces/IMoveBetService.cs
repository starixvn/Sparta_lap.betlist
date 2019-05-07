// -----------------------------------------------------------------------
// <copyright file="IMoveBetService.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------
using System.Threading.Tasks;

namespace Fanex.Athena.Service.Core.Configuration.Interfaces
{
    public interface IMoveBetService
    {
        Task<string> GetMoveBetNotificationAsync(string language);
    }
}