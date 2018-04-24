namespace Fanex.Athena.Service.Casino.Wrappers
{
    using System.Data;
    using Fanex.Athena.Model.Casino.MetaModel.ApiInputParameter;

    public interface IBasicInfoApiWrapper
    {
        DataTable GetGameKind(string casinoNo);

        DataTable GetGameType(GetGameTypeMetalModel input);
    }
}