namespace Fanex.Athena.Service.Core.Common
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Fanex.Athena.DataAccess.Core.Common.Interfaces;
    using Fanex.Athena.Model.Core.DTO.Common;
    using Interfaces;

    public class FunctionStorage : IFunctionStorage
    {
        private static readonly object LockFunctionList = new object();

        private static readonly object LockMasterFunction = new object();

        private static readonly object LockSubFunction = new object();

        private static IEnumerable<Function> _functionList;

        private static IEnumerable<Function> _masterFunctions;

        private static IDictionary<int, IEnumerable<Function>> _subFunctions;

        private readonly IFunctionDataAccess _functionDataAccess;

        public FunctionStorage(IFunctionDataAccess functionDataAccess)
        {
            _functionDataAccess = functionDataAccess;
        }

        public async Task<IEnumerable<Function>> GetFunctionList()
        {
            if (_functionList == null)
            {
                var functionList = await _functionDataAccess.GetFunctionList().ConfigureAwait(false);
                AssignFunctionList(functionList);
            }

            return _functionList;
        }

        public async Task<IEnumerable<Function>> GetMasterFunctions()
        {
            if (_masterFunctions == null)
            {
                AssignMasterFunctionList(await GetFunctionList().ConfigureAwait(false));
            }

            return _masterFunctions;
        }

        public async Task<IDictionary<int, IEnumerable<Function>>> GetSubFunctions()
        {
            if (_subFunctions == null)
            {
                AssignSubFunctionList(await GetMasterFunctions().ConfigureAwait(false));
            }

            return _subFunctions;
        }

        private static void AssignFunctionList(IEnumerable<Function> functionList)
        {
            lock (LockFunctionList)
            {
                if (_functionList == null)
                {
                    _functionList = functionList;
                }
            }
        }

        private static void AssignMasterFunctionList(IEnumerable<Function> functionList)
        {
            lock (LockMasterFunction)
            {
                if (_masterFunctions == null)
                {
                    _masterFunctions = functionList.Where(f => f.ParentId == 0)
                                                 .OrderBy(f => f.DisplayOrder)
                                                 .ToList();
                }
            }
        }

        private static void AssignSubFunctionList(IEnumerable<Function> masterFunctionList)
        {
            lock (LockSubFunction)
            {
                if (_subFunctions == null)
                {
                    _subFunctions = new Dictionary<int, IEnumerable<Function>>();
                    foreach (var function in masterFunctionList)
                    {
                        var subFunctions = _functionList.Where(f => f.ParentId == function.FunctionId)
                                                     .OrderBy(f => f.DisplayOrder)
                                                     .ToList();
                        _subFunctions.Add(function.FunctionId, subFunctions);
                    }
                }
            }
        }
    }
}