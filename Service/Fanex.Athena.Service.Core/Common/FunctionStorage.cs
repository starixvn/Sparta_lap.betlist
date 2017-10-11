namespace Fanex.Athena.Service.Core.Common
{
    using System.Collections.Generic;
    using System.Linq;
    using Fanex.Athena.DataAccess.Core.Common;
    using Fanex.Athena.DataAccess.Core.Common.Interfaces;
    using Fanex.Athena.Model.Core.DTO.Common;
    using Interfaces;

    public class FunctionStorage : IFunctionStorage
    {
        private static readonly object _lockFunctionList = new object();

        private static readonly object _lockMasterFunction = new object();

        private static readonly object _lockSubFunction = new object();

        private static IEnumerable<Function> _functionList;

        private static IEnumerable<Function> _masterFunctions;

        private static IDictionary<int, IEnumerable<Function>> _subFunctions;

        private readonly IFunctionDataAccess _functionDataAccess;

        public FunctionStorage()
        {
            _functionDataAccess = new FunctionDataAccess();
        }

        public FunctionStorage(IFunctionDataAccess functionDataAccess)
        {
            _functionDataAccess = functionDataAccess;
        }

        public IEnumerable<Function> FunctionList
        {
            get
            {
                if (_functionList == null)
                {
                    var functionList = _functionDataAccess.GetFunctionList();
                    AssignFunctionList(functionList);
                }

                return _functionList;
            }
        }

        public IEnumerable<Function> MasterFunctions
        {
            get
            {
                if (_masterFunctions == null)
                {
                    AssignMasterFunctionList(FunctionList);
                }

                return _masterFunctions;
            }
        }

        public IDictionary<int, IEnumerable<Function>> SubFunctions
        {
            get
            {
                if (_subFunctions == null)
                {
                    AssignSubFunctionList(MasterFunctions);
                }

                return _subFunctions;
            }
        }

        private static void AssignFunctionList(IEnumerable<Function> functionList)
        {
            lock (_lockFunctionList)
            {
                if (_functionList == null)
                {
                    _functionList = functionList;
                }
            }
        }

        private static void AssignMasterFunctionList(IEnumerable<Function> functionList)
        {
            lock (_lockMasterFunction)
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
            lock (_lockSubFunction)
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