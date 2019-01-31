namespace Fanex.Athena.BetList.DataAccess.Interfaces
{
    using Fanex.Athena.BetList.Model.MetaModel;
    using Model.ViewModel;

    public interface IColossusBetBetListDataAccess
    {
        ColossusBetDetailViewModel GetData(ColossusBetDetailMetaModel metaModel);

        int GetSiteId(int custId);
    }
}