namespace Fanex.Athena.Service.BetList.Interfaces
{
    using Fanex.Athena.Model.BetList.MetaModel;
    using Model.BetList.ViewModel;

    public interface IColossusBetBetListService
    {
        ColossusBetDetailViewModel GetData(ColossusBetDetailMetaModel metaModel);

        int GetSiteId(int custId);
    }
}