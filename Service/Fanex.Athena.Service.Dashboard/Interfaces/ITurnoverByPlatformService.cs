namespace Fanex.Athena.Service.Dashboard.Interfaces
{
    using Fanex.Athena.Model.Dashboard.ViewModel;

    public interface ITurnoverByPlatformService
    {
        TurnoverByPlatformViewModel GetData();
    }
}