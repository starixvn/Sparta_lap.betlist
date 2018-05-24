using Fanex.Athena.Model.Core.DTO.Common;
using Fanex.Athena.Session.Enum;

namespace Fanex.Athena.Service.Core.Helpers
{
    public interface IClientViewHelper
    {
        ViewByOption GetViewPath(string viewPath, ClientViewType viewtype);
    }
}