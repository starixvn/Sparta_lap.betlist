namespace Fanex.Athena.Service.Casino.Wrappers
{
    using System.Data;
    using CasinoLibrary.SBAgent;
    using Fanex.Athena.Model.Casino.MetaModel.ApiInputParameter;

    public class BasicInfoApiWrapper : IBasicInfoApiWrapper
    {
        private readonly BasicInfo _basicInfoApi;

        public BasicInfoApiWrapper()
        {
            _basicInfoApi = new BasicInfo();
        }

        public DataTable GetGameKind(string casinoNo)
        {
            return new ApiMethodOperationHandler<DataTable>().Handle(
                casinoNo, 
                (obj) => _basicInfoApi.GetGameKind(obj));
        }

        public DataTable GetGameType(GetGameTypeMetalModel input)
        {
            return new ApiMethodOperationHandler<DataTable>().Handle<GetGameTypeMetalModel>(
                input, 
                (obj) => _basicInfoApi.GetGameType(obj.GameKind, obj.SbCustID, obj.SbExchangeID));
        }
    }
}