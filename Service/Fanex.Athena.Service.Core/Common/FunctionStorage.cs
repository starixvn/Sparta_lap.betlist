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

        public virtual IEnumerable<Function> FunctionList
        {
            get
            {
                InitilizeData();
                return _functionList;
            }
        }

        public virtual IEnumerable<Function> MasterFunctions
        {
            get
            {
                InitilizeData();
                return _masterFunctions;
            }
        }

        public virtual IDictionary<int, IEnumerable<Function>> SubFunctions
        {
            get
            {
                InitilizeData();
                return _subFunctions;
            }
        }

        private static void AssignData(IEnumerable<Function> functionList)
        {
            _functionList = functionList;

            _masterFunctions = _functionList.Where(f => f.ParentId == 0)
                                         .OrderBy(f => f.DisplayOrder)
                                         .ToList();

            _subFunctions = new Dictionary<int, IEnumerable<Function>>();
            foreach (var function in _masterFunctions)
            {
                var subFunctions = _functionList.Where(f => f.ParentId == function.FunctionId)
                                             .OrderBy(f => f.DisplayOrder)
                                             .ToList();
                _subFunctions.Add(function.FunctionId, subFunctions);
            }
        }

        private void InitilizeData()
        {
            if (_functionList == null)
            {
                var functionList = _functionDataAccess.GetFunctionList();
                AssignData(functionList);
            }
        }
    }
}