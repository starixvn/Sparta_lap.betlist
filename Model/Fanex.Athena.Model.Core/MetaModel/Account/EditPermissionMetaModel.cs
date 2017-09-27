// -----------------------------------------------------------------------
// <copyright file="EditPermissionMetaModel.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Model.Core.MetaModel.Account
{
    using System.Collections.Generic;
    using Fanex.Athena.Model.Core.DTO.Common;

    public class EditPermissionMetaModel
    {
        public int UserId { get; set; }

        public string FunctionIdList { get; set; }

        public IEnumerable<Function> AllMasterFunctions { get; set; }

        public IDictionary<int, IEnumerable<Function>> AllFunctionMap { get; set; }

        public IEnumerable<Function> AllUserFunctions { get; set; }
    }
}