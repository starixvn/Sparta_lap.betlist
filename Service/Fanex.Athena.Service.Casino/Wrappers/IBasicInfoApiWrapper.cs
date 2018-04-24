namespace Fanex.Athena.Service.Casino.Wrappers
{
    using Fanex.Athena.Model.Casino.MetaModel.ApiInputParameter;
    using System.Data;

    public interface IBasicInfoApiWrapper
    {
        DataTable GetGameKind(string casinoNo);

        DataTable GetGameType(GetGameTypeMetalModel input);
    }
}