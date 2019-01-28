namespace Fanex.Athena.DataAccess.BetList.Interfaces
{
    using Fanex.Athena.Model.BetList.MetaModel;
    using Model.BetList.ViewModel;

    public interface IColossusBetBetListDataAccess
    {
        ColossusBetDetailViewModel GetData(ColossusBetDetailMetaModel metaModel);

        int GetSiteId(int custId);
    }
}