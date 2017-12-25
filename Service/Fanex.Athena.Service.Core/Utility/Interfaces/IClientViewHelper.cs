using Fanex.Athena.Model.Core.DataDomain;
using Fanex.Athena.Model.Core.DTO.Common;

namespace Fanex.Athena.Service.Core.Helpers
{
    public interface IClientViewHelper
    {
        ViewByOption GetViewPath(string viewPath, ClientViewType viewtype);
    }
}