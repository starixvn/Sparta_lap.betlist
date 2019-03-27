namespace Fanex.Athena.Service.Core.Common.Interfaces
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Fanex.Athena.Model.Core.DTO.Common;

    public interface IFunctionStorage
    {
        Task<IEnumerable<Function>> GetFunctionList();

        Task<IEnumerable<Function>> GetMasterFunctions();

        Task<IDictionary<int, IEnumerable<Function>>> GetSubFunctions();
    }
}